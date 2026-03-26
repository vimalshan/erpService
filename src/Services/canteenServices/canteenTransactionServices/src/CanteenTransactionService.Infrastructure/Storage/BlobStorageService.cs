using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CanteenTransactionService.Domain.Interfaces;

namespace CanteenTransactionService.Infrastructure.Storage;

public class BlobStorageSettings
{
    public string ConnectionString { get; set; } = "UseDevelopmentStorage=true";
    public string ContainerName { get; set; } = "canteen-transaction-images";
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

    public async Task<string> UploadImageAsync(string blobName, Stream imageStream, string contentType, CancellationToken ct = default)
    {
        var blobClient = _containerClient.GetBlobClient(blobName);
        var headers = new BlobHttpHeaders { ContentType = contentType };
        await blobClient.UploadAsync(imageStream, new BlobUploadOptions { HttpHeaders = headers }, ct);
        _logger.LogInformation("Uploaded blob: {BlobName}", blobName);
        return blobClient.Uri.ToString();
    }

    public async Task<Stream?> DownloadImageAsync(string blobName, CancellationToken ct = default)
    {
        var blobClient = _containerClient.GetBlobClient(blobName);
        if (!await blobClient.ExistsAsync(ct)) return null;
        var response = await blobClient.DownloadStreamingAsync(cancellationToken: ct);
        return response.Value.Content;
    }

    public async Task DeleteImageAsync(string blobName, CancellationToken ct = default)
    {
        var blobClient = _containerClient.GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync(cancellationToken: ct);
        _logger.LogInformation("Deleted blob: {BlobName}", blobName);
    }
}
