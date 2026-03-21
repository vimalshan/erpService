using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ConfigService.Application.Interfaces;

namespace ConfigService.Infrastructure.Services;

public class BlobStorageService(BlobServiceClient blobClient) : IBlobStorageService
{
    public async Task<string> UploadAsync(string containerName, string fileName, Stream content, string contentType, CancellationToken ct = default)
    {
        var container = blobClient.GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: ct);
        var blob = container.GetBlobClient(fileName);
        await blob.UploadAsync(content, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: ct);
        return blob.Uri.ToString();
    }

    public async Task<Stream?> DownloadAsync(string containerName, string fileName, CancellationToken ct = default)
    {
        var container = blobClient.GetBlobContainerClient(containerName);
        var blob = container.GetBlobClient(fileName);
        if (!await blob.ExistsAsync(ct)) return null;
        var response = await blob.DownloadAsync(ct);
        return response.Value.Content;
    }

    public async Task<bool> DeleteAsync(string containerName, string fileName, CancellationToken ct = default)
    {
        var container = blobClient.GetBlobContainerClient(containerName);
        var blob = container.GetBlobClient(fileName);
        var response = await blob.DeleteIfExistsAsync(cancellationToken: ct);
        return response.Value;
    }

    public Task<string> GetUrlAsync(string containerName, string fileName, CancellationToken ct = default)
    {
        var container = blobClient.GetBlobContainerClient(containerName);
        var blob = container.GetBlobClient(fileName);
        return Task.FromResult(blob.Uri.ToString());
    }
}
