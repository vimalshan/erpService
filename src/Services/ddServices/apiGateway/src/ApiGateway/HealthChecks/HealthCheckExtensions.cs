using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ApiGateway.HealthChecks;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddGatewayHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var healthBuilder = services.AddHealthChecks()
            .AddCheck("Gateway-Self", () => HealthCheckResult.Healthy("API Gateway is running"), tags: new[] { "self", "ready" });

        // Add health checks for each downstream service
        var endpoints = configuration.GetSection("ServiceEndpoints");
        foreach (var svc in endpoints.GetChildren())
        {
            var serviceName = svc.Key;
            var baseUrl = svc.Value;
            if (!string.IsNullOrWhiteSpace(baseUrl))
            {
                healthBuilder.AddUrlGroup(
                    new Uri($"{baseUrl}/health"),
                    name: $"{serviceName}-Health",
                    failureStatus: HealthStatus.Degraded,
                    tags: new[] { "downstream", "ready" },
                    timeout: TimeSpan.FromSeconds(5));
            }
        }

        return services;
    }

    public static WebApplication UseGatewayHealthChecks(this WebApplication app)
    {
        // Liveness: just gateway itself
        app.UseHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("self"),
            ResponseWriter = WriteHealthResponse
        });

        // Readiness: gateway + all downstream services
        app.UseHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("ready"),
            ResponseWriter = WriteHealthResponse
        });

        // Downstream only
        app.UseHealthChecks("/health/downstream", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("downstream"),
            ResponseWriter = WriteHealthResponse
        });

        return app;
    }

    private static async Task WriteHealthResponse(HttpContext context, HealthReport report)
    {
        context.Response.ContentType = "application/json";
        var response = new
        {
            status = report.Status.ToString(),
            totalDuration = report.TotalDuration.TotalMilliseconds + "ms",
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                duration = e.Value.Duration.TotalMilliseconds + "ms",
                description = e.Value.Description,
                exception = e.Value.Exception?.Message
            })
        };
        await context.Response.WriteAsJsonAsync(response);
    }
}
