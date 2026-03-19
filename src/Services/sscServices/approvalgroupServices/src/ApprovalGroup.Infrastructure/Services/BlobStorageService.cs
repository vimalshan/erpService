using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ApprovalGroup.Infrastructure.Services;

public class BlobStorageSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string ContainerName { get; set; } = "approval-group-images";
}

public interface IBlobStorageService
{
    Task<string> UploadAsync(Stream content, string fileName, string contentType, CancellationToken ct = default);
    Task<Stream?> DownloadAsync(string fileName, CancellationToken ct = default);
    Task DeleteAsync(string fileName, CancellationToken ct = default);
    Task<bool> ExistsAsync(string fileName, CancellationToken ct = default);
}

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;
    private readonly ILogger<BlobStorageService> _logger;

    public BlobStorageService(IOptions<BlobStorageSettings> settings, ILogger<BlobStorageService> logger)
    {
        _containerName = settings.Value.ContainerName;
        _logger = logger;
        _blobServiceClient = new BlobServiceClient(settings.Value.ConnectionString);
    }

    private BlobContainerClient GetContainerClient() =>
        _blobServiceClient.GetBlobContainerClient(_containerName);

    public async Task<string> UploadAsync(Stream content, string fileName, string contentType, CancellationToken ct = default)
    {
        var container = GetContainerClient();
        await container.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: ct);

        var blobClient = container.GetBlobClient(fileName);
        await blobClient.UploadAsync(content, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: ct);

        _logger.LogInformation("Uploaded blob {FileName} to {Container}", fileName, _containerName);
        return blobClient.Uri.ToString();
    }

    public async Task<Stream?> DownloadAsync(string fileName, CancellationToken ct = default)
    {
        var blobClient = GetContainerClient().GetBlobClient(fileName);
        if (!await blobClient.ExistsAsync(ct)) return null;

        var response = await blobClient.DownloadStreamingAsync(cancellationToken: ct);
        return response.Value.Content;
    }

    public async Task DeleteAsync(string fileName, CancellationToken ct = default)
    {
        var blobClient = GetContainerClient().GetBlobClient(fileName);
        await blobClient.DeleteIfExistsAsync(cancellationToken: ct);
    }

    public async Task<bool> ExistsAsync(string fileName, CancellationToken ct = default)
    {
        var blobClient = GetContainerClient().GetBlobClient(fileName);
        return await blobClient.ExistsAsync(ct);
    }
}
