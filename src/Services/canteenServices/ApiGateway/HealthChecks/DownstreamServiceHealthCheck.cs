using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ApiGateway.HealthChecks;

public class DownstreamServiceHealthCheck : IHealthCheck
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<DownstreamServiceHealthCheck> _logger;

    public DownstreamServiceHealthCheck(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<DownstreamServiceHealthCheck> logger)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken ct = default)
    {
        var endpoints = _configuration.GetSection("ServiceEndpoints").Get<Dictionary<string, string>>() ?? new();
        var results = new Dictionary<string, object>();
        var allHealthy = true;

        var client = _httpClientFactory.CreateClient("HealthCheckClient");

        foreach (var (name, baseUrl) in endpoints)
        {
            try
            {
                var response = await client.GetAsync($"{baseUrl}/health", ct);
                var healthy = response.IsSuccessStatusCode;
                results[name] = new { status = healthy ? "Healthy" : "Unhealthy", statusCode = (int)response.StatusCode };
                if (!healthy) allHealthy = false;
            }
            catch (Exception ex)
            {
                results[name] = new { status = "Unhealthy", error = ex.Message };
                allHealthy = false;
                _logger.LogWarning(ex, "Health check failed for {Service}", name);
            }
        }

        return allHealthy
            ? HealthCheckResult.Healthy("All downstream services are healthy.", results)
            : HealthCheckResult.Degraded("One or more downstream services are unhealthy.", data: results);
    }
}
