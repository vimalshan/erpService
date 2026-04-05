using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ERPGateway.HealthChecks;

/// <summary>
/// Aggregate health check that concurrently probes every downstream service's
/// <c>/health</c> endpoint and rolls the results into a single status.
///
/// Registered as a single named check "downstream-services".
/// Call <c>GET /health/downstream</c> on the gateway to see individual results.
/// </summary>
public sealed class DownstreamHealthCheck : IHealthCheck
{
    private readonly IHttpClientFactory _factory;
    private readonly IConfiguration     _config;

    public DownstreamHealthCheck(IHttpClientFactory factory, IConfiguration config)
    {
        _factory = factory;
        _config  = config;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken  cancellationToken = default)
    {
        var services = _config.GetSection("DownstreamServices")
                              .GetChildren()
                              .ToDictionary(s => s.Key, s => s.Value ?? string.Empty);

        using var client = _factory.CreateClient("healthcheck");

        // Probe all services concurrently.
        var tasks = services.Select(async kvp =>
        {
            var (name, baseUrl) = (kvp.Key, kvp.Value);
            var url = baseUrl.TrimEnd('/') + "/health";
            try
            {
                var response = await client.GetAsync(url, cancellationToken);
                var label    = response.IsSuccessStatusCode
                    ? $"Healthy (HTTP {(int)response.StatusCode})"
                    : $"Degraded (HTTP {(int)response.StatusCode})";

                return (name, label, healthy: response.IsSuccessStatusCode);
            }
            catch (Exception ex)
            {
                return (name, label: $"Unhealthy — {ex.Message}", healthy: false);
            }
        });

        var results   = await Task.WhenAll(tasks);
        var dataDict  = results.ToDictionary(r => r.name, r => (object)r.label);
        var allHealthy = results.All(r => r.healthy);

        return allHealthy
            ? HealthCheckResult.Healthy("All downstream services are reachable.", dataDict)
            : HealthCheckResult.Degraded(
                "One or more downstream services are degraded or unreachable.",
                data: dataDict);
    }
}
