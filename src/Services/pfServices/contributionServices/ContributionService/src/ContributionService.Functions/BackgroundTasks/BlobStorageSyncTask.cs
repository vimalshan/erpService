using ContributionService.Application.Interfaces;

namespace ContributionService.Functions.BackgroundTasks;

public class BlobStorageSyncTask(
    IServiceScopeFactory scopeFactory,
    ILogger<BlobStorageSyncTask> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Blob Storage Sync Task started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Periodic sync for stationery item images
                var now = DateTime.UtcNow;
                if (now.Hour == 4 && now.Minute == 0)
                {
                    logger.LogInformation("Running blob storage sync for stationery images");
                    using var scope = scopeFactory.CreateScope();
                    var blobService = scope.ServiceProvider.GetRequiredService<IBlobStorageService>();

                    // Verify container exists
                    var url = await blobService.GetUrlAsync("stationery-images", "sync-check.txt", stoppingToken);
                    logger.LogInformation("Blob storage sync completed. Container URL: {Url}", url);
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Blob storage sync encountered an issue (non-critical)");
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
