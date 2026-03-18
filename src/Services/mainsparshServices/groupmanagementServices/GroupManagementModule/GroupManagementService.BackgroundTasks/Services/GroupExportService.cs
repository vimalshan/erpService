using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;

namespace GroupManagementService.BackgroundTasks.Services
{
    /// <summary>
    /// Service for managing group exports to Azure Blob Storage
    /// </summary>
    public interface IGroupExportService
    {
        Task ExportGroupsAsync(CancellationToken cancellationToken);
        Task ExportGroupAsync(long groupId, CancellationToken cancellationToken);
    }

    public class GroupExportService : IGroupExportService
    {
        private readonly BlobContainerClient _blobContainerClient;
        private readonly ILogger<GroupExportService> _logger;

        public GroupExportService(BlobContainerClient blobContainerClient, ILogger<GroupExportService> logger)
        {
            _blobContainerClient = blobContainerClient ?? throw new ArgumentNullException(nameof(blobContainerClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ExportGroupsAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Starting export of all groups to Azure Blob Storage");

                var fileName = $"groups_export_{DateTime.UtcNow:yyyy-MM-dd-HH-mm-ss}.json";
                var blobClient = _blobContainerClient.GetBlobClient(fileName);

                // Create dummy export data (in production, fetch from DB)
                var exportData = new { timestamp = DateTime.UtcNow, groups = new object[] { } };
                var json = System.Text.Json.JsonSerializer.Serialize(exportData);
                var bytes = System.Text.Encoding.UTF8.GetBytes(json);

                using var stream = new MemoryStream(bytes);
                await blobClient.UploadAsync(stream, overwrite: true, cancellationToken);

                _logger.LogInformation("Successfully exported groups to {FileName}", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting groups to Azure Blob Storage");
                throw;
            }
        }

        public async Task ExportGroupAsync(long groupId, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Exporting group {GroupId} to Azure Blob Storage", groupId);

                var fileName = $"group_{groupId}_{DateTime.UtcNow:yyyy-MM-dd-HH-mm-ss}.json";
                var blobClient = _blobContainerClient.GetBlobClient(fileName);

                // Create dummy export data
                var exportData = new { groupId, timestamp = DateTime.UtcNow };
                var json = System.Text.Json.JsonSerializer.Serialize(exportData);
                var bytes = System.Text.Encoding.UTF8.GetBytes(json);

                using var stream = new MemoryStream(bytes);
                await blobClient.UploadAsync(stream, overwrite: true, cancellationToken);

                _logger.LogInformation("Successfully exported group {GroupId} to {FileName}", groupId, fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting group {GroupId}", groupId);
                throw;
            }
        }
    }
}
