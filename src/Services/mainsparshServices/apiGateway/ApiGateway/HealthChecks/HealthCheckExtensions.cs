using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ApiGateway.HealthChecks;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddGatewayHealthChecks(this IServiceCollection services, IConfiguration config)
    {
        var hcBuilder = services.AddHealthChecks()
            .AddCheck("gateway-self", () => HealthCheckResult.Healthy("API Gateway is running."), tags: ["gateway"]);

        var serviceDiscovery = config.GetSection("ServiceDiscovery");
        foreach (var child in serviceDiscovery.GetChildren())
        {
            var serviceName = child.Key;
            var serviceUrl = child.Value;
            if (!string.IsNullOrWhiteSpace(serviceUrl))
            {
                hcBuilder.AddUrlGroup(
                    new Uri($"{serviceUrl}/health"),
                    name: $"{serviceName}-health",
                    failureStatus: HealthStatus.Degraded,
                    tags: ["services", serviceName.ToLowerInvariant()],
                    timeout: TimeSpan.FromSeconds(5));
            }
        }

        return services;
    }

    public static WebApplication MapGatewayHealthChecks(this WebApplication app)
    {
        // Overall gateway health
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            Predicate = _ => true
        });

        // Gateway-only (no downstream service checks)
        app.MapHealthChecks("/health/gateway", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            Predicate = check => check.Tags.Contains("gateway")
        });

        // Downstream services only
        app.MapHealthChecks("/health/services", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            Predicate = check => check.Tags.Contains("services")
        });

        return app;
    }
}
