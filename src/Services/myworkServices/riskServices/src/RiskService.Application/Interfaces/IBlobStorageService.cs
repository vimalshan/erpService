namespace RiskService.Application.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadAsync(string containerName, string fileName, Stream content, string contentType, CancellationToken ct = default);
    Task<Stream?> DownloadAsync(string containerName, string fileName, CancellationToken ct = default);
    Task<bool> DeleteAsync(string containerName, string fileName, CancellationToken ct = default);
}
