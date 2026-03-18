using Dapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace UtilityService.Functions.Functions;

/// <summary>
/// Archival function that runs every Sunday at midnight to archive old TOAD plan SQL entries.
/// Archives entries older than 30 days into an audit/archive table.
/// </summary>
public class ToadPlanSqlArchivalFunction
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<ToadPlanSqlArchivalFunction> _logger;

    public ToadPlanSqlArchivalFunction(IConfiguration configuration, ILogger<ToadPlanSqlArchivalFunction> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    [Function("ToadPlanSqlWeeklyArchival")]
    public async Task RunAsync(
        [TimerTrigger("0 0 0 * * 0")] TimerInfo timerInfo, // Sundays at midnight
        FunctionContext executionContext)
    {
        _logger.LogInformation("ToadPlanSqlWeeklyArchival started at {Time}", DateTime.UtcNow);

        var connectionString = _configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection not configured.");

        using var connection = new SqlConnection(connectionString);

        // Ensure archive table exists
        const string ensureArchive = """
            IF NOT EXISTS (
                SELECT 1 FROM INFORMATION_SCHEMA.TABLES
                WHERE TABLE_NAME = 'TOAD_PLAN_SQL_ARCHIVE'
            )
            BEGIN
                SELECT * INTO TOAD_PLAN_SQL_ARCHIVE
                FROM TOAD_PLAN_SQL WHERE 1 = 0;
                ALTER TABLE TOAD_PLAN_SQL_ARCHIVE
                    ADD ARCHIVED_AT DATETIME2 DEFAULT GETUTCDATE();
            END
            """;

        await connection.ExecuteAsync(ensureArchive);

        const string archiveSql = """
            INSERT INTO TOAD_PLAN_SQL_ARCHIVE
                (ID, USERNAME, STATEMENT_ID, TIMESTAMP, STATEMENT, IS_DELETED, CREATED_AT, UPDATED_AT, ARCHIVED_AT)
            SELECT ID, USERNAME, STATEMENT_ID, TIMESTAMP, STATEMENT, IS_DELETED, CREATED_AT, UPDATED_AT, GETUTCDATE()
            FROM TOAD_PLAN_SQL
            WHERE CREATED_AT < DATEADD(DAY, -30, GETUTCDATE())
              AND IS_DELETED = 0
            """;

        var archived = await connection.ExecuteAsync(archiveSql);
        _logger.LogInformation("Archived {Count} TOAD plan records.", archived);
    }
}
