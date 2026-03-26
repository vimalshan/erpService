using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BusServices.Infrastructure.Storage;

public interface IBlobStorageService
{
    Task<string> UploadAsync(string containerName, string blobName, Stream content, string contentType, CancellationToken ct = default);
    Task<Stream?> DownloadAsync(string containerName, string blobName, CancellationToken ct = default);
    Task DeleteAsync(string containerName, string blobName, CancellationToken ct = default);
    Task<IEnumerable<string>> ListAsync(string containerName, CancellationToken ct = default);
}

public sealed class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient? _client;
    private readonly ILogger<BlobStorageService> _logger;

    public BlobStorageService(IConfiguration config, ILogger<BlobStorageService> logger)
    {
        _logger = logger;
        var connectionString = config.GetConnectionString("AzureBlobStorage");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            _logger.LogWarning("AzureBlobStorage connection string is not configured. Blob storage will be unavailable.");
            return;
        }
        try
        {
            _client = new BlobServiceClient(connectionString);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize BlobServiceClient. Blob storage will be unavailable.");
        }
    }

    public async Task<string> UploadAsync(string containerName, string blobName, Stream content, string contentType, CancellationToken ct = default)
    {
        EnsureAvailable();
        var container = _client!.GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: ct);

        var blob = container.GetBlobClient(blobName);
        await blob.UploadAsync(content, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: ct);

        _logger.LogInformation("Uploaded blob: {Container}/{BlobName}", containerName, blobName);
        return blob.Uri.ToString();
    }

    public async Task<Stream?> DownloadAsync(string containerName, string blobName, CancellationToken ct = default)
    {
        EnsureAvailable();
        var blob = _client!.GetBlobContainerClient(containerName).GetBlobClient(blobName);
        if (!await blob.ExistsAsync(ct)) return null;

        var result = await blob.DownloadAsync(ct);
        return result.Value.Content;
    }

    public async Task DeleteAsync(string containerName, string blobName, CancellationToken ct = default)
    {
        EnsureAvailable();
        var blob = _client!.GetBlobContainerClient(containerName).GetBlobClient(blobName);
        await blob.DeleteIfExistsAsync(cancellationToken: ct);
        _logger.LogInformation("Deleted blob: {Container}/{BlobName}", containerName, blobName);
    }

    public async Task<IEnumerable<string>> ListAsync(string containerName, CancellationToken ct = default)
    {
        EnsureAvailable();
        var container = _client!.GetBlobContainerClient(containerName);
        var blobs = new List<string>();
        await foreach (var item in container.GetBlobsAsync(cancellationToken: ct))
            blobs.Add(item.Name);
        return blobs;
    }

    private void EnsureAvailable()
    {
        if (_client is null)
            throw new InvalidOperationException("Azure Blob Storage is not configured.");
    }
}
