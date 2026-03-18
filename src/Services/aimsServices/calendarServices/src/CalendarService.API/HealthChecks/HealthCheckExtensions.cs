using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;
using System.Text.Json;

namespace CalendarService.API.HealthChecks;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddCalendarHealthChecks(this IServiceCollection services, IConfiguration config)
    {
        var hcBuilder = services.AddHealthChecks()
            .AddSqlServer(
                config.GetConnectionString("CalendarDb")!,
                name: "sqlserver",
                failureStatus: HealthStatus.Degraded,
                tags: ["db", "sql"]);

        // Register RabbitMQ connection factory for health check
        services.AddSingleton<IConnectionFactory>(_ =>
            new ConnectionFactory
            {
                HostName = config["RabbitMQ:Host"] ?? "localhost",
                UserName = config["RabbitMQ:Username"] ?? "guest",
                Password = config["RabbitMQ:Password"] ?? "guest",
                VirtualHost = config["RabbitMQ:VirtualHost"] ?? "/"
            });

        hcBuilder.AddRabbitMQ(
            name: "rabbitmq",
            failureStatus: HealthStatus.Degraded,
            tags: ["messaging"]);

        return services;
    }

    public static IEndpointRouteBuilder MapCalendarHealthChecks(this IEndpointRouteBuilder app)
    {
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = async (ctx, report) =>
            {
                ctx.Response.ContentType = "application/json";
                var result = JsonSerializer.Serialize(new
                {
                    status = report.Status.ToString(),
                    checks = report.Entries.Select(e => new
                    {
                        name = e.Key,
                        status = e.Value.Status.ToString(),
                        description = e.Value.Description
                    })
                });
                await ctx.Response.WriteAsync(result);
            }
        });

        app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false });
        app.MapHealthChecks("/health/ready", new HealthCheckOptions { Predicate = c => c.Tags.Contains("db") });

        return app;
    }
}
