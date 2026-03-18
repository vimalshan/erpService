using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using MediatR;
using LovService.Application.Features.LovMaster.Queries;

namespace LovService.AzureFunctions.Functions;

/// <summary>
/// Timer-triggered function that runs daily to emit a summary of LOV items.
/// </summary>
public sealed class LovSummaryTimerFunction(ILogger<LovSummaryTimerFunction> logger, IMediator mediator)
{
    [Function(nameof(LovSummaryTimerFunction))]
    public async Task Run(
        [TimerTrigger("0 0 6 * * *", RunOnStartup = false)] TimerInfo timerInfo,
        CancellationToken ct)
    {
        logger.LogInformation("LOV Summary timer triggered at: {Time}", DateTime.UtcNow);

        var lovMasters = await mediator.Send(new GetAllLovMastersQuery(), ct);
        var count = lovMasters.Count();
        logger.LogInformation("Total active LOV master records: {Count}", count);

        if (timerInfo.ScheduleStatus is not null)
            logger.LogInformation("Next timer occurrence: {Next}", timerInfo.ScheduleStatus.Next);
    }
}
