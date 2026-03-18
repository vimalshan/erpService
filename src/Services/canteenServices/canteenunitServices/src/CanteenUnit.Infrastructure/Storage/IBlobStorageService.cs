namespace CanteenUnit.Infrastructure.Storage;

public interface IBlobStorageService
{
    Task<string> UploadAsync(string containerName, string blobName, Stream content, string contentType, CancellationToken ct = default);
    Task<Stream?> DownloadAsync(string containerName, string blobName, CancellationToken ct = default);
    Task DeleteAsync(string containerName, string blobName, CancellationToken ct = default);
    Task<IEnumerable<string>> ListBlobsAsync(string containerName, string? prefix = null, CancellationToken ct = default);
}
