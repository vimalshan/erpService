using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LeaveServices.Infrastructure.Storage;

/// <summary>
/// Azure Blob Storage service – used to store attachments (e.g. medical certificates).
/// </summary>
public sealed class BlobStorageService
{
    private readonly BlobServiceClient? _client;
    private readonly ILogger<BlobStorageService> _logger;
    private const string ContainerName = "leave-attachments";

    public BlobStorageService(IConfiguration configuration, ILogger<BlobStorageService> logger)
    {
        _logger = logger;
        var connectionString = configuration["AzureStorage:ConnectionString"];
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            _logger.LogWarning("AzureStorage:ConnectionString is not configured. Blob storage will be unavailable.");
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
            throw new InvalidOperationException("Blob storage is not available: AzureStorage:ConnectionString is missing or invalid.");
    }

    public async Task<string> UploadAsync(string fileName, Stream content, string contentType, CancellationToken ct = default)
    {
        EnsureAvailable();
        var container = _client!.GetBlobContainerClient(ContainerName);
        await container.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: ct);

        var blobName = $"{Guid.NewGuid():N}_{Path.GetFileName(fileName)}";
        var blob     = container.GetBlobClient(blobName);
        await blob.UploadAsync(content, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: ct);

        return blob.Uri.ToString();
    }

    public async Task DeleteAsync(string blobUri, CancellationToken ct = default)
    {
        EnsureAvailable();
        var uri  = new Uri(blobUri);
        var name = uri.Segments.Last();
        var container = _client!.GetBlobContainerClient(ContainerName);
        await container.GetBlobClient(name).DeleteIfExistsAsync(cancellationToken: ct);
    }
}
