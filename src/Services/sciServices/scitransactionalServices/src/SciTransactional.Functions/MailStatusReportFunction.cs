using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SciTransactional.Application.Queries.GetAutoMailStatus;

namespace SciTransactional.Functions;

public sealed class MailStatusReportFunction(IMediator mediator, ILogger<MailStatusReportFunction> logger)
{
    [Function("MailStatusReportFunction")]
    public async Task Run([TimerTrigger("0 0 8 * * MON")] TimerInfo timerInfo, CancellationToken ct)
    {
        logger.LogInformation("MailStatusReportFunction triggered at {Time}", DateTime.UtcNow);

        var statuses = await mediator.Send(new GetAutoMailStatusQuery(), ct);

        var pendingCount = statuses.Count(s => s.MailStatus == "P");
        var completedCount = statuses.Count(s => s.MailStatus == "C");

        logger.LogInformation("Weekly Mail Report: Total={Total}, Pending={Pending}, Completed={Completed}",
            statuses.Count, pendingCount, completedCount);
    }
}
