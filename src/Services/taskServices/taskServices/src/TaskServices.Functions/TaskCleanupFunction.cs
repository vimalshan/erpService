using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TaskServices.Functions;

public class TaskCleanupFunction
{
    private readonly ILogger<TaskCleanupFunction> _logger;

    public TaskCleanupFunction(ILogger<TaskCleanupFunction> logger)
    {
        _logger = logger;
    }

    [Function("TaskCleanup")]
    public async Task Run([TimerTrigger("0 0 2 * * *")] TimerInfo timerInfo)
    {
        _logger.LogInformation("TaskCleanup function executed at: {Time}", DateTime.UtcNow);

        // Background task: clean up stale/orphaned task mails
        // In production, inject ITaskMailRepository via DI and perform cleanup
        await Task.CompletedTask;

        _logger.LogInformation("TaskCleanup completed. Next run: {NextRun}", timerInfo.ScheduleStatus?.Next);
    }
}
