namespace FaqServices.Domain.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadAsync(Stream content, string fileName, string contentType, CancellationToken ct = default);
    Task<bool> DeleteAsync(string blobUrl, CancellationToken ct = default);
    Task<Stream?> DownloadAsync(string blobUrl, CancellationToken ct = default);
}
