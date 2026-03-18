using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BatchService.Functions;

/// <summary>
/// Timer-triggered Azure Function that runs at midnight on the 1st of every month
/// to close all open batches for the previous month.
/// CRON: "0 0 0 1 * *" = At 00:00 on 1st of every month.
/// </summary>
public sealed class BatchProcessingFunction
{
    private readonly ILogger<BatchProcessingFunction> _logger;

    public BatchProcessingFunction(ILogger<BatchProcessingFunction> logger)
        => _logger = logger;

    [Function(nameof(BatchProcessingFunction))]
    public void Run([TimerTrigger("0 0 0 1 * *")] TimerInfo timerInfo)
    {
        _logger.LogInformation(
            "[BatchProcessingFunction] Triggered at {UtcNow}. Next schedule: {Next}",
            DateTime.UtcNow,
            timerInfo.ScheduleStatus?.Next);

        // TODO: Resolve MediatR and dispatch CloseBatchCommand for open batches
        // This requires a proper DI setup for the isolated worker model.
    }
}
