using Microsoft.Extensions.Diagnostics.HealthChecks;
using TourPlanService.Infrastructure.Data;

namespace TourPlanService.Infrastructure.HealthChecks;

public sealed class DatabaseHealthCheck(TourPlanDbContext dbContext) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await dbContext.Database.CanConnectAsync(cancellationToken);
            return HealthCheckResult.Healthy("Database connection is healthy.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Database connection failed.", ex);
        }
    }
}
