using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace UnitService.Functions.Functions;

public class EquipmentStatusSyncFunction
{
    private readonly ILogger<EquipmentStatusSyncFunction> _logger;

    public EquipmentStatusSyncFunction(ILogger<EquipmentStatusSyncFunction> logger)
    {
        _logger = logger;
    }

    [Function("EquipmentStatusSync")]
    public async Task Run([TimerTrigger("0 */30 * * * *")] TimerInfo timerInfo)
    {
        _logger.LogInformation("Equipment status sync function executed at: {Time}", DateTime.UtcNow);

        // Sync equipment statuses - check for stale statuses, send notifications, etc.
        _logger.LogInformation("Next timer schedule at: {Next}", timerInfo.ScheduleStatus?.Next);

        await Task.CompletedTask;
    }
}
