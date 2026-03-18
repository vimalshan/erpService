using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace UserManagement.Infrastructure.BlobStorage;

public interface IBlobStorageService
{
    Task<string> UploadAsync(string containerName, string blobName, Stream content, string contentType, CancellationToken cancellationToken = default);
    Task<Stream?> DownloadAsync(string containerName, string blobName, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string containerName, string blobName, CancellationToken cancellationToken = default);
    Task<string> GetBlobUrlAsync(string containerName, string blobName);
}

public class AzureBlobStorageService(
    IConfiguration configuration,
    ILogger<AzureBlobStorageService> logger) : IBlobStorageService
{
    private readonly string _connectionString = configuration["Azure:BlobStorage:ConnectionString"]
        ?? throw new InvalidOperationException("Azure Blob Storage connection string not configured.");

    public async Task<string> UploadAsync(string containerName, string blobName, Stream content, string contentType, CancellationToken cancellationToken = default)
    {
        var containerClient = new BlobContainerClient(_connectionString, containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: cancellationToken);

        var blobClient = containerClient.GetBlobClient(blobName);
        await blobClient.UploadAsync(content, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: cancellationToken);

        logger.LogInformation("Uploaded blob '{BlobName}' to container '{Container}'", blobName, containerName);
        return blobClient.Uri.ToString();
    }

    public async Task<Stream?> DownloadAsync(string containerName, string blobName, CancellationToken cancellationToken = default)
    {
        var blobClient = new BlobContainerClient(_connectionString, containerName).GetBlobClient(blobName);

        if (!await blobClient.ExistsAsync(cancellationToken))
            return null;

        var response = await blobClient.DownloadStreamingAsync(cancellationToken: cancellationToken);
        return response.Value.Content;
    }

    public async Task<bool> DeleteAsync(string containerName, string blobName, CancellationToken cancellationToken = default)
    {
        var blobClient = new BlobContainerClient(_connectionString, containerName).GetBlobClient(blobName);
        var result = await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
        return result.Value;
    }

    public Task<string> GetBlobUrlAsync(string containerName, string blobName)
    {
        var blobClient = new BlobContainerClient(_connectionString, containerName).GetBlobClient(blobName);
        return Task.FromResult(blobClient.Uri.ToString());
    }
}
