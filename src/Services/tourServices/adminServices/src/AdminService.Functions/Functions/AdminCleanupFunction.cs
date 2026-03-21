using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AdminService.Functions;

public class AdminCleanupFunction
{
    private readonly ILogger<AdminCleanupFunction> _logger;

    public AdminCleanupFunction(ILogger<AdminCleanupFunction> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Runs daily at midnight UTC to clean up stale access rights logs older than 90 days.
    /// </summary>
    [Function("AdminCleanupFunction")]
    public async Task RunCleanup([TimerTrigger("0 0 0 * * *")] TimerInfo timer)
    {
        _logger.LogInformation("AdminCleanupFunction started at {Time}", DateTime.UtcNow);

        // TODO: Inject and call cleanup logic from Application layer
        // e.g., remove access rights logs older than 90 days

        _logger.LogInformation("AdminCleanupFunction completed. Next run: {NextRun}", timer.ScheduleStatus?.Next);
        await Task.CompletedTask;
    }
}
