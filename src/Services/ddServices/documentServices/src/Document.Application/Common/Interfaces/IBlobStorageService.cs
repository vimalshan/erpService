namespace Document.Application.Common.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadAsync(Stream fileStream, string fileName, string containerName, CancellationToken cancellationToken = default);
    Task<Stream> DownloadAsync(string blobName, string containerName, CancellationToken cancellationToken = default);
    Task DeleteAsync(string blobName, string containerName, CancellationToken cancellationToken = default);
    Task<string> GetSasUriAsync(string blobName, string containerName, TimeSpan expiresIn);
}
