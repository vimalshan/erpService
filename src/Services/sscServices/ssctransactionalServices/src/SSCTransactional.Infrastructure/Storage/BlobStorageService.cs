using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SSCTransactional.Infrastructure.Storage;

public class BlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly ILogger<BlobStorageService> _logger;
    private const string ContainerName = "transaction-attachments";

    public BlobStorageService(IConfiguration configuration, ILogger<BlobStorageService> logger)
    {
        _logger = logger;
        var connectionString = configuration.GetConnectionString("AzureBlobStorage")
            ?? throw new InvalidOperationException("AzureBlobStorage connection string not configured.");
        _blobServiceClient = new BlobServiceClient(connectionString);
    }

    public async Task<string> UploadFileAsync(string fileName, Stream fileStream, string contentType, CancellationToken ct = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob, cancellationToken: ct);

        var safeName = $"{Guid.NewGuid():N}_{Path.GetFileName(fileName)}";
        var blobClient = containerClient.GetBlobClient(safeName);

        await blobClient.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: ct);
        _logger.LogInformation("[Blob] Uploaded {FileName} to {BlobUri}", fileName, blobClient.Uri);
        return blobClient.Uri.ToString();
    }

    public async Task<Stream> DownloadFileAsync(string blobName, CancellationToken ct = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
        var blobClient = containerClient.GetBlobClient(blobName);
        var response = await blobClient.DownloadAsync(ct);
        return response.Value.Content;
    }

    public async Task DeleteFileAsync(string blobName, CancellationToken ct = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
        var blobClient = containerClient.GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync(cancellationToken: ct);
        _logger.LogInformation("[Blob] Deleted {BlobName}", blobName);
    }
}
