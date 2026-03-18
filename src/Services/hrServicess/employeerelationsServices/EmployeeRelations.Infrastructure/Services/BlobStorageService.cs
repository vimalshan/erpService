using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;

namespace EmployeeRelations.Infrastructure.Services;

public interface IBlobStorageService
{
    Task<string> UploadDocumentAsync(Stream content, string fileName, string containerName, CancellationToken ct = default);
    Task<Stream> DownloadDocumentAsync(string blobName, string containerName, CancellationToken ct = default);
    Task DeleteDocumentAsync(string blobName, string containerName, CancellationToken ct = default);
    Task<string> GetSasUrlAsync(string blobName, string containerName, TimeSpan expiry, CancellationToken ct = default);
}

public class AzureBlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly ILogger<AzureBlobStorageService> _logger;

    public AzureBlobStorageService(BlobServiceClient blobServiceClient, ILogger<AzureBlobStorageService> logger)
    {
        _blobServiceClient = blobServiceClient;
        _logger = logger;
    }

    public async Task<string> UploadDocumentAsync(Stream content, string fileName, string containerName, CancellationToken ct = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: ct);

        var blobName = $"{Guid.NewGuid():N}_{fileName}";
        var blobClient = containerClient.GetBlobClient(blobName);
        await blobClient.UploadAsync(content, overwrite: true, cancellationToken: ct);

        _logger.LogInformation("Uploaded blob '{BlobName}' to container '{Container}'", blobName, containerName);
        return blobName;
    }

    public async Task<Stream> DownloadDocumentAsync(string blobName, string containerName, CancellationToken ct = default)
    {
        var blobClient = _blobServiceClient.GetBlobContainerClient(containerName).GetBlobClient(blobName);
        var response = await blobClient.DownloadAsync(ct);
        return response.Value.Content;
    }

    public async Task DeleteDocumentAsync(string blobName, string containerName, CancellationToken ct = default)
    {
        var blobClient = _blobServiceClient.GetBlobContainerClient(containerName).GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync(cancellationToken: ct);
    }

    public Task<string> GetSasUrlAsync(string blobName, string containerName, TimeSpan expiry, CancellationToken ct = default)
    {
        var blobClient = _blobServiceClient.GetBlobContainerClient(containerName).GetBlobClient(blobName);
        var sasUri = blobClient.GenerateSasUri(Azure.Storage.Sas.BlobSasPermissions.Read, DateTimeOffset.UtcNow.Add(expiry));
        return Task.FromResult(sasUri.ToString());
    }
}
