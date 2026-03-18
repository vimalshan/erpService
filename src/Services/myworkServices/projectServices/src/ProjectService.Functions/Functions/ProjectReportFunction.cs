using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ProjectService.Domain.Interfaces;

namespace ProjectService.Functions;

public class ProjectReportFunction(
    ILogger<ProjectReportFunction> logger,
    IDapperQueryService dapperQuery,
    IMessagePublisher publisher)
{
    [Function("ProjectReportFunction")]
    public async Task Run([TimerTrigger("0 0 8 * * MON")] TimerInfo timerInfo, CancellationToken cancellationToken)
    {
        logger.LogInformation("ProjectReportFunction executed at: {Time}", DateTime.UtcNow);

        var sql = @"
            SELECT PROJ_STATUS as Status, COUNT(*) as Count 
            FROM PROJECT_MAIN 
            GROUP BY PROJ_STATUS";

        var summary = await dapperQuery.QueryAsync<ProjectStatusSummary>(sql, cancellationToken: cancellationToken);

        await publisher.PublishAsync("project-exchange", "project.report.weekly", new
        {
            GeneratedAt = DateTime.UtcNow,
            Summary = summary
        }, cancellationToken);

        logger.LogInformation("Weekly project report generated with {Count} status groups", summary.Count);
    }
}

public record ProjectStatusSummary(char Status, int Count);
