using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Todos.Functions.Triggers;

/// <summary>
/// Timer-triggered function for sending learning reminders
/// </summary>
public class SendLearningReminders
{
    private readonly ILogger _logger;

    public SendLearningReminders(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<SendLearningReminders>();
    }

    [Function("SendLearningReminders")]
    public async Task Run([TimerTrigger("0 0 9 * * MON-FRI")] TimerInfo myTimer)
    {
        _logger.LogInformation("Learning reminder function triggered at {UtcNow}", DateTime.UtcNow);

        // TODO: Implement logic to send learning reminders to employees
        // This could include:
        // - Querying learning records that are due for feedback
        // - Sending email notifications
        // - Publishing events to RabbitMQ

        await Task.CompletedTask;

        if (myTimer.IsPastDue)
        {
            _logger.LogWarning("Timer schedule status: IsPastDue={IsPastDue}", myTimer.IsPastDue);
        }
    }
}
