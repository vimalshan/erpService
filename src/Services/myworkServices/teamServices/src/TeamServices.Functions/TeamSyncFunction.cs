using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TeamServices.Functions;

public class TeamSyncFunction
{
    private readonly ILogger<TeamSyncFunction> _logger;

    public TeamSyncFunction(ILogger<TeamSyncFunction> logger)
    {
        _logger = logger;
    }

    [Function("TeamSyncTimer")]
    public Task RunAsync([TimerTrigger("0 0 */6 * * *")] TimerInfo timerInfo)
    {
        _logger.LogInformation("TeamSyncTimer executed at: {Time}", DateTime.UtcNow);

        if (timerInfo.ScheduleStatus is not null)
        {
            _logger.LogInformation("Next timer schedule at: {Next}", timerInfo.ScheduleStatus.Next);
        }

        // Add team synchronization logic here
        // e.g., sync team data with external systems, clean up expired memberships
        _logger.LogInformation("Team synchronization completed successfully.");
        return Task.CompletedTask;
    }
}
