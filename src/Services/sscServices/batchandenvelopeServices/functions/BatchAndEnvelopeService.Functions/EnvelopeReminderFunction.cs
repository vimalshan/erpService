using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BatchAndEnvelopeService.Functions;

public class EnvelopeReminderFunction
{
    private readonly ILogger<EnvelopeReminderFunction> _logger;

    public EnvelopeReminderFunction(ILogger<EnvelopeReminderFunction> logger)
        => _logger = logger;

    /// <summary>
    /// Runs every hour to check for unconfirmed envelopes older than 24 hours and send reminders.
    /// </summary>
    [Function("EnvelopeReminder")]
    public void Run([TimerTrigger("0 0 * * * *")] TimerInfo timerInfo)
    {
        _logger.LogInformation("[Function] EnvelopeReminder triggered at: {Time}", DateTime.UtcNow);
        // TODO: Query envelopes with SummaryFlag='N' older than 24h and send notification
        _logger.LogInformation("[Function] EnvelopeReminder completed.");
    }
}
