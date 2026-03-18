using Microsoft.Extensions.Diagnostics.HealthChecks;
using BusServices.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BusServices.API.HealthChecks;

public sealed class BusDbHealthCheck : IHealthCheck
{
    private readonly BusDbContext _ctx;

    public BusDbHealthCheck(BusDbContext ctx) => _ctx = ctx;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken ct = default)
    {
        try
        {
            await _ctx.Database.ExecuteSqlRawAsync("SELECT 1", ct);
            return HealthCheckResult.Healthy("BusDB is reachable.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("BusDB is unreachable.", ex);
        }
    }
}
