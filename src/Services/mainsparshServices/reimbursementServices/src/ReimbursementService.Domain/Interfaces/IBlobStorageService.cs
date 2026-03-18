namespace ReimbursementService.Domain.Interfaces;

/// <summary>Abstraction for blob storage operations (e.g., receipt images).</summary>
public interface IBlobStorageService
{
    Task<string> UploadAsync(string containerName, string blobName, Stream data, string contentType, CancellationToken cancellationToken = default);
    Task<Stream> DownloadAsync(string containerName, string blobName, CancellationToken cancellationToken = default);
    Task DeleteAsync(string containerName, string blobName, CancellationToken cancellationToken = default);
    Task<string> GetBlobUrlAsync(string containerName, string blobName, CancellationToken cancellationToken = default);
}
