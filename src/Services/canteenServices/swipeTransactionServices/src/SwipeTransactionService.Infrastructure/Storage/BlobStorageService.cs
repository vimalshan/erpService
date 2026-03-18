using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace SwipeTransactionService.Infrastructure.Storage;

public interface IBlobStorageService
{
    Task<string> UploadItemImageAsync(string itemCode, Stream imageStream, string contentType, CancellationToken ct = default);
    Task<Stream?> DownloadItemImageAsync(string itemCode, CancellationToken ct = default);
    Task DeleteItemImageAsync(string itemCode, CancellationToken ct = default);
}

public sealed class BlobStorageService : IBlobStorageService
{
    private const string ContainerName = "canteen-item-images";
    private readonly BlobContainerClient _containerClient;

    public BlobStorageService(BlobServiceClient blobServiceClient)
    {
        _containerClient = blobServiceClient.GetBlobContainerClient(ContainerName);
    }

    public async Task<string> UploadItemImageAsync(
        string itemCode,
        Stream imageStream,
        string contentType,
        CancellationToken ct = default)
    {
        await _containerClient.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: ct);

        var blobName = $"{itemCode.Trim()}.img";
        var blobClient = _containerClient.GetBlobClient(blobName);
        await blobClient.UploadAsync(imageStream, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: ct);
        return blobClient.Uri.ToString();
    }

    public async Task<Stream?> DownloadItemImageAsync(string itemCode, CancellationToken ct = default)
    {
        var blobName = $"{itemCode.Trim()}.img";
        var blobClient = _containerClient.GetBlobClient(blobName);

        if (!await blobClient.ExistsAsync(ct)) return null;

        var response = await blobClient.DownloadStreamingAsync(cancellationToken: ct);
        return response.Value.Content;
    }

    public async Task DeleteItemImageAsync(string itemCode, CancellationToken ct = default)
    {
        var blobName = $"{itemCode.Trim()}.img";
        var blobClient = _containerClient.GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync(cancellationToken: ct);
    }
}
