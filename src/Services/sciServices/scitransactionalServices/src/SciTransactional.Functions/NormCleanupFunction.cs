using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SciTransactional.Application.Commands.CloseNorm;
using SciTransactional.Application.Queries.GetAllNorms;

namespace SciTransactional.Functions;

public sealed class NormCleanupFunction(IMediator mediator, ILogger<NormCleanupFunction> logger)
{
    [Function("NormCleanupFunction")]
    public async Task Run([TimerTrigger("0 0 0 * * *")] TimerInfo timerInfo, CancellationToken ct)
    {
        logger.LogInformation("NormCleanupFunction triggered at {Time}", DateTime.UtcNow);

        var norms = await mediator.Send(new GetAllNormsQuery(), ct);
        var closedCount = 0;

        foreach (var norm in norms)
        {
            if (norm.ClosureDate.HasValue && norm.ClosureDate.Value < DateTime.UtcNow.Date)
            {
                closedCount++;
                logger.LogInformation("Closing expired norm {NormNo} (closure: {Date})",
                    norm.NormNo, norm.ClosureDate);
                await mediator.Send(new CloseNormCommand(norm.NormNo), ct);
            }
        }

        logger.LogInformation("NormCleanupFunction completed. Closed {Count} expired norms.", closedCount);
    }
}
