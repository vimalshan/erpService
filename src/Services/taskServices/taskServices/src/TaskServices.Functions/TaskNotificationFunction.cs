using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TaskServices.Functions;

public class TaskNotificationFunction
{
    private readonly ILogger<TaskNotificationFunction> _logger;

    public TaskNotificationFunction(ILogger<TaskNotificationFunction> logger)
    {
        _logger = logger;
    }

    [Function("TaskNotification")]
    public async Task Run([TimerTrigger("0 */5 * * * *")] TimerInfo timerInfo)
    {
        _logger.LogInformation("TaskNotification function executed at: {Time}", DateTime.UtcNow);

        // Background task: process pending notifications
        // In production, check for new/unprocessed task mails and send notifications
        await Task.CompletedTask;

        _logger.LogInformation("TaskNotification completed. Next run: {NextRun}", timerInfo.ScheduleStatus?.Next);
    }
}
