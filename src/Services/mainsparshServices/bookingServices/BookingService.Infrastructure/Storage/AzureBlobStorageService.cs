using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;

namespace BookingService.Infrastructure.Storage;

public interface IBlobStorageService
{
    Task<string> UploadAsync(string containerName, string blobName, Stream content, string contentType, CancellationToken cancellationToken = default);
    Task<Stream> DownloadAsync(string containerName, string blobName, CancellationToken cancellationToken = default);
    Task DeleteAsync(string containerName, string blobName, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string containerName, string blobName, CancellationToken cancellationToken = default);
}

public class AzureBlobStorageService(IConfiguration configuration) : IBlobStorageService
{
    private BlobServiceClient CreateClient()
        => new(configuration["AzureStorage:ConnectionString"]);

    public async Task<string> UploadAsync(string containerName, string blobName, Stream content, string contentType, CancellationToken cancellationToken = default)
    {
        var client = CreateClient();
        var container = client.GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

        var blob = container.GetBlobClient(blobName);
        await blob.UploadAsync(content, overwrite: true, cancellationToken: cancellationToken);
        return blob.Uri.ToString();
    }

    public async Task<Stream> DownloadAsync(string containerName, string blobName, CancellationToken cancellationToken = default)
    {
        var blob = CreateClient().GetBlobContainerClient(containerName).GetBlobClient(blobName);
        var result = await blob.DownloadAsync(cancellationToken);
        return result.Value.Content;
    }

    public async Task DeleteAsync(string containerName, string blobName, CancellationToken cancellationToken = default)
    {
        var blob = CreateClient().GetBlobContainerClient(containerName).GetBlobClient(blobName);
        await blob.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }

    public async Task<bool> ExistsAsync(string containerName, string blobName, CancellationToken cancellationToken = default)
    {
        var blob = CreateClient().GetBlobContainerClient(containerName).GetBlobClient(blobName);
        return await blob.ExistsAsync(cancellationToken);
    }
}
