using EnergyService.Infrastructure.Persistence;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace EnergyService.API.HealthChecks;

public class DatabaseHealthCheck : IHealthCheck
{
    private readonly EnergyDbContext _context;

    public DatabaseHealthCheck(EnergyDbContext context) => _context = context;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken ct = default)
    {
        try
        {
            await _context.Database.CanConnectAsync(ct);
            return HealthCheckResult.Healthy("Database connection is healthy.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Database connection failed.", ex);
        }
    }
}
