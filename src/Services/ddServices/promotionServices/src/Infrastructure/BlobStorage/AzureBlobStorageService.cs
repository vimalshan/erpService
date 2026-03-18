using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace PromotionService.Infrastructure.BlobStorage;

public interface IBlobStorageService
{
    Task<string> UploadAsync(Stream content, string fileName, string contentType, CancellationToken ct = default);
    Task<Stream> DownloadAsync(string blobName, CancellationToken ct = default);
    Task DeleteAsync(string blobName, CancellationToken ct = default);
    Task<bool> ExistsAsync(string blobName, CancellationToken ct = default);
    Task<IReadOnlyList<string>> ListBlobsAsync(string? prefix = null, CancellationToken ct = default);
}

public class AzureBlobStorageService : IBlobStorageService
{
    private readonly BlobContainerClient _containerClient;
    private readonly ILogger<AzureBlobStorageService> _logger;

    public AzureBlobStorageService(IConfiguration configuration, ILogger<AzureBlobStorageService> logger)
    {
        _logger = logger;
        var connectionString = configuration["AzureBlobStorage:ConnectionString"]
            ?? throw new InvalidOperationException("AzureBlobStorage:ConnectionString is not configured.");
        var containerName = configuration["AzureBlobStorage:ContainerName"] ?? "promotion-documents";

        var serviceClient = new BlobServiceClient(connectionString);
        _containerClient = serviceClient.GetBlobContainerClient(containerName);
    }

    /// <summary>Upload a document and return the blob name (GUID-based to prevent path traversal).</summary>
    public async Task<string> UploadAsync(Stream content, string fileName, string contentType, CancellationToken ct = default)
    {
        // Use a GUID-prefixed name to prevent overwrite/path-traversal attacks
        var sanitizedName = System.IO.Path.GetFileName(fileName);
        var blobName = $"{Guid.NewGuid():N}/{sanitizedName}";

        await _containerClient.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: ct);

        var blobClient = _containerClient.GetBlobClient(blobName);
        var options = new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders { ContentType = contentType }
        };

        await blobClient.UploadAsync(content, options, ct);
        _logger.LogInformation("Uploaded blob {BlobName}", blobName);
        return blobName;
    }

    public async Task<Stream> DownloadAsync(string blobName, CancellationToken ct = default)
    {
        var blobClient = _containerClient.GetBlobClient(blobName);
        var response = await blobClient.DownloadStreamingAsync(cancellationToken: ct);
        _logger.LogInformation("Downloaded blob {BlobName}", blobName);
        return response.Value.Content;
    }

    public async Task DeleteAsync(string blobName, CancellationToken ct = default)
    {
        var blobClient = _containerClient.GetBlobClient(blobName);
        var deleted = await blobClient.DeleteIfExistsAsync(cancellationToken: ct);
        if (deleted)
            _logger.LogInformation("Deleted blob {BlobName}", blobName);
        else
            _logger.LogWarning("Blob {BlobName} not found for deletion.", blobName);
    }

    public async Task<bool> ExistsAsync(string blobName, CancellationToken ct = default)
    {
        var blobClient = _containerClient.GetBlobClient(blobName);
        var response = await blobClient.ExistsAsync(ct);
        return response.Value;
    }

    public async Task<IReadOnlyList<string>> ListBlobsAsync(string? prefix = null, CancellationToken ct = default)
    {
        var results = new List<string>();
        await foreach (var item in _containerClient.GetBlobsAsync(prefix: prefix, cancellationToken: ct))
            results.Add(item.Name);
        return results;
    }
}
