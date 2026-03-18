using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;

namespace LovService.Infrastructure.Adapters;

/// <summary>
/// Adapter pattern: wraps Azure Blob Storage SDK into a domain-friendly interface.
/// </summary>
public class BlobStorageAdapter(BlobServiceClient blobServiceClient, ILogger<BlobStorageAdapter> logger)
{
    public async Task<string> UploadAsync(string containerName, string blobName, Stream content, string contentType = "application/json", CancellationToken ct = default)
    {
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: ct);

        var blobClient = containerClient.GetBlobClient(blobName);
        await blobClient.UploadAsync(content, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: ct);

        logger.LogInformation("Uploaded blob '{BlobName}' to container '{Container}'", blobName, containerName);
        return blobClient.Uri.ToString();
    }

    public async Task<Stream?> DownloadAsync(string containerName, string blobName, CancellationToken ct = default)
    {
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(blobName);

        if (!await blobClient.ExistsAsync(ct))
        {
            logger.LogWarning("Blob '{BlobName}' not found in container '{Container}'", blobName, containerName);
            return null;
        }

        var response = await blobClient.DownloadStreamingAsync(cancellationToken: ct);
        return response.Value.Content;
    }

    public async Task DeleteAsync(string containerName, string blobName, CancellationToken ct = default)
    {
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync(cancellationToken: ct);
        logger.LogInformation("Deleted blob '{BlobName}' from container '{Container}'", blobName, containerName);
    }

    public async Task<IEnumerable<string>> ListBlobsAsync(string containerName, CancellationToken ct = default)
    {
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        var blobs = new List<string>();
        await foreach (var blob in containerClient.GetBlobsAsync(cancellationToken: ct))
            blobs.Add(blob.Name);
        return blobs;
    }
}
