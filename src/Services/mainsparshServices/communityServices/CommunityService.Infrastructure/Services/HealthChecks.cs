namespace CommunityService.Infrastructure.Services;

using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Persistence;

public class DatabaseHealthCheck : IHealthCheck
{
    private readonly CommunityDbContext _dbContext;

    public DatabaseHealthCheck(CommunityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var canConnect = await _dbContext.Database.CanConnectAsync(cancellationToken);
            
            if (canConnect)
            {
                return HealthCheckResult.Healthy("Database connection is working");
            }
            else
            {
                return HealthCheckResult.Unhealthy("Database connection failed");
            }
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy($"Database health check failed: {ex.Message}");
        }
    }
}

public class RabbitMqHealthCheck : IHealthCheck
{
    private readonly string _hostName;
    private readonly ILogger<RabbitMqHealthCheck> _logger;

    public RabbitMqHealthCheck(string hostName, ILogger<RabbitMqHealthCheck> logger)
    {
        _hostName = hostName;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var factory = new RabbitMQ.Client.ConnectionFactory { HostName = _hostName };
            using var connection = factory.CreateConnection();
            
            if (connection.IsOpen)
            {
                return HealthCheckResult.Healthy("RabbitMQ connection is working");
            }
            else
            {
                return HealthCheckResult.Unhealthy("RabbitMQ connection failed");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RabbitMQ health check failed");
            return HealthCheckResult.Unhealthy($"RabbitMQ health check failed: {ex.Message}");
        }
    }
}
