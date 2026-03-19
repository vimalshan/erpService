using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MobileExpenseManagement.Application.Common.Interfaces;

namespace MobileExpenseManagement.Infrastructure.BlobStorage;

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

    public async Task<string> UploadFileAsync(string containerName, string blobName, byte[] fileContent, 
        string contentType, CancellationToken cancellationToken = default)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

            var blobClient = containerClient.GetBlobClient(blobName);
            
            using (var stream = new MemoryStream(fileContent))
            {
                await blobClient.UploadAsync(stream, overwrite: true, cancellationToken: cancellationToken);
            }

            // Set content type
            var blobProperties = new BlobHttpHeaders { ContentType = contentType };
            await blobClient.SetHttpHeadersAsync(blobProperties, cancellationToken: cancellationToken);

            _logger.LogInformation($"File uploaded successfully: {containerName}/{blobName}");
            return blobClient.Uri.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error uploading file to blob storage: {containerName}/{blobName}");
            throw;
        }
    }

    public async Task<byte[]> DownloadFileAsync(string containerName, string blobName, CancellationToken cancellationToken = default)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            var download = await blobClient.DownloadAsync(cancellationToken: cancellationToken);

            using (var ms = new MemoryStream())
            {
                await download.Value.Content.CopyToAsync(ms, cancellationToken);
                _logger.LogInformation($"File downloaded successfully: {containerName}/{blobName}");
                return ms.ToArray();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error downloading file from blob storage: {containerName}/{blobName}");
            throw;
        }
    }

    public async Task<bool> DeleteFileAsync(string containerName, string blobName, CancellationToken cancellationToken = default)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            var result = await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
            
            if (result)
            {
                _logger.LogInformation($"File deleted successfully: {containerName}/{blobName}");
            }
            else
            {
                _logger.LogWarning($"File not found for deletion: {containerName}/{blobName}");
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting file from blob storage: {containerName}/{blobName}");
            throw;
        }
    }

    public async Task<Uri> GetFileUriAsync(string containerName, string blobName)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);
            return blobClient.Uri;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting file URI: {containerName}/{blobName}");
            throw;
        }
    }
}
