using Azure.Storage.Blobs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace RiskService.Functions.Functions;

/// <summary>
/// Background task that periodically cleans up orphaned blob attachments.
/// Runs once daily.
/// </summary>
public class BlobCleanupService : BackgroundService
{
    private readonly ILogger<BlobCleanupService> _logger;
    private readonly string? _blobConnectionString;
    private readonly TimeSpan _interval = TimeSpan.FromHours(24);

    public BlobCleanupService(ILogger<BlobCleanupService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _blobConnectionString = configuration.GetValue<string>("AzureBlobStorage:ConnectionString");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("BlobCleanupService started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CleanupOrphanedBlobsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cleaning up orphaned blobs");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }

    private async Task CleanupOrphanedBlobsAsync(CancellationToken ct)
    {
        if (string.IsNullOrEmpty(_blobConnectionString))
        {
            _logger.LogInformation("Blob storage not configured, skipping cleanup");
            return;
        }

        var client = new BlobServiceClient(_blobConnectionString);
        var containerClient = client.GetBlobContainerClient("risk-attachments-temp");

        if (!await containerClient.ExistsAsync(ct))
        {
            _logger.LogInformation("Temp container does not exist, nothing to clean up");
            return;
        }

        var deletedCount = 0;
        await foreach (var blobItem in containerClient.GetBlobsAsync(cancellationToken: ct))
        {
            if (blobItem.Properties.CreatedOn < DateTimeOffset.UtcNow.AddDays(-1))
            {
                await containerClient.DeleteBlobAsync(blobItem.Name, cancellationToken: ct);
                deletedCount++;
            }
        }

        _logger.LogInformation("Cleaned up {Count} orphaned temp blobs", deletedCount);
    }
}
