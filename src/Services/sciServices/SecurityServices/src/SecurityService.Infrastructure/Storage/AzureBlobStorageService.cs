using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.Extensions.Logging;
using SecurityService.Application.Interfaces;

namespace SecurityService.Infrastructure.Storage;

public sealed class AzureBlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _client;
    private readonly ILogger<AzureBlobStorageService> _logger;

    public AzureBlobStorageService(BlobServiceClient client, ILogger<AzureBlobStorageService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<string> UploadAsync(string containerName, string blobName, Stream content, string contentType, CancellationToken cancellationToken = default)
    {
        var container = _client.GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: cancellationToken);
        var blob = container.GetBlobClient(blobName);
        await blob.UploadAsync(content, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: cancellationToken);
        _logger.LogInformation("Uploaded blob {BlobName} to container {Container}", blobName, containerName);
        return blob.Uri.ToString();
    }

    public async Task<Stream?> DownloadAsync(string containerName, string blobName, CancellationToken cancellationToken = default)
    {
        var blob = _client.GetBlobContainerClient(containerName).GetBlobClient(blobName);
        if (!await blob.ExistsAsync(cancellationToken)) return null;
        var download = await blob.DownloadStreamingAsync(cancellationToken: cancellationToken);
        return download.Value.Content;
    }

    public async Task DeleteAsync(string containerName, string blobName, CancellationToken cancellationToken = default)
    {
        var blob = _client.GetBlobContainerClient(containerName).GetBlobClient(blobName);
        await blob.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }

    public Task<string> GetSasUriAsync(string containerName, string blobName, TimeSpan expiry)
    {
        var blob = _client.GetBlobContainerClient(containerName).GetBlobClient(blobName);
        var sasBuilder = new BlobSasBuilder(BlobSasPermissions.Read, DateTimeOffset.UtcNow.Add(expiry))
        {
            BlobContainerName = containerName,
            BlobName = blobName,
            Resource = "b"
        };
        var uri = blob.GenerateSasUri(sasBuilder);
        return Task.FromResult(uri.ToString());
    }
}
