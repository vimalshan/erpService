using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ObjectiveService.Infrastructure.Services;

public interface IBlobStorageService
{
    Task<string> UploadFileAsync(string containerName, string fileName, Stream fileStream, CancellationToken cancellationToken = default);
    Task<Stream> DownloadFileAsync(string containerName, string fileName, CancellationToken cancellationToken = default);
    Task DeleteFileAsync(string containerName, string fileName, CancellationToken cancellationToken = default);
    Task<bool> FileExistsAsync(string containerName, string fileName, CancellationToken cancellationToken = default);
}

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobContainerClient _containerClient;
    private readonly ILogger<BlobStorageService> _logger;

    public BlobStorageService(IConfiguration configuration, ILogger<BlobStorageService> logger)
    {
        _logger = logger;
        var connectionString = configuration["AzureBlobStorage:ConnectionString"];
        var containerName = configuration["AzureBlobStorage:ContainerName"];

        var blobServiceClient = new BlobServiceClient(connectionString);
        _containerClient = blobServiceClient.GetBlobContainerClient(containerName);
    }

    public async Task<string> UploadFileAsync(string containerName, string fileName, Stream fileStream, CancellationToken cancellationToken = default)
    {
        try
        {
            var blobClient = _containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(fileStream, overwrite: true, cancellationToken);
            
            _logger.LogInformation("File {FileName} uploaded successfully to container {ContainerName}", fileName, containerName);
            return blobClient.Uri.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file {FileName} to blob storage", fileName);
            throw;
        }
    }

    public async Task<Stream> DownloadFileAsync(string containerName, string fileName, CancellationToken cancellationToken = default)
    {
        try
        {
            var blobClient = _containerClient.GetBlobClient(fileName);
            var download = await blobClient.DownloadAsync(cancellationToken);
            
            _logger.LogInformation("File {FileName} downloaded from container {ContainerName}", fileName, containerName);
            return download.Value.Content;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading file {FileName} from blob storage", fileName);
            throw;
        }
    }

    public async Task DeleteFileAsync(string containerName, string fileName, CancellationToken cancellationToken = default)
    {
        try
        {
            var blobClient = _containerClient.GetBlobClient(fileName);
            await blobClient.DeleteAsync(cancellationToken: cancellationToken);
            
            _logger.LogInformation("File {FileName} deleted from container {ContainerName}", fileName, containerName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file {FileName} from blob storage", fileName);
            throw;
        }
    }

    public async Task<bool> FileExistsAsync(string containerName, string fileName, CancellationToken cancellationToken = default)
    {
        try
        {
            var blobClient = _containerClient.GetBlobClient(fileName);
            var exists = await blobClient.ExistsAsync(cancellationToken);
            return exists.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking file existence {FileName}", fileName);
            return false;
        }
    }
}
