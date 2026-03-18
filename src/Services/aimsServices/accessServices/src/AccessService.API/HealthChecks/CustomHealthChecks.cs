namespace AccessService.API.HealthChecks;

using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using AccessService.Infrastructure.Persistence;

/// <summary>
/// Custom health checks for the Access Service API
/// </summary>

public class DatabaseHealthCheck : IHealthCheck
{
    private readonly AccessServiceDbContext _dbContext;
    private readonly ILogger<DatabaseHealthCheck> _logger;

    public DatabaseHealthCheck(AccessServiceDbContext dbContext, ILogger<DatabaseHealthCheck> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var canConnect = await _dbContext.Database.CanConnectAsync(cancellationToken);
            if (!canConnect)
            {
                _logger.LogWarning("Database connection failed");
                return HealthCheckResult.Unhealthy("Could not connect to database");
            }

            // Check if tables exist
            var userMapCount = await _dbContext.UserMaps.CountAsync(cancellationToken);
            var userRoleCount = await _dbContext.UserRoles.CountAsync(cancellationToken);

            return HealthCheckResult.Healthy(
                $"Database is healthy. UserMaps: {userMapCount}, UserRoles: {userRoleCount}"
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database health check failed");
            return HealthCheckResult.Unhealthy($"Database health check failed: {ex.Message}");
        }
    }
}

public class ApiHealthCheck : IHealthCheck
{
    private readonly ILogger<ApiHealthCheck> _logger;

    public ApiHealthCheck(ILogger<ApiHealthCheck> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            // Check if API is running and can process requests
            var currentTime = DateTime.UtcNow;
            
            return Task.FromResult(HealthCheckResult.Healthy(
                $"API is healthy. Current time: {currentTime:O}"
            ));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "API health check failed");
            return Task.FromResult(HealthCheckResult.Unhealthy($"API health check failed: {ex.Message}"));
        }
    }
}
