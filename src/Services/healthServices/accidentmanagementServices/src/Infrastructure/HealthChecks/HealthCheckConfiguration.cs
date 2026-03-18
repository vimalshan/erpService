using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AccidentManagementService.Infrastructure.HealthChecks;

/// <summary>
/// Custom health check for database connectivity and readiness
/// </summary>
public class DatabaseHealthCheck : IHealthCheck
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DatabaseHealthCheck> _logger;

    public DatabaseHealthCheck(IServiceProvider serviceProvider, ILogger<DatabaseHealthCheck> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AccidentManagementDbContext>();
                
                // Test database connectivity
                var canConnect = await dbContext.Database.CanConnectAsync(cancellationToken);
                
                if (!canConnect)
                {
                    _logger.LogWarning("Database connectivity check failed");
                    return HealthCheckResult.Unhealthy("Unable to connect to database");
                }

                // Optional: Execute a test query
                var tableCount = await dbContext.AccidentReports.CountAsync(cancellationToken);
                
                _logger.LogInformation("Database health check passed. Tables: {TableCount}", tableCount);
                return HealthCheckResult.Healthy("Database is available and functioning");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database health check failed with exception");
            return HealthCheckResult.Unhealthy($"Database check failed: {ex.Message}");
        }
    }
}

/// <summary>
/// Custom health check for RabbitMQ connectivity
/// </summary>
public class RabbitMQHealthCheck : IHealthCheck
{
    private readonly RabbitMQSettings _settings;
    private readonly ILogger<RabbitMQHealthCheck> _logger;

    public RabbitMQHealthCheck(IOptions<RabbitMQSettings> options, ILogger<RabbitMQHealthCheck> logger)
    {
        _settings = options.Value;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.Host,
                Port = _settings.Port,
                UserName = _settings.Username,
                Password = _settings.Password,
                VirtualHost = _settings.VirtualHost,
                Ssl = new SslOption { Enabled = _settings.UseSsl }
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                // Test connectivity
                var queueName = $"{_settings.QueuePrefix}-health-check";
                channel.QueueDeclarePassive(queueName);
                
                _logger.LogInformation("RabbitMQ health check passed");
                return HealthCheckResult.Healthy("RabbitMQ is available");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RabbitMQ health check failed");
            return HealthCheckResult.Unhealthy($"RabbitMQ check failed: {ex.Message}");
        }
    }
}

/// <summary>
/// Custom health check for application memory usage
/// </summary>
public class MemoryHealthCheck : IHealthCheck
{
    private readonly long _maxMemoryBytes;

    public MemoryHealthCheck(long maxMemoryMBytes = 300)
    {
        // Convert MBytes to bytes
        _maxMemoryBytes = maxMemoryMBytes * 1024 * 1024;
    }

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var totalMemory = GC.GetTotalMemory(false);

        if (totalMemory > _maxMemoryBytes)
        {
            var message = $"Application memory usage ({totalMemory / 1024 / 1024}M) exceeds threshold ({_maxMemoryBytes / 1024 / 1024}M)";
            return Task.FromResult(HealthCheckResult.Degraded(message));
        }

        return Task.FromResult(HealthCheckResult.Healthy(
            $"Application memory usage: {totalMemory / 1024 / 1024}M"));
    }
}

/// <summary>
/// Health check response writer for console/JSON output
/// </summary>
public static class HealthCheckResponseWriter
{
    /// <summary>
    /// Writes health check results as JSON with detailed information
    /// </summary>
    public static async Task WriteHealthCheckResponse(HttpContext context, HealthReport report)
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            status = report.Status.ToString(),
            timestamp = DateTime.UtcNow,
            duration = report.Entries.Values.Sum(x => x.Duration.TotalMilliseconds),
            checks = report.Entries.Select(x => new
            {
                name = x.Key,
                status = x.Value.Status.ToString(),
                duration = x.Value.Duration.TotalMilliseconds,
                description = x.Value.Description,
                data = x.Value.Data?.Count > 0 ? x.Value.Data : null,
                exception = x.Value.Exception?.Message
            }).ToList()
        };

        await context.Response.WriteAsJsonAsync(response);
    }
}

/// <summary>
/// Configuration extension for health checks
/// Usage: services.AddApplicationHealthChecks(configuration)
/// </summary>
public static class HealthCheckExtensions
{
    /// <summary>
    /// Registers all application health checks
    /// </summary>
    public static IHealthChecksBuilder AddApplicationHealthChecks(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var builder = services.AddHealthChecks();

        // Database health check
        builder.AddCheck<DatabaseHealthCheck>(
            name: "Database",
            failureStatus: HealthStatus.Unhealthy,
            tags: new[] { "db", "ready" });

        // RabbitMQ health check
        if (configuration.GetSection("RabbitMQ").Exists())
        {
            builder.AddCheck<RabbitMQHealthCheck>(
                name: "RabbitMQ",
                failureStatus: HealthStatus.Degraded,
                tags: new[] { "messaging", "ready" });
        }

        // Memory health check
        builder.AddCheck<MemoryHealthCheck>(
            name: "Memory",
            failureStatus: HealthStatus.Degraded,
            tags: new[] { "memory", "ready" });

        // Startup check (always healthy initially)
        builder.AddCheck("Startup", () =>
            HealthCheckResult.Healthy("Application started successfully"),
            tags: new[] { "ready" });

        return builder;
    }

    /// <summary>
    /// Maps health check endpoints to HTTP routes
    /// Usage: app.MapApplicationHealthChecks()
    /// </summary>
    public static WebApplication MapApplicationHealthChecks(this WebApplication app)
    {
        // Liveness probe - is the app running?
        app.MapHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = _ => false, // No checks, always healthy if app is running
            ResponseWriter = HealthCheckResponseWriter.WriteHealthCheckResponse,
            ResultStatusCodes =
            {
                HealthStatus.Healthy = StatusCodes.Status200OK,
                HealthStatus.Degraded = StatusCodes.Status503ServiceUnavailable,
                HealthStatus.Unhealthy = StatusCodes.Status503ServiceUnavailable
            }
        });

        // Readiness probe - is the app ready to receive traffic?
        app.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = hc => hc.Tags.Contains("ready"),
            ResponseWriter = HealthCheckResponseWriter.WriteHealthCheckResponse,
            ResultStatusCodes =
            {
                HealthStatus.Healthy = StatusCodes.Status200OK,
                HealthStatus.Degraded = StatusCodes.Status200OK,
                HealthStatus.Unhealthy = StatusCodes.Status503ServiceUnavailable
            }
        });

        // Detailed health check endpoint
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = HealthCheckResponseWriter.WriteHealthCheckResponse,
            AllowCachingResponses = false,
            ResultStatusCodes =
            {
                HealthStatus.Healthy = StatusCodes.Status200OK,
                HealthStatus.Degraded = StatusCodes.Status200OK,
                HealthStatus.Unhealthy = StatusCodes.Status503ServiceUnavailable
            }
        });

        return app;
    }
}
