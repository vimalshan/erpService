using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GroupIncentiveService.Infrastructure.BlobStorage;

public interface IBlobStorageService
{
    Task<string> UploadAsync(string containerName, string blobName, Stream content, string contentType, CancellationToken ct = default);
    Task<Stream?> DownloadAsync(string containerName, string blobName, CancellationToken ct = default);
    Task DeleteAsync(string containerName, string blobName, CancellationToken ct = default);
    Task<bool> ExistsAsync(string containerName, string blobName, CancellationToken ct = default);
}

public class AzureBlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient? _blobServiceClient;
    private readonly ILogger<AzureBlobStorageService> _logger;

    public AzureBlobStorageService(IConfiguration configuration, ILogger<AzureBlobStorageService> logger)
    {
        _logger = logger;
        var connectionString = configuration.GetConnectionString("AzureBlobStorage");
        if (!string.IsNullOrWhiteSpace(connectionString))
            _blobServiceClient = new BlobServiceClient(connectionString);
        else
            _logger.LogWarning("AzureBlobStorage connection string is not configured. Blob operations will be unavailable.");
    }

    private BlobServiceClient GetClient() =>
        _blobServiceClient ?? throw new InvalidOperationException("AzureBlobStorage is not configured.");

    public async Task<string> UploadAsync(string containerName, string blobName, Stream content,
        string contentType, CancellationToken ct = default)
    {
        var container = GetClient().GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: ct);

        var blobClient = container.GetBlobClient(blobName);
        await blobClient.UploadAsync(content, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: ct);

        _logger.LogInformation("Uploaded blob {BlobName} to container {Container}", blobName, containerName);
        return blobClient.Uri.ToString();
    }

    public async Task<Stream?> DownloadAsync(string containerName, string blobName, CancellationToken ct = default)
    {
        var blobClient = GetClient().GetBlobContainerClient(containerName).GetBlobClient(blobName);
        if (!await blobClient.ExistsAsync(ct)) return null;

        var response = await blobClient.DownloadStreamingAsync(cancellationToken: ct);
        return response.Value.Content;
    }

    public async Task DeleteAsync(string containerName, string blobName, CancellationToken ct = default)
    {
        var blobClient = GetClient().GetBlobContainerClient(containerName).GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync(cancellationToken: ct);
        _logger.LogInformation("Deleted blob {BlobName} from container {Container}", blobName, containerName);
    }

    public async Task<bool> ExistsAsync(string containerName, string blobName, CancellationToken ct = default)
    {
        var blobClient = GetClient().GetBlobContainerClient(containerName).GetBlobClient(blobName);
        return await blobClient.ExistsAsync(ct);
    }
}
