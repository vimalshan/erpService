using MasterDataService.Domain.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace MasterDataService.Functions;

public class MasterDataSyncFunction(IUnitOfWork unitOfWork, ILogger<MasterDataSyncFunction> logger)
{
    [Function("MasterDataSync")]
    public async Task Run([TimerTrigger("0 0 */6 * * *")] TimerInfo timerInfo, CancellationToken ct)
    {
        logger.LogInformation("MasterDataSync function executed at: {Time}", DateTime.UtcNow);

        var lovMasters = await unitOfWork.LovMasters.GetAllAsync(ct);
        logger.LogInformation("Synced {Count} LOV Master records", lovMasters.Count);

        var holdTypes = await unitOfWork.HoldTypeMasters.GetAllAsync(ct);
        logger.LogInformation("Synced {Count} Hold Type Master records", holdTypes.Count);

        var scanners = await unitOfWork.ScannerMasters.GetAllAsync(ct);
        logger.LogInformation("Synced {Count} Scanner Master records", scanners.Count);

        logger.LogInformation("MasterDataSync completed. Next run: {NextRun}", timerInfo.ScheduleStatus?.Next);
    }
}
