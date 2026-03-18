using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

namespace EligibilityService.Infrastructure.Services;

public interface IBlobStorageService
{
    Task<string> UploadItemImageAsync(Stream imageStream, string fileName, string contentType, CancellationToken ct = default);
    Task<Stream?> DownloadItemImageAsync(string blobName, CancellationToken ct = default);
    Task DeleteItemImageAsync(string blobName, CancellationToken ct = default);
}

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobContainerClient _container;

    public BlobStorageService(IConfiguration configuration)
    {
        var connectionString = configuration["AzureBlobStorage:ConnectionString"]
            ?? throw new InvalidOperationException("AzureBlobStorage:ConnectionString is not configured.");
        var containerName = configuration["AzureBlobStorage:ContainerName"] ?? "canteen-items";

        _container = new BlobContainerClient(connectionString, containerName);
    }

    public async Task<string> UploadItemImageAsync(Stream imageStream, string fileName, string contentType, CancellationToken ct)
    {
        await _container.CreateIfNotExistsAsync(PublicAccessType.Blob, cancellationToken: ct);

        var blobName = $"{Guid.NewGuid():N}_{fileName}";
        var blob = _container.GetBlobClient(blobName);

        await blob.UploadAsync(imageStream, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: ct);
        return blob.Uri.ToString();
    }

    public async Task<Stream?> DownloadItemImageAsync(string blobName, CancellationToken ct)
    {
        var blob = _container.GetBlobClient(blobName);
        if (!await blob.ExistsAsync(ct)) return null;

        var response = await blob.DownloadAsync(ct);
        return response.Value.Content;
    }

    public async Task DeleteItemImageAsync(string blobName, CancellationToken ct)
    {
        var blob = _container.GetBlobClient(blobName);
        await blob.DeleteIfExistsAsync(cancellationToken: ct);
    }
}
