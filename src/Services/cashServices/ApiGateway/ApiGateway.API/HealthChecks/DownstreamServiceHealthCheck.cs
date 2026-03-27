using ApiGateway.API.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace ApiGateway.API.HealthChecks;

public sealed class DownstreamServiceHealthCheck : IHealthCheck
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ServiceEndpoints _endpoints;
    private readonly ILogger<DownstreamServiceHealthCheck> _logger;

    public DownstreamServiceHealthCheck(
        IHttpClientFactory httpClientFactory,
        IOptions<ServiceEndpoints> endpoints,
        ILogger<DownstreamServiceHealthCheck> logger)
    {
        _httpClientFactory = httpClientFactory;
        _endpoints = endpoints.Value;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var services = _endpoints.GetAll();
        var results = new Dictionary<string, object>();
        var allHealthy = true;
        var anyHealthy = false;

        using var client = _httpClientFactory.CreateClient("HealthCheck");
        client.Timeout = TimeSpan.FromSeconds(5);

        foreach (var (name, baseUrl) in services)
        {
            try
            {
                var response = await client.GetAsync($"{baseUrl}/health", cancellationToken);
                var healthy = response.IsSuccessStatusCode;

                results[name] = new { status = healthy ? "Healthy" : "Unhealthy", url = baseUrl };

                if (healthy) anyHealthy = true;
                else allHealthy = false;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Health check failed for {Service} at {Url}", name, baseUrl);
                results[name] = new { status = "Unavailable", url = baseUrl, error = ex.Message };
                allHealthy = false;
            }
        }

        if (allHealthy)
            return HealthCheckResult.Healthy("All downstream services are healthy.", results);

        if (anyHealthy)
            return HealthCheckResult.Degraded("Some downstream services are unhealthy.", data: results);

        return HealthCheckResult.Unhealthy("All downstream services are unhealthy.", data: results);
    }
}
