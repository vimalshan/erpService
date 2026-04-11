using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

namespace TransactionService.Infrastructure.Services;

public sealed class BlobStorageService
{
    private readonly BlobServiceClient _client;
    private const string ContainerName = "transaction-attachments";

    public BlobStorageService(IConfiguration configuration)
    {
        var connectionString = configuration["AzureBlobStorage:ConnectionString"]
            ?? throw new InvalidOperationException("AzureBlobStorage:ConnectionString is not configured.");
        _client = new BlobServiceClient(connectionString);
    }

    public async Task<string> UploadAsync(
        string folder, Stream fileStream, string contentType, CancellationToken ct = default)
    {
        var container = _client.GetBlobContainerClient(ContainerName);
        await container.CreateIfNotExistsAsync(PublicAccessType.Blob, cancellationToken: ct);

        var blobName = $"{folder}/{Guid.NewGuid()}";
        var blob = container.GetBlobClient(blobName);
        await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: ct);

        return blob.Uri.ToString();
    }

    public async Task<bool> DeleteAsync(string fileUrl, CancellationToken ct = default)
    {
        var uri = new Uri(fileUrl);
        var blobName = string.Join("/", uri.Segments[2..]);
        var container = _client.GetBlobContainerClient(ContainerName);
        var blob = container.GetBlobClient(blobName);
        return await blob.DeleteIfExistsAsync(cancellationToken: ct);
    }
}
