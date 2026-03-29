using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthGateway.HealthChecks;

/// <summary>
/// Lightweight HTTP health check for a downstream microservice.
/// Pings each service's /health endpoint and reports status.
/// </summary>
public class ServiceHealthCheck : IHealthCheck
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _serviceUrl;
    private readonly string _serviceName;

    public ServiceHealthCheck(IHttpClientFactory httpClientFactory, string serviceUrl, string serviceName)
    {
        _httpClientFactory = httpClientFactory;
        _serviceUrl = serviceUrl;
        _serviceName = serviceName;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("HealthCheck");
            var response = await client.GetAsync($"{_serviceUrl}/health", cancellationToken);
            return response.IsSuccessStatusCode
                ? HealthCheckResult.Healthy($"{_serviceName} is healthy")
                : HealthCheckResult.Degraded($"{_serviceName} returned {(int)response.StatusCode}");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy($"{_serviceName} is unreachable: {ex.Message}");
        }
    }
}
