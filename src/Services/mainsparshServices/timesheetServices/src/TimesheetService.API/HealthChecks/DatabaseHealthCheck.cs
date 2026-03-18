using Microsoft.Extensions.Diagnostics.HealthChecks;
using TimesheetService.Infrastructure.Data;

namespace TimesheetService.API.HealthChecks;

public sealed class DatabaseHealthCheck : IHealthCheck
{
    private readonly TimesheetDbContext _context;

    public DatabaseHealthCheck(TimesheetDbContext context) => _context = context;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            if (await _context.Database.CanConnectAsync(cancellationToken))
                return HealthCheckResult.Healthy("Database is reachable.");

            return HealthCheckResult.Unhealthy("Cannot connect to the database.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Database health check failed.", ex);
        }
    }
}
