using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ItemMasterService.Domain.Interfaces;

namespace ItemMasterService.Infrastructure.Storage;

public class BlobStorageSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string ContainerName { get; set; } = "canteen-item-images";
}

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobContainerClient _containerClient;
    private readonly ILogger<BlobStorageService> _logger;

    public BlobStorageService(IOptions<BlobStorageSettings> settings, ILogger<BlobStorageService> logger)
    {
        _logger = logger;
        var blobServiceClient = new BlobServiceClient(settings.Value.ConnectionString);
        _containerClient = blobServiceClient.GetBlobContainerClient(settings.Value.ContainerName);
        _containerClient.CreateIfNotExists(PublicAccessType.None);
    }

    public async Task<string> UploadItemImageAsync(long itemCode, Stream imageStream, string contentType, CancellationToken ct = default)
    {
        var blobName = $"item-{itemCode}.jpg";
        var blobClient = _containerClient.GetBlobClient(blobName);

        var uploadOptions = new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders { ContentType = contentType }
        };

        await blobClient.UploadAsync(imageStream, uploadOptions, ct);
        _logger.LogInformation("[BlobStorage] Uploaded image for ItemCode={ItemCode}", itemCode);
        return blobClient.Uri.ToString();
    }

    public async Task<Stream?> DownloadItemImageAsync(long itemCode, CancellationToken ct = default)
    {
        var blobName = $"item-{itemCode}.jpg";
        var blobClient = _containerClient.GetBlobClient(blobName);

        if (!await blobClient.ExistsAsync(ct)) return null;

        var response = await blobClient.DownloadStreamingAsync(cancellationToken: ct);
        return response.Value.Content;
    }

    public async Task DeleteItemImageAsync(long itemCode, CancellationToken ct = default)
    {
        var blobName = $"item-{itemCode}.jpg";
        var blobClient = _containerClient.GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync(cancellationToken: ct);
        _logger.LogInformation("[BlobStorage] Deleted image for ItemCode={ItemCode}", itemCode);
    }
}
