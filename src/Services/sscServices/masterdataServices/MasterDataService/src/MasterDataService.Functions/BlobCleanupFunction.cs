using MasterDataService.Application.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace MasterDataService.Functions;

public class BlobCleanupFunction(IBlobStorageService blobService, ILogger<BlobCleanupFunction> logger)
{
    [Function("BlobCleanup")]
    public async Task Run([TimerTrigger("0 0 2 * * *")] TimerInfo timerInfo, CancellationToken ct)
    {
        logger.LogInformation("BlobCleanup function executed at: {Time}", DateTime.UtcNow);
        // Placeholder for cleanup logic - remove orphaned blobs, etc.
        logger.LogInformation("BlobCleanup completed. Next run: {NextRun}", timerInfo.ScheduleStatus?.Next);
    }
}
