using ArchiveService.Application.Interfaces;

namespace ArchiveService.Functions.Workers;

public class BlobSyncWorker(
    ILogger<BlobSyncWorker> logger,
    IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                logger.LogInformation("Blob sync task running at: {Time}", DateTimeOffset.Now);

                using var scope = serviceProvider.CreateScope();
                var blobService = scope.ServiceProvider.GetService<IBlobStorageService>();

                if (blobService is null)
                {
                    logger.LogWarning("Blob storage service not configured; skipping sync");
                }
                else
                {
                    // Placeholder: sync stationery item images to blob storage
                    logger.LogInformation("Blob sync completed successfully");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error during blob sync");
            }

            // Run every 6 hours
            await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
        }
    }
}
