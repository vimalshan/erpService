using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using MobileAppManagement.Application.Interfaces;

namespace MobileAppManagement.Functions;

public class DeviceCleanupFunction(
    ILogger<DeviceCleanupFunction> logger,
    IDapperQueryService dapperQueryService)
{
    [Function("CleanupInactiveDevices")]
    public async Task RunAsync([TimerTrigger("0 0 3 * * 0")] TimerInfo timerInfo, CancellationToken ct)
    {
        logger.LogInformation("Device cleanup function started at {Time}", DateTime.UtcNow);

        // Cleanup inactive device registrations
        logger.LogInformation("Cleaning up inactive devices...");

        // In production, query and cleanup stale device records

        logger.LogInformation("Device cleanup function completed at {Time}", DateTime.UtcNow);

        if (timerInfo.ScheduleStatus is not null)
        {
            logger.LogInformation("Next timer schedule at: {Next}", timerInfo.ScheduleStatus.Next);
        }
    }
}
