using StrategicStock.Application.Interfaces;

namespace StrategicStock.Infrastructure.Services;

public sealed class NoOpBlobStorageService : IBlobStorageService
{
    public Task<string> UploadAsync(string containerName, string blobName, Stream content, string contentType, CancellationToken ct = default)
        => Task.FromResult($"noop://{containerName}/{blobName}");

    public Task<Stream?> DownloadAsync(string containerName, string blobName, CancellationToken ct = default)
        => Task.FromResult<Stream?>(Stream.Null);

    public Task<bool> DeleteAsync(string containerName, string blobName, CancellationToken ct = default)
        => Task.FromResult(false);
}
