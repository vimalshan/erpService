using FillingOperationService.Application.FillingPlants.Queries.GetFillingPlants;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker.Extensions.Timer;
using System.Net;

namespace FillingOperationService.Functions;

public class FillingOperationTimerFunction(ILogger<FillingOperationTimerFunction> logger, IMediator mediator)
{
    /// <summary>
    /// Timer trigger — runs every 6 hours to sync/report filling operations data.
    /// </summary>
    [Function("SyncFillingOperationsData")]
    public async Task RunTimerAsync(
        [TimerTrigger("0 0 */6 * * *")] TimerInfo timer,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("SyncFillingOperationsData started at: {Time}", DateTime.UtcNow);

        var plants = await mediator.Send(new GetFillingPlantsQuery(), cancellationToken);
        logger.LogInformation("Found {Count} filling plants during sync.", plants.Count());

        if (timer.ScheduleStatus?.Next is not null)
            logger.LogInformation("Next run scheduled at: {Next}", timer.ScheduleStatus.Next);
    }
}
