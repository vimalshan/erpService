namespace ApprovalService.Infrastructure.External;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using ApprovalService.Application.Interfaces;
using Microsoft.Extensions.Logging;

/// <summary>
/// Azure Blob Storage Service
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

    public async Task<string> UploadAsync(string containerName, string fileName, Stream content)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();

            var blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(content, overwrite: true);

            _logger.LogInformation("Uploaded blob {FileName} to container {ContainerName}", fileName, containerName);
            return blobClient.Uri.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading blob {FileName} to container {ContainerName}", fileName, containerName);
            throw;
        }
    }

    public async Task<Stream> DownloadAsync(string containerName, string fileName)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            var download = await blobClient.DownloadAsync();
            _logger.LogInformation("Downloaded blob {FileName} from container {ContainerName}", fileName, containerName);
            return download.Value.Content;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading blob {FileName} from container {ContainerName}", fileName, containerName);
            throw;
        }
    }

    public async Task DeleteAsync(string containerName, string fileName)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.DeleteAsync();

            _logger.LogInformation("Deleted blob {FileName} from container {ContainerName}", fileName, containerName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting blob {FileName} from container {ContainerName}", fileName, containerName);
            throw;
        }
    }

    public async Task<string> GetSasUrlAsync(string containerName, string fileName, TimeSpan expiresIn)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            var sasBuilder = new BlobSasBuilder(BlobSasPermissions.Parse<BlobSasPermissions>("racwd"), DateTime.UtcNow.Add(expiresIn))
            {
                BlobContainerName = containerName,
                BlobName = fileName
            };

            var sasUri = blobClient.GenerateSasUri(sasBuilder);
            _logger.LogInformation("Generated SAS URL for blob {FileName}", fileName);
            return sasUri.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating SAS URL for blob {FileName}", fileName);
            throw;
        }
    }
}
