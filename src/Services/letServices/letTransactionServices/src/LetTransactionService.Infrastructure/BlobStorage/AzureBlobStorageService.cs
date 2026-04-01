using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using LetTransactionService.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LetTransactionService.Infrastructure.BlobStorage;

public class AzureBlobStorageService(
    IConfiguration configuration,
    ILogger<AzureBlobStorageService> logger)
    : IBlobStorageService
{
    private BlobServiceClient CreateClient()
    {
        var connectionString = configuration.GetConnectionString("AzureBlobStorage")
            ?? "UseDevelopmentStorage=true";
        return new BlobServiceClient(connectionString);
    }

    public async Task<string> UploadAsync(
        string containerName, string blobName,
        Stream content, string contentType, CancellationToken ct = default)
    {
        var client = CreateClient();
        var container = client.GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: ct);

        var blobClient = container.GetBlobClient(blobName);
        await blobClient.UploadAsync(content, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: ct);

        logger.LogInformation("Uploaded blob {BlobName} to container {Container}", blobName, containerName);
        return blobClient.Uri.ToString();
    }

    public async Task<Stream?> DownloadAsync(
        string containerName, string blobName, CancellationToken ct = default)
    {
        var client = CreateClient();
        var blobClient = client.GetBlobContainerClient(containerName).GetBlobClient(blobName);

        if (!await blobClient.ExistsAsync(ct)) return null;

        var response = await blobClient.DownloadStreamingAsync(cancellationToken: ct);
        return response.Value.Content;
    }

    public async Task DeleteAsync(string containerName, string blobName, CancellationToken ct = default)
    {
        var client = CreateClient();
        var blobClient = client.GetBlobContainerClient(containerName).GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync(cancellationToken: ct);
        logger.LogInformation("Deleted blob {BlobName} from container {Container}", blobName, containerName);
    }

    public async Task<bool> ExistsAsync(string containerName, string blobName, CancellationToken ct = default)
    {
        var client = CreateClient();
        var blobClient = client.GetBlobContainerClient(containerName).GetBlobClient(blobName);
        return await blobClient.ExistsAsync(ct);
    }

    public string GetBlobUrl(string containerName, string blobName)
    {
        var client = CreateClient();
        var blobClient = client.GetBlobContainerClient(containerName).GetBlobClient(blobName);
        return blobClient.Uri.ToString();
    }
}
