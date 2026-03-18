using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using OrganizationStructureService.Infrastructure.Dapper;

namespace OrganizationStructureService.Functions;

public class OrganizationBackgroundFunctions
{
    private readonly ILogger<OrganizationBackgroundFunctions> _logger;
    private readonly IDapperQueryService _dapperQueryService;

    public OrganizationBackgroundFunctions(
        ILogger<OrganizationBackgroundFunctions> logger,
        IDapperQueryService dapperQueryService)
    {
        _logger = logger;
        _dapperQueryService = dapperQueryService;
    }

    /// <summary>
    /// Runs every day at 01:00 UTC – audits inactive positions and logs them.
    /// </summary>
    [Function("AuditInactivePositions")]
    public async Task AuditInactivePositions(
        [TimerTrigger("0 0 1 * * *")] TimerInfo timerInfo)
    {
        _logger.LogInformation("AuditInactivePositions triggered at: {Time}", DateTime.UtcNow);

        const string sql = @"
            SELECT POSITION_ID, POSITION_DESIGNATION, POS_UNIT_CODE, DELETED_FLAG, POS_CLOSED_DATE
            FROM POSITION_MASTER
            WHERE DELETED_FLAG = 'Y' OR (POS_CLOSED_DATE IS NOT NULL AND POS_CLOSED_DATE < GETDATE())";

        var results = await _dapperQueryService.QueryAsync<dynamic>(sql);
        _logger.LogInformation("Found {Count} inactive/closed positions.", results.Count());
    }

    /// <summary>
    /// Runs every Sunday at 00:00 UTC – cleans up old log tables.
    /// </summary>
    [Function("CleanupLogTables")]
    public async Task CleanupLogTables(
        [TimerTrigger("0 0 0 * * 0")] TimerInfo timerInfo)
    {
        _logger.LogInformation("CleanupLogTables triggered at: {Time}", DateTime.UtcNow);

        const string sql = @"
            DELETE FROM BUSINESS_DEPTMAP_LOG WHERE BUSDEPT_LOGCREATEDON < DATEADD(YEAR, -1, GETDATE());
            DELETE FROM BUSINESS_PROCESSMAP_LOG WHERE BUSPROC_LOGCREATEDON < DATEADD(YEAR, -1, GETDATE());
            DELETE FROM UNIT_DEPARTMENT_MAP_LOG WHERE UNIT_DELETEDON < DATEADD(YEAR, -1, GETDATE());
            DELETE FROM UNIT_DIVISION_MAP_LOG WHERE UNIT_DELETEDON < DATEADD(YEAR, -1, GETDATE());
            DELETE FROM UNIT_GRADE_MAP_LOG WHERE UNIT_DELETEDON < DATEADD(YEAR, -1, GETDATE());
            DELETE FROM UNIT_PROCESS_MAP_LOG WHERE UNIT_DELETEDON < DATEADD(YEAR, -1, GETDATE());
            DELETE FROM UNIT_SITE_MAP_LOG WHERE UNIT_DELETEDON < DATEADD(YEAR, -1, GETDATE());";

        await _dapperQueryService.QueryAsync<int>(sql);
        _logger.LogInformation("Log table cleanup completed.");
    }

    /// <summary>
    /// Runs every hour – syncs business live flags based on unit activity.
    /// </summary>
    [Function("SyncBusinessLiveFlags")]
    public async Task SyncBusinessLiveFlags(
        [TimerTrigger("0 0 * * * *")] TimerInfo timerInfo)
    {
        _logger.LogInformation("SyncBusinessLiveFlags triggered at: {Time}", DateTime.UtcNow);

        const string sql = @"
            SELECT BUSINESS_ID, BUSINESS_NAME, BUSINESS_LIVFLAG,
                   (SELECT COUNT(*) FROM UNIT_MASTER u WHERE u.UNIT_BUSINESSID = b.BUSINESS_ID AND u.UNIT_LIVFLAG = 'Y') AS ActiveUnitCount
            FROM BUSINESS_MASTER b";

        var businesses = await _dapperQueryService.QueryAsync<dynamic>(sql);
        _logger.LogInformation("Processed {Count} businesses for live flag sync.", businesses.Count());
    }
}
