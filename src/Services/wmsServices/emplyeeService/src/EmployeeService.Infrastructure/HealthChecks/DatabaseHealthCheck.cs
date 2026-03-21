using Microsoft.Extensions.Diagnostics.HealthChecks;
using EmployeeService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EmployeeService.Infrastructure.HealthChecks;

public class DatabaseHealthCheck : IHealthCheck
{
    private readonly EmployeeDbContext _context;

    public DatabaseHealthCheck(EmployeeDbContext context)
    {
        _context = context;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Database.CanConnectAsync(cancellationToken);
            return HealthCheckResult.Healthy("Database is reachable.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Database is unreachable.", ex);
        }
    }
}
