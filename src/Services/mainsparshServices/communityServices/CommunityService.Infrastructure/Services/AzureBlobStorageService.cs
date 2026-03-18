namespace CommunityService.Infrastructure.Services;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

public interface IBlobStorageService
{
    Task<string> UploadAsync(Stream fileStream, string fileName, string containerName);
    Task<Stream> DownloadAsync(string fileName, string containerName);
    Task DeleteAsync(string fileName, string containerName);
    Task<Uri> GetFileUriAsync(string fileName, string containerName);
}

public class AzureBlobStorageService : IBlobStorageService
{
    private readonly BlobContainerClient _containerClient;
    private readonly string _connectionString;

    public AzureBlobStorageService(string connectionString, string containerName)
    {
        _connectionString = connectionString;
        var blobServiceClient = new BlobServiceClient(connectionString);
        _containerClient = blobServiceClient.GetBlobContainerClient(containerName);
    }

    public async Task<string> UploadAsync(Stream fileStream, string fileName, string containerName)
    {
        try
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);

            var blobClient = blobContainerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(fileStream, overwrite: true);

            return blobClient.Uri.ToString();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to upload file {fileName}: {ex.Message}", ex);
        }
    }

    public async Task<Stream> DownloadAsync(string fileName, string containerName)
    {
        try
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = blobContainerClient.GetBlobClient(fileName);

            var download = await blobClient.DownloadAsync();
            return download.Value.Content;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to download file {fileName}: {ex.Message}", ex);
        }
    }

    public async Task DeleteAsync(string fileName, string containerName)
    {
        try
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = blobContainerClient.GetBlobClient(fileName);

            await blobClient.DeleteAsync(DeleteSnapshotsOption.IncludeSnapshots);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to delete file {fileName}: {ex.Message}", ex);
        }
    }

    public async Task<Uri> GetFileUriAsync(string fileName, string containerName)
    {
        try
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = blobContainerClient.GetBlobClient(fileName);

            return await Task.FromResult(blobClient.Uri);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to get file URI for {fileName}: {ex.Message}", ex);
        }
    }
}
