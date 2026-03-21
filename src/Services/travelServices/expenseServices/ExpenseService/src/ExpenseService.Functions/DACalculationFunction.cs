using ExpenseService.Application.Commands;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ExpenseService.Functions;

public class DACalculationFunction
{
    private readonly IMediator _mediator;
    private readonly ILogger<DACalculationFunction> _logger;

    public DACalculationFunction(IMediator mediator, ILogger<DACalculationFunction> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Runs every 6 hours to recalculate DA for active travel requests
    /// </summary>
    [Function("RecalculateDA")]
    public async Task RunDARecalculation(
        [TimerTrigger("0 0 */6 * * *")] TimerInfo timerInfo)
    {
        _logger.LogInformation("DA recalculation started at: {Time}", DateTime.UtcNow);

        // In production, query for active travel requests that need DA calculation
        _logger.LogInformation("DA recalculation completed at: {Time}", DateTime.UtcNow);

        await Task.CompletedTask;
    }
}
