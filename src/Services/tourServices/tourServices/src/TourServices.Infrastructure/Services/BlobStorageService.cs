using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

namespace TourServices.Infrastructure.Services;

public sealed class BlobStorageService
{
    private readonly BlobServiceClient _client;
    private const string ContainerName = "tour-images";

    public BlobStorageService(IConfiguration configuration)
    {
        var connectionString = configuration["AzureBlobStorage:ConnectionString"]
            ?? throw new InvalidOperationException("AzureBlobStorage:ConnectionString is not configured.");
        _client = new BlobServiceClient(connectionString);
    }

    public async Task<string> UploadImageAsync(
        long tourId, Stream imageStream, string contentType, CancellationToken ct = default)
    {
        var container = _client.GetBlobContainerClient(ContainerName);
        await container.CreateIfNotExistsAsync(PublicAccessType.Blob, cancellationToken: ct);

        var blobName = $"tours/{tourId}/{Guid.NewGuid()}";
        var blob = container.GetBlobClient(blobName);
        await blob.UploadAsync(imageStream, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: ct);

        return blob.Uri.ToString();
    }

    public async Task<bool> DeleteImageAsync(string imageUrl, CancellationToken ct = default)
    {
        var uri = new Uri(imageUrl);
        var blobName = string.Join("/", uri.Segments[2..]);
        var container = _client.GetBlobContainerClient(ContainerName);
        var blob = container.GetBlobClient(blobName);
        return await blob.DeleteIfExistsAsync(cancellationToken: ct);
    }
}
