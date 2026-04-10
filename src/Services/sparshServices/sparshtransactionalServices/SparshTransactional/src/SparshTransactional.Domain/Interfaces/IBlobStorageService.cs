namespace SparshTransactional.Domain.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadAsync(string containerName, string fileName, Stream content, string contentType, CancellationToken ct = default);
    Task<Stream?> DownloadAsync(string containerName, string fileName, CancellationToken ct = default);
    Task DeleteAsync(string containerName, string fileName, CancellationToken ct = default);
    Task<string> GetSasUrlAsync(string containerName, string fileName, TimeSpan expiry, CancellationToken ct = default);
}
