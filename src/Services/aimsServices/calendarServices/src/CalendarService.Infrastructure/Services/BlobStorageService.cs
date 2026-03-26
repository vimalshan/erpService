using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CalendarService.Infrastructure.Services;

public interface IBlobStorageService
{
    Task<string> UploadAsync(string containerName, string blobName, Stream content, string contentType, CancellationToken ct = default);
    Task<Stream?> DownloadAsync(string containerName, string blobName, CancellationToken ct = default);
    Task DeleteAsync(string containerName, string blobName, CancellationToken ct = default);
}

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient? _client;
    private readonly ILogger<BlobStorageService> _logger;

    public BlobStorageService(IConfiguration config, ILogger<BlobStorageService> logger)
    {
        _logger = logger;
        var connectionString = config["AzureBlobStorage:ConnectionString"];
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            _logger.LogWarning("AzureBlobStorage:ConnectionString is not configured. Blob storage will be unavailable.");
            return;
        }
        try
        {
            _client = new BlobServiceClient(connectionString);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialise BlobServiceClient. Blob storage will be unavailable.");
        }
    }

    private void EnsureAvailable()
    {
        if (_client is null)
            throw new InvalidOperationException("Blob storage is not available: AzureBlobStorage:ConnectionString is missing or invalid.");
    }

    public async Task<string> UploadAsync(string containerName, string blobName, Stream content, string contentType, CancellationToken ct = default)
    {
        EnsureAvailable();
        var container = _client!.GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync(PublicAccessType.None, null, null, ct);

        var blob = container.GetBlobClient(blobName);
        await blob.UploadAsync(content, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: ct);
        return blob.Uri.ToString();
    }

    public async Task<Stream?> DownloadAsync(string containerName, string blobName, CancellationToken ct = default)
    {
        EnsureAvailable();
        var blob = _client!.GetBlobContainerClient(containerName).GetBlobClient(blobName);
        if (!await blob.ExistsAsync(ct)) return null;
        var download = await blob.DownloadAsync(ct);
        return download.Value.Content;
    }

    public async Task DeleteAsync(string containerName, string blobName, CancellationToken ct = default)
    {
        EnsureAvailable();
        var blob = _client!.GetBlobContainerClient(containerName).GetBlobClient(blobName);
        await blob.DeleteIfExistsAsync(cancellationToken: ct);
    }
}
