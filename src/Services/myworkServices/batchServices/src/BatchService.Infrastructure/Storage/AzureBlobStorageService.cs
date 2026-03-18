using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BatchService.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BatchService.Infrastructure.Storage;

public sealed class AzureBlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _serviceClient;
    private readonly ILogger<AzureBlobStorageService> _logger;

    public AzureBlobStorageService(IConfiguration config, ILogger<AzureBlobStorageService> logger)
    {
        var connStr = config["AzureStorage:ConnectionString"]
                      ?? throw new InvalidOperationException("AzureStorage:ConnectionString not configured.");
        _serviceClient = new BlobServiceClient(connStr);
        _logger        = logger;
    }

    public async Task<string> UploadAsync(string containerName, string blobName, Stream content, string contentType, CancellationToken ct = default)
    {
        var container = _serviceClient.GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: ct);

        var blob = container.GetBlobClient(blobName);
        await blob.UploadAsync(content, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: ct);

        _logger.LogInformation("[Blob] Uploaded {Blob} to {Container}", blobName, containerName);
        return blob.Uri.ToString();
    }

    public async Task<Stream?> DownloadAsync(string containerName, string blobName, CancellationToken ct = default)
    {
        var blob = _serviceClient.GetBlobContainerClient(containerName).GetBlobClient(blobName);
        if (!await blob.ExistsAsync(ct)) return null;

        var result = await blob.DownloadStreamingAsync(cancellationToken: ct);
        return result.Value.Content;
    }

    public async Task DeleteAsync(string containerName, string blobName, CancellationToken ct = default)
    {
        await _serviceClient.GetBlobContainerClient(containerName)
                            .GetBlobClient(blobName)
                            .DeleteIfExistsAsync(cancellationToken: ct);
    }

    public async Task<IEnumerable<string>> ListAsync(string containerName, CancellationToken ct = default)
    {
        var names = new List<string>();
        var container = _serviceClient.GetBlobContainerClient(containerName);

        await foreach (var item in container.GetBlobsAsync(cancellationToken: ct))
            names.Add(item.Name);

        return names;
    }
}
