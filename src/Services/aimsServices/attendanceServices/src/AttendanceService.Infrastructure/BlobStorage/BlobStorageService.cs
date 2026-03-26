using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AttendanceService.Infrastructure.BlobStorage;

public interface IBlobStorageService
{
    Task<string> UploadAsync(Stream content, string fileName, string contentType, CancellationToken ct = default);
    Task<Stream> DownloadAsync(string blobName, CancellationToken ct = default);
    Task DeleteAsync(string blobName, CancellationToken ct = default);
}

public class BlobStorageService(IConfiguration configuration, ILogger<BlobStorageService> logger)
    : IBlobStorageService
{
    private readonly BlobContainerClient? _containerClient = CreateClient(configuration, logger);

    private static BlobContainerClient? CreateClient(IConfiguration configuration, ILogger logger)
    {
        var connectionString = configuration["AzureBlob:ConnectionString"];
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            logger.LogWarning("AzureBlob:ConnectionString is not configured. Blob storage will be unavailable.");
            return null;
        }
        try
        {
            return new BlobContainerClient(
                connectionString,
                configuration["AzureBlob:Container"] ?? "attendance-images");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to initialize BlobContainerClient. Blob storage will be unavailable.");
            return null;
        }
    }

    public async Task<string> UploadAsync(Stream content, string fileName, string contentType, CancellationToken ct = default)
    {
        if (_containerClient is null)
            throw new InvalidOperationException("Azure Blob Storage is not configured.");

        await _containerClient.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: ct);
        var blobClient = _containerClient.GetBlobClient(fileName);

        await blobClient.UploadAsync(content, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: ct);
        logger.LogInformation("Uploaded blob: {FileName}", fileName);

        return blobClient.Uri.ToString();
    }

    public async Task<Stream> DownloadAsync(string blobName, CancellationToken ct = default)
    {
        if (_containerClient is null)
            throw new InvalidOperationException("Azure Blob Storage is not configured.");

        var blobClient = _containerClient.GetBlobClient(blobName);
        var response = await blobClient.DownloadStreamingAsync(cancellationToken: ct);
        return response.Value.Content;
    }

    public async Task DeleteAsync(string blobName, CancellationToken ct = default)
    {
        if (_containerClient is null)
            throw new InvalidOperationException("Azure Blob Storage is not configured.");

        var blobClient = _containerClient.GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync(cancellationToken: ct);
        logger.LogInformation("Deleted blob: {BlobName}", blobName);
    }
}
