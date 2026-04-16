using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ApiGateway.Extensions;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddGatewayHealthChecks(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var builder = services.AddHealthChecks()
            .AddCheck("gateway-self", () => HealthCheckResult.Healthy("Gateway is running"));

        var serviceEntries = configuration
            .GetSection("HealthChecks:Services")
            .Get<ServiceHealthEntry[]>() ?? [];

        foreach (var entry in serviceEntries)
        {
            builder.AddUrlGroup(
                new Uri(entry.Uri),
                name: entry.Name,
                failureStatus: HealthStatus.Degraded,
                timeout: TimeSpan.FromSeconds(5));
        }

        services.AddHealthChecksUI(setup =>
        {
            setup.SetEvaluationTimeInSeconds(15);
            setup.MaximumHistoryEntriesPerEndpoint(50);
            setup.AddHealthCheckEndpoint("ERP Gateway", "/health");
        }).AddInMemoryStorage();

        return services;
    }

    public static IEndpointRouteBuilder MapGatewayHealthChecks(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            ResultStatusCodes =
            {
                [HealthStatus.Healthy]   = StatusCodes.Status200OK,
                [HealthStatus.Degraded]  = StatusCodes.Status200OK,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            }
        });

        endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = _ => false,   // liveness: only gateway itself
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("ready"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        endpoints.MapHealthChecksUI(options =>
        {
            options.UIPath        = "/health-ui";
            options.ApiPath       = "/health-ui-api";
            options.PageTitle     = "ERP Microservices Health";
        });

        return endpoints;
    }

    private sealed record ServiceHealthEntry(string Name, string Uri);
}
