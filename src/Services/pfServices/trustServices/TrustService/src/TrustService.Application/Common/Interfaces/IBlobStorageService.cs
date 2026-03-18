namespace TrustService.Application.Common.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadAsync(string blobName, Stream content, string contentType, CancellationToken cancellationToken = default);
    Task<Stream?> DownloadAsync(string blobName, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string blobName, CancellationToken cancellationToken = default);
    Task<string> GetBlobUrlAsync(string blobName, CancellationToken cancellationToken = default);
}
