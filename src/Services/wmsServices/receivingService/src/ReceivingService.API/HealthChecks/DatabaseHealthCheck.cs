using Microsoft.EntityFrameworkCore;
using ReceivingService.Infrastructure.Data;

namespace ReceivingService.API.HealthChecks;

/// <summary>Custom EF Core database health check for /health endpoints.</summary>
public sealed class DatabaseHealthCheck : Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheck
{
    private readonly ReceivingDbContext _dbContext;

    public DatabaseHealthCheck(ReceivingDbContext dbContext)
        => _dbContext = dbContext;

    public async Task<Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult> CheckHealthAsync(
        Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.Database.ExecuteSqlRawAsync("SELECT 1", cancellationToken);
            return Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy("Database is reachable.");
        }
        catch (Exception ex)
        {
            return Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Unhealthy(
                "Database is unreachable.", ex);
        }
    }
}
