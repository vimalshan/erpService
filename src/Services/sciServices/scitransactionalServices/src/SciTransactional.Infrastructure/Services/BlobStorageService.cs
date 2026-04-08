using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using SciTransactional.Application.Interfaces;

namespace SciTransactional.Infrastructure.Services;

public sealed class BlobStorageService(BlobServiceClient blobServiceClient) : IBlobStorageService
{
    public async Task<string> UploadAsync(string containerName, string blobName,
        Stream content, string contentType, CancellationToken ct = default)
    {
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync(cancellationToken: ct);
        var blobClient = containerClient.GetBlobClient(blobName);
        await blobClient.UploadAsync(content,
            new BlobHttpHeaders { ContentType = contentType }, cancellationToken: ct);
        return blobClient.Uri.ToString();
    }

    public async Task<Stream?> DownloadAsync(string containerName, string blobName,
        CancellationToken ct = default)
    {
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(blobName);
        if (!await blobClient.ExistsAsync(ct)) return null;
        var response = await blobClient.DownloadStreamingAsync(cancellationToken: ct);
        return response.Value.Content;
    }

    public async Task<bool> DeleteAsync(string containerName, string blobName,
        CancellationToken ct = default)
    {
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(blobName);
        var response = await blobClient.DeleteIfExistsAsync(cancellationToken: ct);
        return response.Value;
    }
}

public sealed class NoOpBlobStorageService : IBlobStorageService
{
    public Task<string> UploadAsync(string containerName, string blobName, Stream content,
        string contentType, CancellationToken ct = default)
        => Task.FromResult($"noop://{containerName}/{blobName}");

    public Task<Stream?> DownloadAsync(string containerName, string blobName,
        CancellationToken ct = default)
        => Task.FromResult<Stream?>(Stream.Null);

    public Task<bool> DeleteAsync(string containerName, string blobName,
        CancellationToken ct = default)
        => Task.FromResult(false);
}
