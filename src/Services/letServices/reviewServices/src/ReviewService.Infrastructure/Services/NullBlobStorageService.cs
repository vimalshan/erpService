using ReviewService.Domain.Interfaces;

namespace ReviewService.Infrastructure.Services;

/// <summary>
/// No-op blob storage for local development when Azure is not configured.
/// </summary>
public sealed class NullBlobStorageService : IBlobStorageService
{
    public Task<string> UploadAsync(string containerName, string blobName, Stream content, string contentType, CancellationToken cancellationToken = default)
        => Task.FromResult($"local://{containerName}/{blobName}");

    public Task<Stream?> DownloadAsync(string containerName, string blobName, CancellationToken cancellationToken = default)
        => Task.FromResult<Stream?>(null);

    public Task DeleteAsync(string containerName, string blobName, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public Task<string> GetSasUriAsync(string containerName, string blobName, TimeSpan expiry)
        => Task.FromResult($"local://{containerName}/{blobName}?expiry={expiry.TotalSeconds}s");
}
