using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ProjectService.Domain.Interfaces;

namespace ProjectService.Functions;

public class ProjectCleanupFunction(
    ILogger<ProjectCleanupFunction> logger,
    IDapperQueryService dapperQuery)
{
    [Function("ProjectCleanupFunction")]
    public async Task Run([TimerTrigger("0 0 2 * * *")] TimerInfo timerInfo, CancellationToken cancellationToken)
    {
        logger.LogInformation("ProjectCleanupFunction executed at: {Time}", DateTime.UtcNow);

        // Clean up old draft projects that have been inactive for more than 90 days
        var sql = @"
            UPDATE PROJECT_MAIN 
            SET PROJ_STATUS = 'D' 
            WHERE PROJ_STATUS = 'P' 
            AND PROJ_LASTMODIFIEDON < DATEADD(DAY, -90, GETUTCDATE())";

        var affected = await dapperQuery.ExecuteAsync(sql, cancellationToken: cancellationToken);
        logger.LogInformation("Cleaned up {Count} stale draft projects", affected);
    }
}
