using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MamAllocationService.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace MamAllocationService.Infrastructure.BlobStorage;

public class AzureBlobStorageService(IConfiguration configuration) : IBlobStorageService
{
    private BlobServiceClient GetClient()
    {
        var connectionString = configuration["AzureBlobStorage:ConnectionString"]
            ?? "UseDevelopmentStorage=true";
        return new BlobServiceClient(connectionString);
    }

    public async Task<string> UploadAsync(string containerName, string blobName, Stream content, string contentType, CancellationToken ct = default)
    {
        var client = GetClient();
        var container = client.GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync(cancellationToken: ct);

        var blob = container.GetBlobClient(blobName);
        await blob.UploadAsync(content, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: ct);
        return blob.Uri.ToString();
    }

    public async Task<Stream?> DownloadAsync(string containerName, string blobName, CancellationToken ct = default)
    {
        var client = GetClient();
        var container = client.GetBlobContainerClient(containerName);
        var blob = container.GetBlobClient(blobName);

        if (!await blob.ExistsAsync(ct)) return null;

        var response = await blob.DownloadStreamingAsync(cancellationToken: ct);
        return response.Value.Content;
    }

    public async Task<bool> DeleteAsync(string containerName, string blobName, CancellationToken ct = default)
    {
        var client = GetClient();
        var container = client.GetBlobContainerClient(containerName);
        var blob = container.GetBlobClient(blobName);
        var response = await blob.DeleteIfExistsAsync(cancellationToken: ct);
        return response.Value;
    }
}
