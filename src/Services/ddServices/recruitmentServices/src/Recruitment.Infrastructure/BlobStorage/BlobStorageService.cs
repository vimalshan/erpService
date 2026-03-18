using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Recruitment.Infrastructure.BlobStorage;

/// <summary>
/// Azure Blob Storage service for document management
/// </summary>
public interface IBlobStorageService
{
    Task<string> UploadAsync(string containerName, string blobName, Stream content, string contentType);
    Task<Stream> DownloadAsync(string containerName, string blobName);
    Task<bool> DeleteAsync(string containerName, string blobName);
    Task<IEnumerable<string>> ListBlobsAsync(string containerName);
}

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobContainerClient _containerClient;
    private readonly ILogger<BlobStorageService> _logger;

    public BlobStorageService(IConfiguration configuration, ILogger<BlobStorageService> logger)
    {
        var connectionString = configuration["AzureBlob:ConnectionString"];
        var containerName = configuration["AzureBlob:ContainerName"];
        
        var blobServiceClient = new BlobServiceClient(connectionString);
        _containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        _logger = logger;
    }

    public async Task<string> UploadAsync(string containerName, string blobName, Stream content, string contentType)
    {
        try
        {
            var blobClient = _containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(content, overwrite: true);
            
            _logger.LogInformation($"Blob uploaded successfully: {blobName}");
            return blobClient.Uri.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error uploading blob: {blobName}");
            throw;
        }
    }

    public async Task<Stream> DownloadAsync(string containerName, string blobName)
    {
        try
        {
            var blobClient = _containerClient.GetBlobClient(blobName);
            var download = await blobClient.DownloadAsync();
            
            _logger.LogInformation($"Blob downloaded successfully: {blobName}");
            return download.Value.Content;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error downloading blob: {blobName}");
            throw;
        }
    }

    public async Task<bool> DeleteAsync(string containerName, string blobName)
    {
        try
        {
            var blobClient = _containerClient.GetBlobClient(blobName);
            var result = await blobClient.DeleteIfExistsAsync();
            
            _logger.LogInformation($"Blob deleted: {blobName} - {(result ? "Success" : "Not Found")}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting blob: {blobName}");
            throw;
        }
    }

    public async Task<IEnumerable<string>> ListBlobsAsync(string containerName)
    {
        try
        {
            var blobs = new List<string>();
            await foreach (BlobItem blobItem in _containerClient.GetBlobsAsync())
            {
                blobs.Add(blobItem.Name);
            }
            
            _logger.LogInformation($"Listed {blobs.Count} blobs in container");
            return blobs;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error listing blobs");
            throw;
        }
    }
}
