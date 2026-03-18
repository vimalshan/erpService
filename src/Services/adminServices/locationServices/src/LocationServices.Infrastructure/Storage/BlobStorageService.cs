using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LocationServices.Infrastructure.Storage;

public sealed class BlobStorageOptions
{
    public string ConnectionString { get; init; } = string.Empty;
    public string ContainerName    { get; init; } = "location-exports";
}

public interface IBlobStorageService
{
    Task<string> UploadAsync(string blobName, Stream content, string contentType = "application/octet-stream", CancellationToken ct = default);
    Task<Stream> DownloadAsync(string blobName, CancellationToken ct = default);
    Task DeleteAsync(string blobName, CancellationToken ct = default);
    Task<IEnumerable<string>> ListBlobsAsync(string? prefix = null, CancellationToken ct = default);
}

public sealed class BlobStorageService : IBlobStorageService
{
    private readonly BlobContainerClient _container;
    private readonly ILogger<BlobStorageService> _logger;

    public BlobStorageService(IOptions<BlobStorageOptions> options, ILogger<BlobStorageService> logger)
    {
        _logger = logger;
        var opts = options.Value;
        var client = new BlobServiceClient(opts.ConnectionString);
        _container = client.GetBlobContainerClient(opts.ContainerName);
        _container.CreateIfNotExists(PublicAccessType.None);
    }

    public async Task<string> UploadAsync(string blobName, Stream content, string contentType = "application/octet-stream", CancellationToken ct = default)
    {
        var blob   = _container.GetBlobClient(blobName);
        var headers = new BlobHttpHeaders { ContentType = contentType };
        await blob.UploadAsync(content, new BlobUploadOptions { HttpHeaders = headers }, ct);
        _logger.LogInformation("[Blob] Uploaded {BlobName}", blobName);
        return blob.Uri.ToString();
    }

    public async Task<Stream> DownloadAsync(string blobName, CancellationToken ct = default)
    {
        var blob     = _container.GetBlobClient(blobName);
        var response = await blob.DownloadStreamingAsync(cancellationToken: ct);
        return response.Value.Content;
    }

    public async Task DeleteAsync(string blobName, CancellationToken ct = default)
    {
        var blob = _container.GetBlobClient(blobName);
        await blob.DeleteIfExistsAsync(cancellationToken: ct);
        _logger.LogInformation("[Blob] Deleted {BlobName}", blobName);
    }

    public async Task<IEnumerable<string>> ListBlobsAsync(string? prefix = null, CancellationToken ct = default)
    {
        var names = new List<string>();
        await foreach (var item in _container.GetBlobsAsync(prefix: prefix, cancellationToken: ct))
            names.Add(item.Name);
        return names;
    }
}
