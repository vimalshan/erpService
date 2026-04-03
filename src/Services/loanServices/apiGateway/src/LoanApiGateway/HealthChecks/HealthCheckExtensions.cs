using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LoanApiGateway.HealthChecks;

/// <summary>
/// Registers health checks for all downstream loan microservices,
/// RabbitMQ message broker, and the gateway itself.
/// Exposes /health/live, /health/ready, and /health/ui endpoints.
/// </summary>
public static class HealthCheckExtensions
{
    public static IServiceCollection AddGatewayHealthChecks(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var hc = services
            .AddHealthChecks()
            // Gateway self-check
            .AddCheck("gateway-self", () => HealthCheckResult.Healthy("Gateway is running"),
                tags: ["live", "gateway"])

            // ── Loan Microservices ────────────────────────────────────────────
            .AddUrlGroup(
                new Uri(configuration["Services:LoanTransaction:HealthUrl"]
                    ?? "http://localhost:5292/health"),
                name: "loan-transaction",
                failureStatus: HealthStatus.Degraded,
                tags: ["ready", "loan-services"])

            .AddUrlGroup(
                new Uri(configuration["Services:LoanApplication:HealthUrl"]
                    ?? "http://localhost:5282/health"),
                name: "loan-application",
                failureStatus: HealthStatus.Degraded,
                tags: ["ready", "loan-services"])

            .AddUrlGroup(
                new Uri(configuration["Services:LoanAccount:HealthUrl"]
                    ?? "http://localhost:5150/health"),
                name: "loan-account",
                failureStatus: HealthStatus.Degraded,
                tags: ["ready", "loan-services"])

            .AddUrlGroup(
                new Uri(configuration["Services:LoanDefinition:HealthUrl"]
                    ?? "http://localhost:5077/health"),
                name: "loan-definition",
                failureStatus: HealthStatus.Degraded,
                tags: ["ready", "loan-services"])

            .AddUrlGroup(
                new Uri(configuration["Services:Document:HealthUrl"]
                    ?? "http://localhost:5280/health"),
                name: "document-service",
                failureStatus: HealthStatus.Degraded,
                tags: ["ready", "loan-services"])

            .AddUrlGroup(
                new Uri(configuration["Services:Lov:HealthUrl"]
                    ?? "http://localhost:5008/health"),
                name: "lov-service",
                failureStatus: HealthStatus.Degraded,
                tags: ["ready", "loan-services"])

            .AddUrlGroup(
                new Uri(configuration["Services:Utility:HealthUrl"]
                    ?? "http://localhost:5143/health"),
                name: "utility-service",
                failureStatus: HealthStatus.Degraded,
                tags: ["ready", "loan-services"])

            // ── RabbitMQ (via Management API) ─────────────────────────────────────
            .AddUrlGroup(
                new Uri(configuration["RabbitMQ:ManagementUrl"]
                    ?? "http://localhost:15672/api/healthchecks/node"),
                name: "rabbitmq",
                failureStatus: HealthStatus.Degraded,
                tags: ["ready", "infrastructure"]);

        // Health Checks UI
        services.AddHealthChecksUI(opt =>
        {
            opt.SetEvaluationTimeInSeconds(30);
            opt.MaximumHistoryEntriesPerEndpoint(60);
            opt.SetApiMaxActiveRequests(5);
            opt.AddHealthCheckEndpoint("Loan Microservices", "/health/ready");
            opt.AddHealthCheckEndpoint("Infrastructure", "/health/live");
        }).AddInMemoryStorage();

        return services;
    }

    public static IEndpointRouteBuilder MapGatewayHealthChecks(this IEndpointRouteBuilder endpoints)
    {
        // Liveness — just gateway self
        endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = hc => hc.Tags.Contains("live"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        // Readiness — all downstream services
        endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = hc => hc.Tags.Contains("ready"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        // Full — every registered check
        endpoints.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        // Health Checks UI dashboard
        endpoints.MapHealthChecksUI(config =>
        {
            config.UIPath = "/health/ui";
            config.ApiPath = "/health/ui-api";
        });

        return endpoints;
    }
}
