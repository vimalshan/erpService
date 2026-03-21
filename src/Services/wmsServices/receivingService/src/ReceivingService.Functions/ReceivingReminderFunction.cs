using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ReceivingService.Functions;

/// <summary>
/// Timer-triggered Azure Function that scans for OPEN receivings older than
/// a configurable threshold and logs / alerts for follow-up.
/// Schedule: every day at 06:00 UTC.
/// </summary>
public sealed class ReceivingReminderFunction
{
    private readonly ILogger<ReceivingReminderFunction> _logger;

    public ReceivingReminderFunction(ILogger<ReceivingReminderFunction> logger)
        => _logger = logger;

    [Function(nameof(ReceivingReminderFunction))]
    public Task RunAsync(
        [TimerTrigger("0 0 6 * * *")] TimerInfo timerInfo,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "ReceivingReminder triggered at {UtcNow}. " +
            "Checking for stale OPEN receivings …",
            DateTime.UtcNow);

        // TODO: Inject IReceivingRepository and query for stale records.
        // Send notifications (email / Teams) as required.
        return Task.CompletedTask;
    }
}
