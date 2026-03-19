namespace OrderScheduleService.API.Services;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

public interface IBlobStorageService
{
    Task<string> UploadAsync(string containerName, string blobName, Stream content);
    Task<Stream?> DownloadAsync(string containerName, string blobName);
    Task<bool> DeleteAsync(string containerName, string blobName);
    Task<IEnumerable<string>> ListBlobsAsync(string containerName);
}

public class AzureBlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly ILogger<AzureBlobStorageService> _logger;

    public AzureBlobStorageService(BlobServiceClient blobServiceClient, ILogger<AzureBlobStorageService> logger)
    {
        _blobServiceClient = blobServiceClient;
        _logger = logger;
    }

    public async Task<string> UploadAsync(string containerName, string blobName, Stream content)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(content, overwrite: true);

            _logger.LogInformation($"Blob {blobName} uploaded to container {containerName}");
            return blobClient.Uri.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error uploading blob {blobName}");
            throw;
        }
    }

    public async Task<Stream?> DownloadAsync(string containerName, string blobName)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            var download = await blobClient.DownloadAsync();
            var stream = new MemoryStream();
            await download.Value.Content.CopyToAsync(stream);
            stream.Position = 0;

            _logger.LogInformation($"Blob {blobName} downloaded from container {containerName}");
            return stream;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error downloading blob {blobName}");
            return null;
        }
    }

    public async Task<bool> DeleteAsync(string containerName, string blobName)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            await blobClient.DeleteAsync();

            _logger.LogInformation($"Blob {blobName} deleted from container {containerName}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting blob {blobName}");
            return false;
        }
    }

    public async Task<IEnumerable<string>> ListBlobsAsync(string containerName)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobs = new List<string>();

            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
            {
                blobs.Add(blobItem.Name);
            }

            return blobs;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error listing blobs in container {containerName}");
            return Enumerable.Empty<string>();
        }
    }
}
