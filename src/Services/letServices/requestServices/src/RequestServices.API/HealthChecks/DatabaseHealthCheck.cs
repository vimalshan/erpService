using Microsoft.Extensions.Diagnostics.HealthChecks;
using RequestServices.Infrastructure.Data;

namespace RequestServices.API.HealthChecks;

/// <summary>Custom health check that verifies the EF DbContext can reach the database.</summary>
public class DatabaseHealthCheck(RequestDbContext context) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext hcContext, CancellationToken ct = default)
    {
        try
        {
            await context.Database.CanConnectAsync(ct);
            return HealthCheckResult.Healthy("Database connection is healthy.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Database connection failed.", ex);
        }
    }
}
