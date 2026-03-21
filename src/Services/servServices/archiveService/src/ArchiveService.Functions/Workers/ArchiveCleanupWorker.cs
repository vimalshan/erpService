using ArchiveService.Infrastructure.Dapper;

namespace ArchiveService.Functions.Workers;

public class ArchiveCleanupWorker(
    ILogger<ArchiveCleanupWorker> logger,
    IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                logger.LogInformation("Archive cleanup task running at: {Time}", DateTimeOffset.Now);

                using var scope = serviceProvider.CreateScope();
                var dapperService = scope.ServiceProvider.GetRequiredService<DapperQueryService>();

                // Clean up records older than 7 years
                var cutoffDate = DateTime.UtcNow.AddYears(-7);
                var deletedCount = await dapperService.ExecuteAsync(
                    "DELETE FROM COPY_OLD_SERVICE_ORDER_HDR WHERE ENTERED_ON < @CutoffDate",
                    new { CutoffDate = cutoffDate }, stoppingToken);

                if (deletedCount > 0)
                    logger.LogInformation("Purged {Count} old archive records", deletedCount);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error during archive cleanup");
            }

            // Run daily
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}
