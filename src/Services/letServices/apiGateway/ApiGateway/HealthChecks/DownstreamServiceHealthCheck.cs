using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ApiGateway.HealthChecks;

public sealed class DownstreamServiceHealthCheck(
    IHttpClientFactory httpClientFactory,
    IConfiguration configuration,
    ILogger<DownstreamServiceHealthCheck> logger) : IHealthCheck
{
    private record ServiceEndpoint(string Name, string HealthUrl);

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken ct = default)
    {
        var services = new ServiceEndpoint[]
        {
            new("LeaveService",          "http://localhost:5166/health"),
            new("CourseService",         "http://localhost:5215/health"),
            new("RequestService",        "http://localhost:5006/health"),
            new("ReviewService",         "http://localhost:5114/health"),
            new("DevelopmentService",    "http://localhost:5216/health"),
            new("MasterService",         "http://localhost:5279/health"),
            new("LetTransactionService", "http://localhost:5320/health")
        };

        var data = new Dictionary<string, object>();
        var degraded = false;
        var unhealthy = false;
        var client = httpClientFactory.CreateClient("HealthChecks");

        var tasks = services.Select(async svc =>
        {
            try
            {
                using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
                cts.CancelAfter(TimeSpan.FromSeconds(5));
                var response = await client.GetAsync(svc.HealthUrl, cts.Token);
                return (svc.Name, IsHealthy: response.IsSuccessStatusCode, Error: (string?)null);
            }
            catch (Exception ex)
            {
                return (svc.Name, IsHealthy: false, Error: ex.Message);
            }
        });

        var results = await Task.WhenAll(tasks);

        foreach (var result in results)
        {
            if (result.IsHealthy)
            {
                data[result.Name] = "Healthy";
            }
            else
            {
                data[result.Name] = $"Unhealthy: {result.Error ?? "Non-success status"}";
                degraded = true;
                logger.LogWarning("Downstream {Service} is unhealthy: {Error}", result.Name, result.Error);
            }
        }

        if (unhealthy)
            return HealthCheckResult.Unhealthy("One or more downstream services are unavailable.", data: data);

        if (degraded)
            return HealthCheckResult.Degraded("One or more downstream services are degraded.", data: data);

        return HealthCheckResult.Healthy("All downstream services are healthy.", data);
    }
}
