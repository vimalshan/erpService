using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace ConfigService.AzureFunctions;

public class BlobCleanupWorker(ILogger<BlobCleanupWorker> logger, IConfiguration configuration) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Blob Cleanup Worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var connectionString = configuration.GetValue<string>("AzureBlobStorage:ConnectionString");
                if (!string.IsNullOrEmpty(connectionString))
                {
                    var blobClient = new BlobServiceClient(connectionString);
                    var container = blobClient.GetBlobContainerClient("stationery-images");

                    if (await container.ExistsAsync(stoppingToken))
                    {
                        var cutoff = DateTimeOffset.UtcNow.AddDays(-90);
                        var deletedCount = 0;

                        await foreach (var blob in container.GetBlobsAsync(BlobTraits.Metadata, BlobStates.None, prefix: null, cancellationToken: stoppingToken))
                        {
                            if (blob.Properties.LastModified < cutoff)
                            {
                                await container.DeleteBlobIfExistsAsync(blob.Name, cancellationToken: stoppingToken);
                                deletedCount++;
                            }
                        }

                        if (deletedCount > 0)
                            logger.LogInformation("Blob cleanup: deleted {Count} old blobs.", deletedCount);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in blob cleanup worker.");
            }

            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}
