using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using InventoryManagement.Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace InventoryManagement.Infrastructure.Storage;

public sealed class AzureBlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _client;

    public AzureBlobStorageService(IOptions<BlobStorageOptions> options)
        => _client = new BlobServiceClient(options.Value.ConnectionString);

    public async Task<string> UploadAsync(
        string containerName, string blobName, Stream content, string contentType, CancellationToken ct = default)
    {
        var container = _client.GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: ct);

        var blob = container.GetBlobClient(blobName);
        await blob.UploadAsync(content, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: ct);
        return blob.Uri.ToString();
    }

    public async Task<Stream?> DownloadAsync(string containerName, string blobName, CancellationToken ct = default)
    {
        var blob = _client.GetBlobContainerClient(containerName).GetBlobClient(blobName);
        if (!await blob.ExistsAsync(ct)) return null;
        var response = await blob.DownloadAsync(ct);
        return response.Value.Content;
    }

    public async Task DeleteAsync(string containerName, string blobName, CancellationToken ct = default)
    {
        var blob = _client.GetBlobContainerClient(containerName).GetBlobClient(blobName);
        await blob.DeleteIfExistsAsync(cancellationToken: ct);
    }

    public Task<string> GetBlobUrlAsync(string containerName, string blobName, CancellationToken ct = default)
    {
        var blob = _client.GetBlobContainerClient(containerName).GetBlobClient(blobName);
        return Task.FromResult(blob.Uri.ToString());
    }
}
