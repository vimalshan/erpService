using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using RecruitmentService.Infrastructure.Persistence;

namespace RecruitmentService.API.HealthChecks;

public class DatabaseHealthCheck : IHealthCheck
{
    private readonly RecruitmentDbContext _dbContext;

    public DatabaseHealthCheck(RecruitmentDbContext dbContext) => _dbContext = dbContext;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken ct = default)
    {
        try
        {
            await _dbContext.Database.ExecuteSqlRawAsync("SELECT 1", ct);
            return HealthCheckResult.Healthy("Database is reachable.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Database is unreachable.", ex);
        }
    }
}
