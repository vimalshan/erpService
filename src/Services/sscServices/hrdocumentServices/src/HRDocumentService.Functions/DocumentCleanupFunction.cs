using HRDocumentService.Application.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace HRDocumentService.Functions;

public class DocumentCleanupFunction(
    IDapperQueryService dapperQuery,
    ILogger<DocumentCleanupFunction> logger)
{
    [Function("DocumentCleanupFunction")]
    public async Task Run([TimerTrigger("0 0 2 * * *")] TimerInfo timerInfo, CancellationToken ct)
    {
        logger.LogInformation("DocumentCleanupFunction started at {Time}", DateTime.UtcNow);

        // Clean up draft documents older than 30 days
        var sql = @"
            UPDATE HRDOC_DET 
            SET DOC_CANCELFLAG = 'Y', DOC_DOCSTATUS = 'CN', DOC_CANCELON = @Now 
            WHERE DOC_DOCSTATUS = 'DR' 
            AND DOC_CREATEDON < @CutoffDate 
            AND DOC_CANCELFLAG IS NULL";

        var cutoffDate = DateTime.UtcNow.AddDays(-30);

        var affected = await dapperQuery.QueryFirstOrDefaultAsync<int>(
            "SELECT COUNT(*) FROM HRDOC_DET WHERE DOC_DOCSTATUS = 'DR' AND DOC_CREATEDON < @CutoffDate AND DOC_CANCELFLAG IS NULL",
            new { CutoffDate = cutoffDate }, ct);

        logger.LogInformation("Found {Count} stale draft documents to clean up.", affected);

        if (affected > 0)
        {
            await dapperQuery.QueryAsync<int>(sql, new { Now = DateTime.UtcNow, CutoffDate = cutoffDate }, ct);
            logger.LogInformation("Cleaned up {Count} stale draft documents.", affected);
        }

        logger.LogInformation("DocumentCleanupFunction completed. Next run: {NextRun}", timerInfo.ScheduleStatus?.Next);
    }
}
