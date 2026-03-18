using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DealTicketing.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;

namespace DealTicketing.Infrastructure.BlobStorage;

public class AzureBlobStorageService(IConfiguration configuration) : IBlobStorageService
{
    private BlobServiceClient CreateClient()
        => new(configuration["Azure:BlobStorage:ConnectionString"]);

    public async Task<string> UploadAsync(
        string containerName, string blobName, Stream content, string contentType, CancellationToken ct = default)
    {
        var containerClient = CreateClient().GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: ct);

        var blobClient = containerClient.GetBlobClient(blobName);
        await blobClient.UploadAsync(content, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: ct);
        return blobClient.Uri.AbsoluteUri;
    }

    public async Task<Stream> DownloadAsync(string containerName, string blobName, CancellationToken ct = default)
    {
        var blobClient = CreateClient().GetBlobContainerClient(containerName).GetBlobClient(blobName);
        var response = await blobClient.DownloadAsync(ct);
        return response.Value.Content;
    }

    public async Task DeleteAsync(string containerName, string blobName, CancellationToken ct = default)
    {
        var blobClient = CreateClient().GetBlobContainerClient(containerName).GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync(cancellationToken: ct);
    }

    public async Task<string> GetSasUriAsync(
        string containerName, string blobName, TimeSpan expiry, CancellationToken ct = default)
    {
        var blobClient = CreateClient().GetBlobContainerClient(containerName).GetBlobClient(blobName);
        var sasUri = blobClient.GenerateSasUri(Azure.Storage.Sas.BlobSasPermissions.Read, DateTimeOffset.UtcNow.Add(expiry));
        return await Task.FromResult(sasUri.AbsoluteUri);
    }
}
