using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ObjectiveService.AzureFunctions;

/// <summary>
/// Timer-triggered Azure Function that sends goal review reminders.
/// Runs every day at 08:00 UTC.
/// </summary>
public class GoalReminderFunction
{
    private readonly ILogger<GoalReminderFunction> _logger;

    public GoalReminderFunction(ILogger<GoalReminderFunction> logger) => _logger = logger;

    [Function("GoalReviewReminder")]
    public async Task Run(
        [TimerTrigger("0 0 8 * * *")] TimerInfo timerInfo,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("GoalReviewReminder triggered at {Time}", DateTime.UtcNow);

        if (timerInfo.ScheduleStatus is not null)
        {
            _logger.LogInformation(
                "Last run: {Last} | Next run: {Next}",
                timerInfo.ScheduleStatus.Last,
                timerInfo.ScheduleStatus.Next);
        }

        // TODO: query goals with next review date = today and send notification via email / push
        await Task.CompletedTask;

        _logger.LogInformation("GoalReviewReminder completed.");
    }
}
