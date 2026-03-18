using TdsService.Application.Common.Interfaces;

namespace TdsService.Infrastructure.Services;

/// <summary>
/// No-op Blob Storage implementation used when Azure Storage is not configured.
/// </summary>
internal sealed class NullBlobStorageService : IBlobStorageService
{
    public Task<string> UploadAsync(string containerName, string blobName, Stream content, string contentType, CancellationToken ct = default)
        => Task.FromResult($"blob://{containerName}/{blobName}");

    public Task<Stream> DownloadAsync(string containerName, string blobName, CancellationToken ct = default)
        => Task.FromResult<Stream>(Stream.Null);

    public Task DeleteAsync(string containerName, string blobName, CancellationToken ct = default)
        => Task.CompletedTask;

    public Task<bool> ExistsAsync(string containerName, string blobName, CancellationToken ct = default)
        => Task.FromResult(false);

    public string GetBlobUri(string containerName, string blobName)
        => $"blob://{containerName}/{blobName}";
}
