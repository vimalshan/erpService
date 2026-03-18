using Microsoft.Extensions.Diagnostics.HealthChecks;
using DevelopmentService.Infrastructure.Data;

namespace DevelopmentService.API.HealthChecks;

public class DevelopmentDbHealthCheck : IHealthCheck
{
    private readonly DevelopmentDbContext _dbContext;

    public DevelopmentDbHealthCheck(DevelopmentDbContext dbContext)
        => _dbContext = dbContext;

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken ct = default)
    {
        try
        {
            await _dbContext.Database.CanConnectAsync(ct);
            return HealthCheckResult.Healthy("Database connection is healthy.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Database connection failed.", ex);
        }
    }
}
