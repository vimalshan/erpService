using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AuditLogService.Infrastructure.BlobStorage;

public class BlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly BlobStorageSettings _settings;
    private readonly ILogger<BlobStorageService> _logger;

    public BlobStorageService(IOptions<BlobStorageSettings> settings, ILogger<BlobStorageService> logger)
    {
        _settings = settings.Value;
        _blobServiceClient = new BlobServiceClient(_settings.ConnectionString);
        _logger = logger;
    }

    private BlobContainerClient GetContainerClient()
    {
        return _blobServiceClient.GetBlobContainerClient(_settings.ContainerName);
    }

    public async Task<string> UploadAsync(string fileName, Stream content, string contentType, CancellationToken cancellationToken = default)
    {
        var containerClient = GetContainerClient();
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: cancellationToken);

        var blobClient = containerClient.GetBlobClient(fileName);
        var headers = new BlobHttpHeaders { ContentType = contentType };

        await blobClient.UploadAsync(content, new BlobUploadOptions { HttpHeaders = headers }, cancellationToken);

        _logger.LogInformation("Uploaded blob {FileName} to container {Container}", fileName, _settings.ContainerName);
        return blobClient.Uri.ToString();
    }

    public async Task<Stream?> DownloadAsync(string fileName, CancellationToken cancellationToken = default)
    {
        var containerClient = GetContainerClient();
        var blobClient = containerClient.GetBlobClient(fileName);

        if (!await blobClient.ExistsAsync(cancellationToken))
            return null;

        var response = await blobClient.DownloadStreamingAsync(cancellationToken: cancellationToken);
        return response.Value.Content;
    }

    public async Task<bool> DeleteAsync(string fileName, CancellationToken cancellationToken = default)
    {
        var containerClient = GetContainerClient();
        var blobClient = containerClient.GetBlobClient(fileName);
        var response = await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);

        _logger.LogInformation("Deleted blob {FileName}: {Deleted}", fileName, response.Value);
        return response.Value;
    }
}
