namespace CashManagement.Domain.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadAsync(Stream content, string fileName, string containerName, CancellationToken ct = default);
    Task<Stream> DownloadAsync(string fileName, string containerName, CancellationToken ct = default);
    Task DeleteAsync(string fileName, string containerName, CancellationToken ct = default);
    Task<string> GetSasUriAsync(string fileName, string containerName, TimeSpan expiry, CancellationToken ct = default);
}
