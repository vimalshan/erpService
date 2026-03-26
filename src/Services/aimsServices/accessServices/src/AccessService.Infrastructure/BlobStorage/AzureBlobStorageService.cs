using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;

namespace AccessService.Infrastructure.BlobStorage
{
    /// <summary>
    /// Azure Blob Storage service implementation using the real Azure SDK
    /// Provides methods for uploading, downloading, and managing blobs in Azure Storage
    /// </summary>
    public class AzureBlobStorageService : IAzureBlobStorageService
    {
        private readonly AzureBlobStorageSettings _settings;
        private readonly ILogger<AzureBlobStorageService> _logger;
        private BlobContainerClient? _containerClient;

        public AzureBlobStorageService(AzureBlobStorageSettings settings, ILogger<AzureBlobStorageService> logger)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (string.IsNullOrWhiteSpace(_settings.ConnectionString))
            {
                _logger.LogWarning("Azure Blob Storage connection string is not configured. Blob storage will be unavailable.");
                return;
            }

            try
            {
                // Initialize connection to Azure Blob Storage
                var blobServiceClient = new BlobServiceClient(_settings.ConnectionString);
                _containerClient = blobServiceClient.GetBlobContainerClient(_settings.ContainerName);

                _logger.LogInformation($"Azure Blob Storage Service initialized for container: {_settings.ContainerName}");
            }
            catch (Exception ex)
            {
                // Log and degrade gracefully — do not throw, or every DI scope will fail
                _logger.LogError(ex, "Failed to initialize Azure Blob Storage Service. Blob storage will be unavailable.");
                _containerClient = null;
            }
        }

        public async Task<string> UploadBlobAsync(string blobName, Stream content, string contentType = "application/octet-stream")
        {
            try
            {
                if (content == null)
                    throw new ArgumentNullException(nameof(content));
                
                if (_containerClient == null)
                    throw new InvalidOperationException("Container client not initialized");

                var blobClient = _containerClient.GetBlobClient(blobName);
                
                // Reset stream position to beginning
                if (content.CanSeek)
                    content.Seek(0, SeekOrigin.Begin);

                // Upload blob with overwrite
                await blobClient.UploadAsync(content, overwrite: true);
                
                // Set content type if provided
                if (!string.IsNullOrEmpty(contentType) && contentType != "application/octet-stream")
                {
                    var httpHeaders = new BlobHttpHeaders { ContentType = contentType };
                    await blobClient.SetHttpHeadersAsync(httpHeaders);
                }

                _logger.LogInformation($"Blob uploaded successfully: {blobName}");
                return blobName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error uploading blob: {blobName}");
                throw;
            }
        }

        public async Task<Stream> DownloadBlobAsync(string blobName)
        {
            try
            {
                if (string.IsNullOrEmpty(blobName))
                    throw new ArgumentNullException(nameof(blobName));

                if (_containerClient == null)
                    throw new InvalidOperationException("Container client not initialized");

                var blobClient = _containerClient.GetBlobClient(blobName);

                // Check if blob exists
                var exists = await blobClient.ExistsAsync();
                if (!exists.Value)
                {
                    throw new FileNotFoundException($"Blob not found: {blobName}");
                }

                // Download blob to memory stream
                var download = await blobClient.DownloadAsync();
                var memoryStream = new MemoryStream();
                await download.Value.Content.CopyToAsync(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                _logger.LogInformation($"Blob downloaded successfully: {blobName}");
                return memoryStream;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error downloading blob: {blobName}");
                throw;
            }
        }

        public async Task DeleteBlobAsync(string blobName)
        {
            try
            {
                if (string.IsNullOrEmpty(blobName))
                    throw new ArgumentNullException(nameof(blobName));

                if (_containerClient == null)
                    throw new InvalidOperationException("Container client not initialized");

                var blobClient = _containerClient.GetBlobClient(blobName);
                await blobClient.DeleteIfExistsAsync();

                _logger.LogInformation($"Blob deleted successfully: {blobName}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting blob: {blobName}");
                throw;
            }
        }

        public async Task<bool> BlobExistsAsync(string blobName)
        {
            try
            {
                if (string.IsNullOrEmpty(blobName))
                    throw new ArgumentNullException(nameof(blobName));

                if (_containerClient == null)
                    throw new InvalidOperationException("Container client not initialized");

                var blobClient = _containerClient.GetBlobClient(blobName);
                var exists = await blobClient.ExistsAsync();
                
                return exists.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking blob existence: {blobName}");
                return false;
            }
        }

        public async Task<IEnumerable<string>> ListBlobsAsync(string prefix = null)
        {
            try
            {
                if (_containerClient == null)
                    throw new InvalidOperationException("Container client not initialized");

                var blobs = new List<string>();
                
                // List blobs with optional prefix filter
                // BlobTraits.None - we only need the blob name
                // BlobStates.All - include all blob states (committed and uncommitted)
                await foreach (BlobItem blobItem in _containerClient.GetBlobsAsync(BlobTraits.None, BlobStates.All, prefix, CancellationToken.None))
                {
                    blobs.Add(blobItem.Name);
                }

                _logger.LogInformation($"Listed {blobs.Count} blobs with prefix: {prefix ?? "none"}");
                return blobs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listing blobs");
                throw;
            }
        }

        public async Task<string> GetBlobSasUrlAsync(string blobName, TimeSpan expirationTime)
        {
            try
            {
                if (string.IsNullOrEmpty(blobName))
                    throw new ArgumentNullException(nameof(blobName));

                if (_containerClient == null)
                    throw new InvalidOperationException("Container client not initialized");

                var blobClient = _containerClient.GetBlobClient(blobName);

                // Check if blob exists
                var exists = await blobClient.ExistsAsync();
                if (!exists.Value)
                {
                    throw new FileNotFoundException($"Blob not found: {blobName}");
                }

                // Generate SAS URL
                var sasUri = blobClient.GenerateSasUri(BlobSasPermissions.Read, DateTime.UtcNow.Add(expirationTime));
                var sasUrl = sasUri.AbsoluteUri;

                _logger.LogInformation($"SAS URL generated for blob: {blobName}");
                return sasUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error generating SAS URL for blob: {blobName}");
                throw;
            }
        }

        public async Task<bool> IsConnectedAsync()
        {
            try
            {
                if (_containerClient == null)
                    return false;

                // Test connectivity by checking container existence
                var exists = await _containerClient.ExistsAsync();
                
                if (exists.Value)
                {
                    _logger.LogInformation("Azure Blob Storage is connected");
                    return true;
                }
                else
                {
                    _logger.LogWarning("Azure Blob Storage container does not exist");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Azure Blob Storage health check failed");
                return false;
            }
        }
    }
}
