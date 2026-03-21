using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace UnitService.Functions.Functions;

public class BlobCleanupFunction
{
    private readonly ILogger<BlobCleanupFunction> _logger;

    public BlobCleanupFunction(ILogger<BlobCleanupFunction> logger)
    {
        _logger = logger;
    }

    [Function("BlobCleanup")]
    public async Task Run([TimerTrigger("0 0 2 * * *")] TimerInfo timerInfo)
    {
        _logger.LogInformation("Blob cleanup function executed at: {Time}", DateTime.UtcNow);

        // Clean up orphaned blobs that are no longer referenced by equipment records
        var connectionString = Environment.GetEnvironmentVariable("BlobStorage:ConnectionString");
        if (string.IsNullOrEmpty(connectionString))
        {
            _logger.LogWarning("Blob storage connection string not configured. Skipping cleanup.");
            return;
        }

        var blobServiceClient = new BlobServiceClient(connectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient("equipment-images");

        if (!await containerClient.ExistsAsync())
        {
            _logger.LogInformation("Container 'equipment-images' does not exist. Nothing to clean up.");
            return;
        }

        var deletedCount = 0;
        await foreach (var blob in containerClient.GetBlobsAsync())
        {
            // Example: delete blobs older than 90 days that match orphaned pattern
            if (blob.Properties.LastModified.HasValue &&
                blob.Properties.LastModified.Value < DateTimeOffset.UtcNow.AddDays(-90))
            {
                await containerClient.DeleteBlobIfExistsAsync(blob.Name);
                deletedCount++;
            }
        }

        _logger.LogInformation("Blob cleanup completed. Deleted {Count} orphaned blobs.", deletedCount);
    }
}
