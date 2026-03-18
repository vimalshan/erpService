using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ScholarshipService.Infrastructure.Azure;

public interface IBlobStorageService
{
    Task<string> UploadAsync(string containerName, string blobName, Stream content, string contentType, CancellationToken cancellationToken = default);
    Task<Stream?> DownloadAsync(string containerName, string blobName, CancellationToken cancellationToken = default);
    Task DeleteAsync(string containerName, string blobName, CancellationToken cancellationToken = default);
    Task<string> GetBlobUrlAsync(string containerName, string blobName);
}

public class BlobStorageService(IConfiguration configuration, ILogger<BlobStorageService> logger)
    : IBlobStorageService
{
    private readonly string _connectionString = configuration["Azure:BlobStorage:ConnectionString"]
        ?? "UseDevelopmentStorage=true";

    public async Task<string> UploadAsync(string containerName, string blobName, Stream content,
        string contentType, CancellationToken cancellationToken = default)
    {
        var client = new BlobServiceClient(_connectionString);
        var container = client.GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: cancellationToken);

        var blob = container.GetBlobClient(blobName);
        await blob.UploadAsync(content, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: cancellationToken);
        logger.LogInformation("Uploaded blob {BlobName} to {Container}", blobName, containerName);
        return blob.Uri.ToString();
    }

    public async Task<Stream?> DownloadAsync(string containerName, string blobName, CancellationToken cancellationToken = default)
    {
        var client = new BlobServiceClient(_connectionString);
        var blob = client.GetBlobContainerClient(containerName).GetBlobClient(blobName);
        if (!await blob.ExistsAsync(cancellationToken)) return null;
        var response = await blob.DownloadAsync(cancellationToken);
        return response.Value.Content;
    }

    public async Task DeleteAsync(string containerName, string blobName, CancellationToken cancellationToken = default)
    {
        var client = new BlobServiceClient(_connectionString);
        var blob = client.GetBlobContainerClient(containerName).GetBlobClient(blobName);
        await blob.DeleteIfExistsAsync(cancellationToken: cancellationToken);
        logger.LogInformation("Deleted blob {BlobName} from {Container}", blobName, containerName);
    }

    public Task<string> GetBlobUrlAsync(string containerName, string blobName)
    {
        var client = new BlobServiceClient(_connectionString);
        var blob = client.GetBlobContainerClient(containerName).GetBlobClient(blobName);
        return Task.FromResult(blob.Uri.ToString());
    }
}
