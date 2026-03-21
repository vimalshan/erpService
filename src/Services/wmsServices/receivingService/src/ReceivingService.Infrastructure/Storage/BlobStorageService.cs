using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ReceivingService.Infrastructure.Storage;

public sealed class BlobStorageService
{
    private readonly BlobContainerClient _container;
    private readonly ILogger<BlobStorageService> _logger;

    public BlobStorageService(
        IConfiguration configuration,
        ILogger<BlobStorageService> logger)
    {
        _logger = logger;
        var connStr       = configuration["AzureStorage:ConnectionString"]
            ?? throw new InvalidOperationException("AzureStorage:ConnectionString not configured.");
        var containerName = configuration["AzureStorage:ContainerName"] ?? "stationery-images";
        _container = new BlobContainerClient(connStr, containerName);
    }

    /// <summary>Upload an image and return its public/SAS URI.</summary>
    public async Task<Uri> UploadImageAsync(
        string blobName, Stream content, string contentType,
        CancellationToken ct = default)
    {
        await _container.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: ct);
        var blob = _container.GetBlobClient(blobName);
        var opts = new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders { ContentType = contentType }
        };
        await blob.UploadAsync(content, opts, ct);
        _logger.LogInformation("Uploaded blob {BlobName}", blobName);
        return blob.Uri;
    }

    /// <summary>Download an image by blob name.</summary>
    public async Task<Stream> DownloadImageAsync(string blobName, CancellationToken ct = default)
    {
        var blob     = _container.GetBlobClient(blobName);
        var download = await blob.DownloadStreamingAsync(cancellationToken: ct);
        return download.Value.Content;
    }

    /// <summary>Delete an image by blob name.</summary>
    public async Task DeleteImageAsync(string blobName, CancellationToken ct = default)
    {
        var blob = _container.GetBlobClient(blobName);
        await blob.DeleteIfExistsAsync(cancellationToken: ct);
        _logger.LogInformation("Deleted blob {BlobName}", blobName);
    }
}
