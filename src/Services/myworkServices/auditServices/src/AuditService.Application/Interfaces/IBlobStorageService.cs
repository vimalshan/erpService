namespace AuditService.Application.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadAsync(string containerName, string fileName, Stream content, string contentType, CancellationToken cancellationToken = default);
    Task<Stream?> DownloadAsync(string containerName, string fileName, CancellationToken cancellationToken = default);
    Task DeleteAsync(string containerName, string fileName, CancellationToken cancellationToken = default);
    Task<IEnumerable<string>> ListAsync(string containerName, CancellationToken cancellationToken = default);
}
