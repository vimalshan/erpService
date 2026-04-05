namespace WebsiteContentService.Infrastructure.ExternalServices;

using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public interface IBlobStorageService
{
    Task<string> UploadImageAsync(Stream fileStream, string fileName, CancellationToken ct = default);
    Task<Stream> DownloadImageAsync(string blobName, CancellationToken ct = default);
    Task DeleteImageAsync(string blobName, CancellationToken ct = default);
}

public class AzureBlobStorageService : IBlobStorageService
{
    private readonly BlobContainerClient _containerClient;
    private readonly ILogger<AzureBlobStorageService> _logger;

    public AzureBlobStorageService(BlobServiceClient blobServiceClient, IConfiguration configuration, ILogger<AzureBlobStorageService> logger)
    {
        _logger = logger;
        var containerName = configuration["AzureStorage:ContainerName"] ?? "website-content-images";
        _containerClient = blobServiceClient.GetBlobContainerClient(containerName);
    }

    public async Task<string> UploadImageAsync(Stream fileStream, string fileName, CancellationToken ct = default)
    {
        var blobClient = _containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(fileStream, overwrite: true, ct);
        _logger.LogInformation("Image uploaded: {FileName}", fileName);
        return blobClient.Uri.ToString();
    }

    public async Task<Stream> DownloadImageAsync(string blobName, CancellationToken ct = default)
    {
        var blobClient = _containerClient.GetBlobClient(blobName);
        var download = await blobClient.DownloadAsync(ct);
        return download.Value.Content;
    }

    public async Task DeleteImageAsync(string blobName, CancellationToken ct = default)
    {
        var blobClient = _containerClient.GetBlobClient(blobName);
        await blobClient.DeleteAsync(cancellationToken: ct);
        _logger.LogInformation("Image deleted: {BlobName}", blobName);
    }
}
