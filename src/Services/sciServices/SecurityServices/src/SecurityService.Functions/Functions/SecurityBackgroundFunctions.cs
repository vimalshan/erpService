using Dapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SecurityService.Functions.Functions;

/// <summary>
/// Timer-triggered function: runs daily to expire user roles whose end date has passed.
/// </summary>
public sealed class ExpireUserRolesFunction
{
    private readonly IConfiguration _config;
    private readonly ILogger<ExpireUserRolesFunction> _logger;

    public ExpireUserRolesFunction(IConfiguration config, ILogger<ExpireUserRolesFunction> logger)
    {
        _config = config;
        _logger = logger;
    }

    [Function(nameof(ExpireUserRolesFunction))]
    public async Task Run([TimerTrigger("0 0 1 * * *")] TimerInfo timerInfo)
    {
        _logger.LogInformation("ExpireUserRolesFunction triggered at {UtcNow}", DateTime.UtcNow);

        var connectionString = _config.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection not set.");

        await using var conn = new SqlConnection(connectionString);
        var affected = await conn.ExecuteAsync("""
            UPDATE [USER_ROLE]
            SET    [UR_UPD_DAT] = GETDATE()
            WHERE  [UR_END_DAT] IS NOT NULL
              AND  [UR_END_DAT] < GETDATE()
            """);

        _logger.LogInformation("Expired {Count} user role(s).", affected);
    }
}

/// <summary>
/// Timer-triggered function: sends a daily audit summary of user changes.
/// </summary>
public sealed class DailyAuditSummaryFunction
{
    private readonly IConfiguration _config;
    private readonly ILogger<DailyAuditSummaryFunction> _logger;

    public DailyAuditSummaryFunction(IConfiguration config, ILogger<DailyAuditSummaryFunction> logger)
    {
        _config = config;
        _logger = logger;
    }

    [Function(nameof(DailyAuditSummaryFunction))]
    public async Task Run([TimerTrigger("0 0 6 * * *")] TimerInfo timerInfo)
    {
        _logger.LogInformation("DailyAuditSummaryFunction triggered at {UtcNow}", DateTime.UtcNow);

        var connectionString = _config.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection not set.");

        await using var conn = new SqlConnection(connectionString);
        var results = await conn.QueryAsync<dynamic>("""
            SELECT COUNT(*)               AS TotalUsers,
                   SUM(CASE WHEN UM_END_DAT IS NULL OR UM_END_DAT >= GETDATE() THEN 1 ELSE 0 END) AS ActiveUsers,
                   (SELECT COUNT(*) FROM ROLE_MAST)   AS TotalRoles,
                   (SELECT COUNT(*) FROM USER_ROLE
                    WHERE  UR_STR_DAT >= CAST(GETDATE()-1 AS DATE)) AS RolesAssignedLastDay
            FROM   USER_MASTER
            """);

        foreach (var row in results)
        {
            _logger.LogInformation(
                "Audit Summary — TotalUsers: {TotalUsers}, ActiveUsers: {ActiveUsers}, TotalRoles: {TotalRoles}, RolesAssignedLastDay: {RolesAssignedLastDay}",
                (int)row.TotalUsers, (int)row.ActiveUsers, (int)row.TotalRoles, (int)row.RolesAssignedLastDay);
        }
    }
}

/// <summary>
/// Timer-triggered function: cleans up old ACCESS_ROLE records without a primary key.
/// </summary>
public sealed class CleanupAccessRolesFunction
{
    private readonly IConfiguration _config;
    private readonly ILogger<CleanupAccessRolesFunction> _logger;

    public CleanupAccessRolesFunction(IConfiguration config, ILogger<CleanupAccessRolesFunction> logger)
    {
        _config = config;
        _logger = logger;
    }

    [Function(nameof(CleanupAccessRolesFunction))]
    public async Task Run([TimerTrigger("0 30 2 * * 0")] TimerInfo timerInfo) // every Sunday 02:30
    {
        _logger.LogInformation("CleanupAccessRolesFunction triggered at {UtcNow}", DateTime.UtcNow);

        var connectionString = _config.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection not set.");

        await using var conn = new SqlConnection(connectionString);
        var affected = await conn.ExecuteAsync("""
            DELETE FROM [ACCESS_ROLE]
            WHERE  [RA_END_DAT] IS NOT NULL
              AND  [RA_END_DAT] < DATEADD(MONTH, -6, GETDATE())
            """);

        _logger.LogInformation("CleanupAccessRolesFunction removed {Count} stale record(s).", affected);
    }
}
