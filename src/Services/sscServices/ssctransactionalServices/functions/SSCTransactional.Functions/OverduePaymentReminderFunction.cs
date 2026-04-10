using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace SSCTransactional.Functions;

public class OverduePaymentReminderFunction
{
    private readonly ILogger<OverduePaymentReminderFunction> _logger;

    public OverduePaymentReminderFunction(ILogger<OverduePaymentReminderFunction> logger)
        => _logger = logger;

    /// <summary>
    /// Runs every hour to check for overdue Oracle payment entries and send reminders.
    /// </summary>
    [Function("OverduePaymentReminder")]
    public void Run([TimerTrigger("0 0 * * * *")] TimerInfo timerInfo)
    {
        _logger.LogInformation("[Function] OverduePaymentReminder triggered at: {Time}", DateTime.UtcNow);
        // TODO: Query DOC_ORACLEDUEDET for overdue payments and send notification
        _logger.LogInformation("[Function] OverduePaymentReminder completed.");
    }
}
