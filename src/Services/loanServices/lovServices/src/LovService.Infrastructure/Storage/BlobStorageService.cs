using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LovService.Infrastructure.Storage;

public interface IBlobStorageService
{
    Task<string> UploadAsync(string fileName, Stream content, string contentType, CancellationToken ct = default);
    Task<Stream?> DownloadAsync(string fileName, CancellationToken ct = default);
    Task DeleteAsync(string fileName, CancellationToken ct = default);
    Task<IEnumerable<string>> ListAsync(CancellationToken ct = default);
}

public sealed class BlobStorageService : IBlobStorageService
{
    private readonly BlobContainerClient _container;
    private readonly ILogger<BlobStorageService> _logger;

    public BlobStorageService(IConfiguration config, ILogger<BlobStorageService> logger)
    {
        _logger = logger;
        var connectionString = config["AzureStorage:ConnectionString"]
            ?? throw new InvalidOperationException("Azure Storage connection string not configured.");
        var containerName = config["AzureStorage:ContainerName"] ?? "lov-images";

        _container = new BlobContainerClient(connectionString, containerName);
        _container.CreateIfNotExists(PublicAccessType.None);
    }

    public async Task<string> UploadAsync(string fileName, Stream content, string contentType, CancellationToken ct)
    {
        var blob = _container.GetBlobClient(fileName);
        await blob.UploadAsync(content, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: ct);
        _logger.LogInformation("Uploaded blob {FileName}", fileName);
        return blob.Uri.ToString();
    }

    public async Task<Stream?> DownloadAsync(string fileName, CancellationToken ct)
    {
        var blob = _container.GetBlobClient(fileName);
        if (!await blob.ExistsAsync(ct)) return null;
        var response = await blob.DownloadAsync(ct);
        return response.Value.Content;
    }

    public async Task DeleteAsync(string fileName, CancellationToken ct)
    {
        var blob = _container.GetBlobClient(fileName);
        await blob.DeleteIfExistsAsync(cancellationToken: ct);
        _logger.LogInformation("Deleted blob {FileName}", fileName);
    }

    public async Task<IEnumerable<string>> ListAsync(CancellationToken ct)
    {
        var blobs = new List<string>();
        await foreach (var item in _container.GetBlobsAsync(cancellationToken: ct))
            blobs.Add(item.Name);
        return blobs;
    }
}
