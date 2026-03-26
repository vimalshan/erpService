using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Data.SqlClient;

namespace CanteenTransactionService.API.HealthChecks;

public class DatabaseHealthCheck : IHealthCheck
{
    private readonly IConfiguration _config;

    public DatabaseHealthCheck(IConfiguration config) => _config = config;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken ct = default)
    {
        try
        {
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await conn.OpenAsync(ct);
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT 1";
            await cmd.ExecuteScalarAsync(ct);
            return HealthCheckResult.Healthy("Database is reachable.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Database check failed.", ex);
        }
    }
}
