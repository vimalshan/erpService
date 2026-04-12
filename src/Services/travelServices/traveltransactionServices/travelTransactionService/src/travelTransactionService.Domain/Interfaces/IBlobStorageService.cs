namespace travelTransactionService.Domain.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadFileAsync(string containerName, string fileName, Stream fileStream, string contentType, CancellationToken cancellationToken = default);
    Task<Stream?> DownloadFileAsync(string containerName, string fileName, CancellationToken cancellationToken = default);
    Task<bool> DeleteFileAsync(string containerName, string fileName, CancellationToken cancellationToken = default);
    Task<string> GetFileUrlAsync(string containerName, string fileName, CancellationToken cancellationToken = default);
}
