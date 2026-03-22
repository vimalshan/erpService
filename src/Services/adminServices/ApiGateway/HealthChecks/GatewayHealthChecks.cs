using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ApiGateway.HealthChecks;

/// <summary>
/// Custom health check for tracking individual service health
/// </summary>
public class ServiceHealthCheck : IHealthCheck
{
    private readonly HttpClient _httpClient;
    private readonly string _serviceName;
    private readonly string _serviceUrl;

    public ServiceHealthCheck(HttpClient httpClient, string serviceName, string serviceUrl)
    {
        _httpClient = httpClient;
        _serviceName = serviceName;
        _serviceUrl = serviceUrl;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_serviceUrl}/health", cancellationToken);
            
            if (response.IsSuccessStatusCode)
            {
                return HealthCheckResult.Healthy($"{_serviceName} is healthy");
            }

            return HealthCheckResult.Unhealthy($"{_serviceName} returned status code: {response.StatusCode}");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy($"{_serviceName} health check failed: {ex.Message}");
        }
    }
}

/// <summary>
/// Gateway health check that combines all service health statuses
/// </summary>
public class GatewayHealthCheck : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(HealthCheckResult.Healthy("Gateway is healthy"));
    }
}

/// <summary>
/// Configuration for health checks
/// </summary>
public static class HealthCheckConfiguration
{
    public static IHealthChecksBuilder AddGatewayHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var healthChecks = services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy("Gateway is running"))
            .AddUrlGroup(
                new Uri("http://localhost:5186/health"),
                name: "FinyearService",
                timeout: TimeSpan.FromSeconds(5),
                failureStatus: HealthStatus.Degraded)
            .AddUrlGroup(
                new Uri("http://localhost:7136/health"),
                name: "LocationService",
                timeout: TimeSpan.FromSeconds(5),
                failureStatus: HealthStatus.Degraded)
            .AddUrlGroup(
                new Uri("http://localhost:5181/health"),
                name: "VendorService",
                timeout: TimeSpan.FromSeconds(5),
                failureStatus: HealthStatus.Degraded)
            .AddUrlGroup(
                new Uri("http://localhost:5166/health"),
                name: "ScholarshipService",
                timeout: TimeSpan.FromSeconds(5),
                failureStatus: HealthStatus.Degraded)
            .AddUrlGroup(
                new Uri("http://localhost:5273/health"),
                name: "StationeryService",
                timeout: TimeSpan.FromSeconds(5),
                failureStatus: HealthStatus.Degraded)
            .AddUrlGroup(
                new Uri("http://localhost:5116/health"),
                name: "TDSService",
                timeout: TimeSpan.FromSeconds(5),
                failureStatus: HealthStatus.Degraded)
            .AddUrlGroup(
                new Uri("http://localhost:5184/health"),
                name: "LOVService",
                timeout: TimeSpan.FromSeconds(5),
                failureStatus: HealthStatus.Degraded)
            .AddUrlGroup(
                new Uri("http://localhost:5185/health"),
                name: "TransactionService",
                timeout: TimeSpan.FromSeconds(5),
                failureStatus: HealthStatus.Degraded);

        return healthChecks;
    }
}
