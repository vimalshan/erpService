namespace FeedbackService.API.Configuration;

using Microsoft.Extensions.Diagnostics.HealthChecks;

/// <summary>
/// Health checks configuration
/// </summary>
public static class HealthChecksConfiguration
{
    /// <summary>
    /// Adds health checks to the service collection
    /// </summary>
    public static IServiceCollection AddHealthChecks(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var healthChecks = services
            .AddHealthChecks();
        
        // Add SQL Server health check
        if (!string.IsNullOrEmpty(connectionString))
        {
            healthChecks.AddSqlServer(
                connectionString,
                name: "SQL Server",
                failureStatus: HealthStatus.Degraded);
        }

        // Add RabbitMQ health check - non-critical
        try
        {
            healthChecks.AddRabbitMQ(
                new Uri(GetRabbitMQUri(configuration)),
                name: "RabbitMQ",
                failureStatus: HealthStatus.Degraded,
                timeout: TimeSpan.FromSeconds(5));
        }
        catch
        {
            // RabbitMQ connection might not be available, which is okay
        }

        // API health check
        healthChecks.AddCheck("API", () => HealthCheckResult.Healthy("API Health Check"), tags: new[] { "ready" });

        return services;
    }

    /// <summary>
    /// Gets the RabbitMQ connection URI
    /// </summary>
    private static string GetRabbitMQUri(IConfiguration configuration)
    {
        var hostname = configuration["RabbitMQ:Hostname"] ?? "localhost";
        var username = configuration["RabbitMQ:Username"] ?? "guest";
        var password = configuration["RabbitMQ:Password"] ?? "guest";
        var port = configuration["RabbitMQ:Port"] ?? "5672";

        return $"amqp://{username}:{password}@{hostname}:{port}/";
    }
}
