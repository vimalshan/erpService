using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;

namespace AuthorizationService.Shared.Utilities;

public interface IBlobStorageService
{
    Task<string> UploadBlobAsync(string containerName, string blobName, Stream stream);
    Task<Stream> DownloadBlobAsync(string containerName, string blobName);
    Task DeleteBlobAsync(string containerName, string blobName);
    Task<List<string>> ListBlobsAsync(string containerName);
}

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;

    public BlobStorageService(IConfiguration configuration)
    {
        var connectionString = configuration["AzureStorage:BlobConnectionString"]
            ?? throw new InvalidOperationException("Blob Storage connection string not configured");
        _blobServiceClient = new BlobServiceClient(connectionString);
    }

    public async Task<string> UploadBlobAsync(string containerName, string blobName, Stream stream)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync();

        var blobClient = containerClient.GetBlobClient(blobName);
        await blobClient.UploadAsync(stream, overwrite: true);

        return blobClient.Uri.ToString();
    }

    public async Task<Stream> DownloadBlobAsync(string containerName, string blobName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(blobName);

        var download = await blobClient.DownloadAsync();
        return download.Value.Content;
    }

    public async Task DeleteBlobAsync(string containerName, string blobName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(blobName);

        await blobClient.DeleteIfExistsAsync();
    }

    public async Task<List<string>> ListBlobsAsync(string containerName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobs = new List<string>();

        await foreach (var blobItem in containerClient.GetBlobsAsync())
        {
            blobs.Add(blobItem.Name);
        }

        return blobs;
    }
}
