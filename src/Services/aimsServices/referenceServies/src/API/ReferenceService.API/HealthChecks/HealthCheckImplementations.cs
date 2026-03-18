using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ReferenceService.API.HealthChecks;

/// <summary>
/// Custom health check for database connectivity.
/// </summary>
public class DatabaseHealthCheck : IHealthCheck
{
    private readonly IServiceProvider _serviceProvider;
    
    public DatabaseHealthCheck(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ReferenceService.Infrastructure.Persistence.ReferenceDbContext>();
            
            var canConnect = await dbContext.Database.CanConnectAsync(cancellationToken);
            
            return canConnect
                ? HealthCheckResult.Healthy("Database connectivity is healthy")
                : HealthCheckResult.Unhealthy("Database connectivity failed");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy($"Database health check failed: {ex.Message}");
        }
    }
}

/// <summary>
/// Custom health check for API readiness.
/// </summary>
public class ApiReadinessHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        // Custom logic to determine if API is ready to accept requests
        return Task.FromResult(HealthCheckResult.Healthy("API is ready"));
    }
}
