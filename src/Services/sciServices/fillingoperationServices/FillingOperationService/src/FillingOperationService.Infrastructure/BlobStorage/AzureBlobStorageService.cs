using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FillingOperationService.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FillingOperationService.Infrastructure.BlobStorage;

public class AzureBlobStorageService(IConfiguration configuration, ILogger<AzureBlobStorageService> logger) : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient = new(
        configuration["AzureBlobStorage:ConnectionString"]
        ?? throw new InvalidOperationException("AzureBlobStorage:ConnectionString not configured."));

    public async Task<string> UploadImageAsync(string containerName, string fileName, Stream content, string contentType, CancellationToken cancellationToken = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: cancellationToken);

        var blobClient = containerClient.GetBlobClient(fileName);
        var headers = new BlobHttpHeaders { ContentType = contentType };
        await blobClient.UploadAsync(content, new BlobUploadOptions { HttpHeaders = headers }, cancellationToken);

        logger.LogInformation("Uploaded blob {FileName} to container {Container}", fileName, containerName);
        return blobClient.Uri.ToString();
    }

    public async Task<Stream?> DownloadImageAsync(string containerName, string fileName, CancellationToken cancellationToken = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(fileName);

        if (!await blobClient.ExistsAsync(cancellationToken))
            return null;

        var download = await blobClient.DownloadStreamingAsync(cancellationToken: cancellationToken);
        return download.Value.Content;
    }

    public async Task DeleteImageAsync(string containerName, string fileName, CancellationToken cancellationToken = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
        logger.LogInformation("Deleted blob {FileName} from container {Container}", fileName, containerName);
    }

    public async Task<bool> ExistsAsync(string containerName, string fileName, CancellationToken cancellationToken = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(fileName);
        return await blobClient.ExistsAsync(cancellationToken);
    }
}
