namespace AimsTransactionService.Application.Common.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default);
    Task DeleteAsync(string blobName, CancellationToken cancellationToken = default);
    Task<Stream> DownloadAsync(string blobName, CancellationToken cancellationToken = default);
}
