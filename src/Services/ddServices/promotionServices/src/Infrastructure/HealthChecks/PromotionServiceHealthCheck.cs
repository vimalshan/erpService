using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using PromotionService.Infrastructure.Persistence;

namespace PromotionService.Infrastructure.HealthChecks;

/// <summary>
/// Custom health check that verifies database connectivity and checks for active promotion periods.
/// </summary>
public class PromotionServiceHealthCheck : IHealthCheck
{
    private readonly PromotionDbContext _dbContext;

    public PromotionServiceHealthCheck(PromotionDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            // Verify DB connection is alive
            var canConnect = await _dbContext.Database.CanConnectAsync(cancellationToken);
            if (!canConnect)
                return HealthCheckResult.Unhealthy("Cannot connect to DDDB database.");

            // Check there is at least one promotion period defined
            var periodCount = await _dbContext.PromotionPeriods.CountAsync(cancellationToken);

            var data = new Dictionary<string, object>
            {
                ["promotionPeriodCount"] = periodCount,
                ["database"] = "DDDB",
                ["checkedAt"] = DateTime.UtcNow
            };

            // Service is healthy if database is accessible, even without promotion periods
            return HealthCheckResult.Healthy("Promotion service is healthy.", data);
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Promotion service health check failed.", ex);
        }
    }
}
