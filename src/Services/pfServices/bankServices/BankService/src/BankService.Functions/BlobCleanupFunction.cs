using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BankService.Functions;

public class BlobCleanupFunction(ILogger<BlobCleanupFunction> logger)
{
    [Function("BlobCleanupTimer")]
    public async Task RunAsync([TimerTrigger("0 0 3 * * 0")] TimerInfo timerInfo)
    {
        logger.LogInformation("Blob Cleanup function started at: {Time}", DateTime.UtcNow);

        // Weekly cleanup of orphaned blobs in the stationery-images container
        // In production, connect to blob storage and clean up old/orphaned blobs
        await Task.CompletedTask;

        logger.LogInformation("Blob Cleanup function completed at: {Time}", DateTime.UtcNow);
    }
}
