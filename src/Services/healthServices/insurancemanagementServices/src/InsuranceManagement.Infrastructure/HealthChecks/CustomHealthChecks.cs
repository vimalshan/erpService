using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using InsuranceManagement.Infrastructure.Data;

namespace InsuranceManagement.Infrastructure.HealthChecks;

/// <summary>
/// Health check for database connectivity and basic operations
/// </summary>
public class DatabaseHealthCheck : IHealthCheck
{
    private readonly InsuranceManagementDbContext _dbContext;
    private readonly ILogger<DatabaseHealthCheck> _logger;

    public DatabaseHealthCheck(InsuranceManagementDbContext dbContext, ILogger<DatabaseHealthCheck> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            // Test database connectivity
            var canConnect = await _dbContext.Database.CanConnectAsync(cancellationToken);
            if (!canConnect)
            {
                _logger.LogWarning("Database health check failed: Cannot connect to database");
                return HealthCheckResult.Unhealthy("Cannot connect to database");
            }

            // Test basic query
            var planCount = _dbContext.InsurancePlans.Count();

            _logger.LogDebug($"Database health check passed. Insurance plans count: {planCount}");
            return HealthCheckResult.Healthy($"Database is healthy. Found {planCount} insurance plans.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Database health check failed: {ex.Message}");
            return HealthCheckResult.Unhealthy($"Database health check failed: {ex.Message}");
        }
    }
}

/// <summary>
/// Health check for RabbitMQ connectivity
/// </summary>
public class RabbitMqHealthCheck : IHealthCheck
{
    private readonly ILogger<RabbitMqHealthCheck> _logger;
    private readonly string _hostName;
    private readonly int _port;

    public RabbitMqHealthCheck(ILogger<RabbitMqHealthCheck> logger, IConfiguration configuration)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _hostName = configuration["RabbitMQ:HostName"] ?? "localhost";
        _port = int.TryParse(configuration["RabbitMQ:Port"], out var port) ? port : 5672;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            using (var client = new System.Net.Sockets.TcpClient())
            {
                var connectTask = client.ConnectAsync(_hostName, _port, cancellationToken).AsTask();
                var completedTask = await Task.WhenAny(connectTask, Task.Delay(5000, cancellationToken));

                if (completedTask != connectTask)
                {
                    _logger.LogWarning($"RabbitMQ health check timeout: {_hostName}:{_port}");
                    return HealthCheckResult.Unhealthy($"RabbitMQ connection timeout at {_hostName}:{_port}");
                }

                _logger.LogDebug($"RabbitMQ health check passed: {_hostName}:{_port}");
                return HealthCheckResult.Healthy($"RabbitMQ is healthy at {_hostName}:{_port}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"RabbitMQ health check failed: {ex.Message}");
            return HealthCheckResult.Unhealthy($"RabbitMQ is unhealthy: {ex.Message}");
        }
    }
}

/// <summary>
/// Health check for API dependencies
/// </summary>
public class ApiDependenciesHealthCheck : IHealthCheck
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ApiDependenciesHealthCheck> _logger;

    public ApiDependenciesHealthCheck(IServiceProvider serviceProvider, ILogger<ApiDependenciesHealthCheck> logger)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var healthChecks = new List<string>();

            // Check MediatR availability
            try
            {
                var mediatorType = Type.GetType("MediatR.IMediator, MediatR");
                if (mediatorType != null)
                {
                    var mediator = _serviceProvider.GetService(mediatorType);
                    if (mediator != null)
                        healthChecks.Add("MediatR: OK");
                    else
                        healthChecks.Add("MediatR: Not registered");
                }
                else
                {
                    healthChecks.Add("MediatR: Not installed");
                }
            }
            catch
            {
                healthChecks.Add("MediatR: Error");
            }

            // Check AutoMapper availability
            try
            {
                var mapperType = Type.GetType("AutoMapper.IMapper, AutoMapper");
                if (mapperType != null)
                {
                    var mapper = _serviceProvider.GetService(mapperType);
                    if (mapper != null)
                        healthChecks.Add("AutoMapper: OK");
                    else
                        healthChecks.Add("AutoMapper: Not registered");
                }
                else
                {
                    healthChecks.Add("AutoMapper: Not installed");
                }
            }
            catch
            {
                healthChecks.Add("AutoMapper: Error");
            }

            var statusMessage = string.Join(", ", healthChecks);
            _logger.LogDebug($"API dependencies health check: {statusMessage}");

            return healthChecks.All(h => h.Contains("OK")) 
                ? HealthCheckResult.Healthy($"Dependencies: {statusMessage}")
                : HealthCheckResult.Degraded($"Dependencies: {statusMessage}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"API dependencies health check failed: {ex.Message}");
            return HealthCheckResult.Unhealthy($"Dependencies health check failed: {ex.Message}");
        }
    }
}

/// <summary>
/// Extension methods for health check registration
/// </summary>
public static class HealthCheckExtensions
{
    /// <summary>
    /// Add custom health checks
    /// </summary>
    public static IHealthChecksBuilder AddCustomHealthChecks(
        this IHealthChecksBuilder builder,
        IConfiguration configuration)
    {
        builder.AddCheck<DatabaseHealthCheck>("database", tags: new[] { "db" })
               .AddCheck<ApiDependenciesHealthCheck>("dependencies", tags: new[] { "api" });

        // Add RabbitMQ health check only if enabled
        var enableRabbitMq = bool.TryParse(configuration["RabbitMQ:Enabled"], out var result) && result;
        if (enableRabbitMq)
        {
            builder.AddCheck<RabbitMqHealthCheck>("rabbitmq", tags: new[] { "message-queue" });
        }

        return builder;
    }
}
