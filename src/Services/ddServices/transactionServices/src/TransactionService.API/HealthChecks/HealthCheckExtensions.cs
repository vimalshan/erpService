using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace TransactionService.API.HealthChecks;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddApplicationHealthChecks(
        this IServiceCollection services,
        string connectionString,
        string? rabbitmqHostname = null)
    {
        var healthChecksBuilder = services.AddHealthChecks();

        // Add database health check
        healthChecksBuilder.Add(new HealthCheckRegistration(
            name: "Database Check",
            instance: new SimpleHealthCheck("Database"),
            failureStatus: HealthStatus.Unhealthy,
            tags: new[] { "database" }));

        // Add RabbitMQ health check if hostname is provided
        if (!string.IsNullOrEmpty(rabbitmqHostname))
        {
            healthChecksBuilder.Add(new HealthCheckRegistration(
                name: "RabbitMQ Check",
                instance: new SimpleHealthCheck("RabbitMQ"),
                failureStatus: HealthStatus.Degraded,
                tags: new[] { "messaging" }));
        }

        return services;
    }

    public static IApplicationBuilder UseApplicationHealthChecks(this IApplicationBuilder app)
    {
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = WriteJsonResponse
        });

        app.UseHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("ready"),
            ResponseWriter = WriteJsonResponse
        });

        return app;
    }

    private static async Task WriteJsonResponse(HttpContext context, HealthReport report)
    {
        context.Response.ContentType = "application/json";
        var response = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(x => new
            {
                name = x.Key,
                status = x.Value.Status.ToString(),
                exception = x.Value.Exception?.Message,
                description = x.Value.Description
            })
        };
        await context.Response.WriteAsJsonAsync(response);
    }
}

public class SimpleHealthCheck : IHealthCheck
{
    private readonly string _name;

    public SimpleHealthCheck(string name)
    {
        _name = name;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(HealthCheckResult.Healthy($"{_name} is healthy"));
    }
}
