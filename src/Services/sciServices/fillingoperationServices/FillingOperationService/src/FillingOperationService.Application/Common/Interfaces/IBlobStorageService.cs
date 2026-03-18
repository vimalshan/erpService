namespace FillingOperationService.Application.Common.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadImageAsync(string containerName, string fileName, Stream content, string contentType, CancellationToken cancellationToken = default);
    Task<Stream?> DownloadImageAsync(string containerName, string fileName, CancellationToken cancellationToken = default);
    Task DeleteImageAsync(string containerName, string fileName, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string containerName, string fileName, CancellationToken cancellationToken = default);
}
