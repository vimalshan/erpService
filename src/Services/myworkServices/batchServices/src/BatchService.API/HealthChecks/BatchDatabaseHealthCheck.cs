using Microsoft.Extensions.Diagnostics.HealthChecks;
using BatchService.Infrastructure.Persistence;

namespace BatchService.API.HealthChecks;

public sealed class BatchDatabaseHealthCheck : IHealthCheck
{
    private readonly BatchDbContext _context;

    public BatchDatabaseHealthCheck(BatchDbContext context) => _context = context;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext ctx, CancellationToken ct = default)
    {
        try
        {
            await _context.Database.CanConnectAsync(ct);
            var count = _context.BatchMasters.Count();
            return HealthCheckResult.Healthy($"Database reachable. BatchMaster rows: {count}");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Database unreachable.", ex);
        }
    }
}
