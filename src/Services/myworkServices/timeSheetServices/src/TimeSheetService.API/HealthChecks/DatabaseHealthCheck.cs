using Microsoft.Extensions.Diagnostics.HealthChecks;
using TimeSheetService.Infrastructure.Persistence;

namespace TimeSheetService.API.HealthChecks;

public class DatabaseHealthCheck : IHealthCheck
{
    private readonly TimeSheetDbContext _context;

    public DatabaseHealthCheck(TimeSheetDbContext context) => _context = context;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var canConnect = await _context.Database.CanConnectAsync(cancellationToken);
            return canConnect
                ? HealthCheckResult.Healthy("Database connection is healthy.")
                : HealthCheckResult.Unhealthy("Cannot connect to database.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Database health check failed.", ex);
        }
    }
}
