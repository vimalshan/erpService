using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ContributionService.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ContributionService.Infrastructure.Services;

public class BlobStorageService(IConfiguration configuration) : IBlobStorageService
{
    private BlobServiceClient CreateClient()
        => new(configuration.GetConnectionString("BlobStorage"));

    public async Task<string> UploadAsync(string containerName, string fileName, Stream content, string contentType, CancellationToken ct = default)
    {
        var client = CreateClient();
        var containerClient = client.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: ct);

        var blobClient = containerClient.GetBlobClient(fileName);
        var headers = new BlobHttpHeaders { ContentType = contentType };
        await blobClient.UploadAsync(content, new BlobUploadOptions { HttpHeaders = headers }, ct);

        return blobClient.Uri.ToString();
    }

    public async Task<Stream?> DownloadAsync(string containerName, string fileName, CancellationToken ct = default)
    {
        var client = CreateClient();
        var blobClient = client.GetBlobContainerClient(containerName).GetBlobClient(fileName);

        if (!await blobClient.ExistsAsync(ct))
            return null;

        var response = await blobClient.DownloadStreamingAsync(cancellationToken: ct);
        return response.Value.Content;
    }

    public async Task<bool> DeleteAsync(string containerName, string fileName, CancellationToken ct = default)
    {
        var client = CreateClient();
        var blobClient = client.GetBlobContainerClient(containerName).GetBlobClient(fileName);
        var response = await blobClient.DeleteIfExistsAsync(cancellationToken: ct);
        return response.Value;
    }

    public Task<string> GetUrlAsync(string containerName, string fileName, CancellationToken ct = default)
    {
        var client = CreateClient();
        var blobClient = client.GetBlobContainerClient(containerName).GetBlobClient(fileName);
        return Task.FromResult(blobClient.Uri.ToString());
    }
}
