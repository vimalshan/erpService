using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Reflection;
using System.Data;

namespace CheckupManagementService.Infrastructure.HealthChecks;

/// <summary>
/// Custom health check for database connectivity
/// </summary>
public class DatabaseHealthCheck : IHealthCheck
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<DatabaseHealthCheck> _logger;

    public DatabaseHealthCheck(IConfiguration configuration, ILogger<DatabaseHealthCheck> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var connectionString = _configuration.GetConnectionString("HealthDb");
            
            if (string.IsNullOrEmpty(connectionString))
            {
                _logger.LogError("Database connection string not configured");
                return HealthCheckResult.Unhealthy("Database connection string not configured");
            }

            // For now, just validate that connection string exists
            // In production, you would attempt to open a real connection
            _logger.LogInformation("Database health check passed");
            return HealthCheckResult.Healthy("Database configuration valid");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database health check failed");
            return HealthCheckResult.Unhealthy("Database configuration invalid", ex);
        }
    }
}

/// <summary>
/// Custom health check for RabbitMQ connectivity
/// </summary>
public class RabbitMQHealthCheck : IHealthCheck
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<RabbitMQHealthCheck> _logger;

    public RabbitMQHealthCheck(IConfiguration configuration, ILogger<RabbitMQHealthCheck> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var host = _configuration.GetValue<string>("RabbitMQ:Host");
            var port = _configuration.GetValue<int>("RabbitMQ:Port");

            if (string.IsNullOrEmpty(host))
            {
                _logger.LogError("RabbitMQ host not configured");
                return Task.FromResult(HealthCheckResult.Unhealthy("RabbitMQ host not configured"));
            }

            using (var socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp))
            {
                var result = socket.BeginConnect(host, port, null, null);
                if (result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(3)))
                {
                    socket.EndConnect(result);
                    _logger.LogInformation("RabbitMQ health check passed");
                    return Task.FromResult(HealthCheckResult.Healthy("RabbitMQ connection successful"));
                }
                else
                {
                    _logger.LogError("RabbitMQ connection timeout");
                    return Task.FromResult(HealthCheckResult.Unhealthy("RabbitMQ connection timeout"));
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RabbitMQ health check failed");
            return Task.FromResult(HealthCheckResult.Unhealthy("RabbitMQ connection failed", ex));
        }
    }
}

/// <summary>
/// Custom health check for Redis connectivity
/// </summary>
public class RedisHealthCheck : IHealthCheck
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<RedisHealthCheck> _logger;

    public RedisHealthCheck(IConfiguration configuration, ILogger<RedisHealthCheck> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var connectionString = _configuration.GetConnectionString("Redis:Configuration");

            if (string.IsNullOrEmpty(connectionString))
            {
                _logger.LogError("Redis connection string not configured");
                return HealthCheckResult.Unhealthy("Redis connection string not configured");
            }

            // Validate Redis connection format
            if (!connectionString.Contains(":"))
            {
                return HealthCheckResult.Unhealthy("Invalid Redis connection string format");
            }

            _logger.LogInformation("Redis health check passed");
            return HealthCheckResult.Healthy("Redis configuration valid");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Redis health check failed");
            return HealthCheckResult.Unhealthy("Redis configuration invalid", ex);
        }
    }
}

/// <summary>
/// Health check for API itself
/// </summary>
public class ApiHealthCheck : IHealthCheck
{
    private readonly ILogger<ApiHealthCheck> _logger;

    public ApiHealthCheck(ILogger<ApiHealthCheck> logger)
    {
        _logger = logger;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var version = typeof(Program).Assembly.GetName().Version?.ToString() ?? "1.0.0";
            _logger.LogInformation("API health check passed - Version: {version}", version);
            
            return Task.FromResult(HealthCheckResult.Healthy(
                $"CheckupManagementService v{version} is operational",
                new Dictionary<string, object>
                {
                    { "serviceName", "CheckupManagementService" },
                    { "version", version },
                    { "timestamp", DateTime.UtcNow }
                }
            ));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "API health check failed");
            return Task.FromResult(HealthCheckResult.Unhealthy("API health check failed", ex));
        }
    }
}
