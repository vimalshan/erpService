using Dapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace UtilityService.Functions.Functions;

/// <summary>
/// Cleanup function that runs nightly to purge soft-deleted TOAD plan SQL entries older than 90 days.
/// </summary>
public class ToadPlanSqlCleanupFunction
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<ToadPlanSqlCleanupFunction> _logger;

    public ToadPlanSqlCleanupFunction(IConfiguration configuration, ILogger<ToadPlanSqlCleanupFunction> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    [Function("ToadPlanSqlNightlyCleanup")]
    public async Task RunAsync(
        [TimerTrigger("0 0 2 * * *")] TimerInfo timerInfo, // 2 AM UTC daily
        FunctionContext executionContext)
    {
        _logger.LogInformation("ToadPlanSqlNightlyCleanup started at {Time}", DateTime.UtcNow);

        var connectionString = _configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection not configured.");

        using var connection = new SqlConnection(connectionString);

        const string sql = """
            DELETE FROM TOAD_PLAN_SQL
            WHERE IS_DELETED = 1
              AND UPDATED_AT < DATEADD(DAY, -90, GETUTCDATE())
            """;

        var affected = await connection.ExecuteAsync(sql);
        _logger.LogInformation("Cleanup complete. Deleted {Count} expired TOAD plan records.", affected);

        if (timerInfo.ScheduleStatus is { } status)
        {
            _logger.LogInformation("Next cleanup scheduled at {Next}", status.Next);
        }
    }
}
