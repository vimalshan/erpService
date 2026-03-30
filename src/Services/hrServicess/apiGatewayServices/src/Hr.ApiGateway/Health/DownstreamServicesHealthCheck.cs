using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Hr.ApiGateway.Health;

public sealed class DownstreamServicesHealthCheck(IConfiguration configuration, IHttpClientFactory httpClientFactory) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var services = configuration.GetSection("HealthTargets").Get<List<DownstreamTarget>>() ?? [];
        if (services.Count == 0)
        {
            return HealthCheckResult.Degraded("No downstream health targets configured.");
        }

        var client = httpClientFactory.CreateClient("gateway-health");
        client.Timeout = TimeSpan.FromSeconds(3);

        var unhealthy = new List<string>();

        foreach (var service in services)
        {
            try
            {
                var baseUrl = service.BaseUrl.TrimEnd('/');
                using var response = await client.GetAsync($"{baseUrl}/health", cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    unhealthy.Add($"{service.Name}:{(int)response.StatusCode}");
                }
            }
            catch
            {
                unhealthy.Add(service.Name);
            }
        }

        if (unhealthy.Count > 0)
        {
            return HealthCheckResult.Unhealthy("Downstream services unavailable", data: new Dictionary<string, object>
            {
                ["failedServices"] = unhealthy
            });
        }

        return HealthCheckResult.Healthy("All downstream services are healthy.");
    }

    public sealed class DownstreamTarget
    {
        public string Name { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = string.Empty;
    }
}
