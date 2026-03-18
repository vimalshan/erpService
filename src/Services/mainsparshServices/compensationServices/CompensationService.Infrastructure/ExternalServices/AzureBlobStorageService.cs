using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CompensationService.Infrastructure.ExternalServices;

/// <summary>
/// Service for managing stationery item images in Azure Blob Storage
/// </summary>
public interface IBlobStorageService
{
    Task<string> UploadImageAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default);
    Task<Stream> DownloadImageAsync(string blobName, CancellationToken cancellationToken = default);
    Task DeleteImageAsync(string blobName, CancellationToken cancellationToken = default);
}

/// <summary>
/// Implementation of Azure Blob Storage Service
/// </summary>
public class AzureBlobStorageService : IBlobStorageService
{
    private readonly BlobContainerClient _containerClient;
    private readonly ILogger<AzureBlobStorageService> _logger;

    public AzureBlobStorageService(BlobServiceClient blobServiceClient, IConfiguration configuration, ILogger<AzureBlobStorageService> logger)
    {
        _logger = logger;
        var containerName = configuration["AzureStorage:ContainerName"] ?? "stationery-images";
        _containerClient = blobServiceClient.GetBlobContainerClient(containerName);
    }

    public async Task<string> UploadImageAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default)
    {
        try
        {
            var blobClient = _containerClient.GetBlobClient(fileName);
            var result = await blobClient.UploadAsync(fileStream, overwrite: true, cancellationToken);
            _logger.LogInformation($"Image uploaded successfully: {fileName}");
            return blobClient.Uri.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error uploading image: {ex.Message}");
            throw;
        }
    }

    public async Task<Stream> DownloadImageAsync(string blobName, CancellationToken cancellationToken = default)
    {
        try
        {
            var blobClient = _containerClient.GetBlobClient(blobName);
            var download = await blobClient.DownloadAsync(cancellationToken);
            return download.Value.Content;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error downloading image: {ex.Message}");
            throw;
        }
    }

    public async Task DeleteImageAsync(string blobName, CancellationToken cancellationToken = default)
    {
        try
        {
            var blobClient = _containerClient.GetBlobClient(blobName);
            await blobClient.DeleteAsync(cancellationToken: cancellationToken);
            _logger.LogInformation($"Image deleted successfully: {blobName}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting image: {ex.Message}");
            throw;
        }
    }
}
