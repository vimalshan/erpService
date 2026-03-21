using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SalesOrderService.Infrastructure.Storage;

/// <summary>
/// Azure Blob Storage service — used for storing order-related documents and item images.
/// </summary>
public sealed class BlobStorageService(
    BlobServiceClient blobServiceClient,
    IConfiguration config,
    ILogger<BlobStorageService> logger)
{
    private readonly string _containerName =
        config["BlobStorage:ContainerName"] ?? "salesorder-documents";

    public async Task<string> UploadAsync(
        string fileName, Stream content, string contentType,
        CancellationToken ct = default)
    {
        var container = blobServiceClient.GetBlobContainerClient(_containerName);
        await container.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: ct);

        var blobName = $"{Guid.NewGuid():N}/{fileName}";
        var blobClient = container.GetBlobClient(blobName);

        await blobClient.UploadAsync(content,
            new BlobHttpHeaders { ContentType = contentType }, cancellationToken: ct);

        logger.LogInformation("Uploaded blob {BlobName} to {Container}", blobName, _containerName);
        return blobClient.Uri.ToString();
    }

    public async Task<Stream?> DownloadAsync(string blobName, CancellationToken ct = default)
    {
        var container = blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = container.GetBlobClient(blobName);

        if (!await blobClient.ExistsAsync(ct)) return null;

        var response = await blobClient.DownloadAsync(ct);
        return response.Value.Content;
    }

    public async Task DeleteAsync(string blobName, CancellationToken ct = default)
    {
        var container = blobServiceClient.GetBlobContainerClient(_containerName);
        await container.GetBlobClient(blobName).DeleteIfExistsAsync(cancellationToken: ct);
    }
}
