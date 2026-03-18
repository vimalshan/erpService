using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;

namespace LocationService.Infrastructure.ExternalServices
{
    /// <summary>
    /// Interface for Azure Blob Storage operations
    /// </summary>
    public interface IBlobStorageService
    {
        Task<string> UploadAsync(string containerName, string fileName, Stream fileStream, CancellationToken cancellationToken = default);
        Task<Stream?> DownloadAsync(string containerName, string fileName, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string containerName, string fileName, CancellationToken cancellationToken = default);
        Task<IEnumerable<string>> ListAsync(string containerName, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Azure Blob Storage service implementation
    /// </summary>
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ILogger<BlobStorageService> _logger;

        public BlobStorageService(BlobServiceClient blobServiceClient, ILogger<BlobStorageService> logger)
        {
            _blobServiceClient = blobServiceClient;
            _logger = logger;
        }

        public async Task<string> UploadAsync(string containerName, string fileName, Stream fileStream, CancellationToken cancellationToken = default)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                await containerClient.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: cancellationToken);

                var blobClient = containerClient.GetBlobClient(fileName);
                fileStream.Seek(0, SeekOrigin.Begin);
                var uploadInfo = await blobClient.UploadAsync(fileStream, overwrite: true, cancellationToken);

                _logger.LogInformation("File uploaded to blob storage: {FileName}", fileName);
                return blobClient.Uri.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file to blob storage: {FileName}", fileName);
                throw;
            }
        }

        public async Task<Stream?> DownloadAsync(string containerName, string fileName, CancellationToken cancellationToken = default)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(fileName);

                if (!await blobClient.ExistsAsync(cancellationToken))
                {
                    _logger.LogWarning("File not found in blob storage: {FileName}", fileName);
                    return null;
                }

                var download = await blobClient.DownloadAsync(cancellationToken);
                return download.Value.Content;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading file from blob storage: {FileName}", fileName);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(string containerName, string fileName, CancellationToken cancellationToken = default)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(fileName);

                var response = await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
                _logger.LogInformation("File deleted from blob storage: {FileName}", fileName);
                return response.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file from blob storage: {FileName}", fileName);
                throw;
            }
        }

        public async Task<IEnumerable<string>> ListAsync(string containerName, CancellationToken cancellationToken = default)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                var result = new List<string>();
                await foreach (var blobItem in containerClient.GetBlobsAsync(cancellationToken: cancellationToken))
                {
                    result.Add(blobItem.Name);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listing files in blob storage container: {ContainerName}", containerName);
                throw;
            }
        }
    }
}
