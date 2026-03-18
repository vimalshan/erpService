using LoanService.Domain.Interfaces;

namespace LoanService.Infrastructure.BlobStorage;

/// <summary>
/// No-op implementation when Azure Blob Storage is not configured.
/// </summary>
public class NullBlobStorageService : IBlobStorageService
{
    public Task<string> UploadAsync(string containerName, string blobName, Stream content, string contentType, CancellationToken ct = default)
        => Task.FromResult($"[blob-not-configured]/{containerName}/{blobName}");

    public Task<Stream?> DownloadAsync(string containerName, string blobName, CancellationToken ct = default)
        => Task.FromResult<Stream?>(null);

    public Task<bool> DeleteAsync(string containerName, string blobName, CancellationToken ct = default)
        => Task.FromResult(false);
}
