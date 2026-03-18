using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Timer;
using Microsoft.Extensions.Logging;

namespace RecruitmentService.API.Functions;

public class NotificationFunction
{
    private readonly ILogger<NotificationFunction> _logger;

    public NotificationFunction(ILogger<NotificationFunction> logger) => _logger = logger;

    /// <summary>
    /// Background Azure Function triggered by a timer every day at midnight UTC.
    /// Sends reminders for vacancies nearing their close date.
    /// </summary>
    [Function("VacancyReminderFunction")]
    public async Task RunVacancyReminder(
        [TimerTrigger("0 0 0 * * *")] TimerInfo timer)
    {
        _logger.LogInformation("[Azure Function] VacancyReminderFunction triggered at {Time}", DateTime.UtcNow);
        // TODO: Query vacancies closing within 3 days and dispatch notification emails.
        await Task.CompletedTask;
    }

    /// <summary>
    /// Background Azure Function triggered by a timer every hour.
    /// Cleans up expired draft applications.
    /// </summary>
    [Function("DraftApplicationCleanupFunction")]
    public async Task RunDraftCleanup(
        [TimerTrigger("0 0 * * * *")] TimerInfo timer)
    {
        _logger.LogInformation("[Azure Function] DraftApplicationCleanupFunction triggered at {Time}", DateTime.UtcNow);
        // TODO: Remove stale draft applications older than 30 days.
        await Task.CompletedTask;
    }
}
