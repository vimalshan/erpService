using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using MobileAppManagement.Application.Interfaces;

namespace MobileAppManagement.Functions;

public class LoginArchiveFunction(
    ILogger<LoginArchiveFunction> logger,
    IDapperQueryService dapperQueryService)
{
    [Function("ArchiveOldLogins")]
    public async Task RunAsync([TimerTrigger("0 0 2 * * *")] TimerInfo timerInfo, CancellationToken ct)
    {
        logger.LogInformation("Login archive function started at {Time}", DateTime.UtcNow);

        // Archive login records older than 12 months
        // In production, this would call a stored procedure or run a query
        logger.LogInformation("Archiving login records older than 12 months...");

        // Example: could query and move old records
        // var oldLogins = await dapperQueryService.GetLoginsByUserAsync(...);

        logger.LogInformation("Login archive function completed at {Time}", DateTime.UtcNow);

        if (timerInfo.ScheduleStatus is not null)
        {
            logger.LogInformation("Next timer schedule at: {Next}", timerInfo.ScheduleStatus.Next);
        }
    }
}
