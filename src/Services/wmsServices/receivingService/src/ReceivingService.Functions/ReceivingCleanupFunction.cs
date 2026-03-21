using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ReceivingService.Functions;

/// <summary>
/// Timer-triggered Azure Function that archives / cleans up cancelled
/// receiving records older than 90 days.
/// Schedule: every Sunday at 02:00 UTC.
/// </summary>
public sealed class ReceivingCleanupFunction
{
    private readonly ILogger<ReceivingCleanupFunction> _logger;

    public ReceivingCleanupFunction(ILogger<ReceivingCleanupFunction> logger)
        => _logger = logger;

    [Function(nameof(ReceivingCleanupFunction))]
    public Task RunAsync(
        [TimerTrigger("0 0 2 * * 0")] TimerInfo timerInfo,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "ReceivingCleanup triggered at {UtcNow}. " +
            "Archiving cancelled receivings older than 90 days …",
            DateTime.UtcNow);

        // TODO: Inject IReceivingRepository and soft-delete / archive old records.
        return Task.CompletedTask;
    }
}
