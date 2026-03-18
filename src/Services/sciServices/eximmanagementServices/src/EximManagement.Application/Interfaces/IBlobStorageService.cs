namespace EximManagement.Application.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadAsync(Stream stream, string containerName, string blobName, string contentType, CancellationToken ct = default);
    Task<Stream> DownloadAsync(string containerName, string blobName, CancellationToken ct = default);
    Task DeleteAsync(string containerName, string blobName, CancellationToken ct = default);
    Task<string> GetSasUriAsync(string containerName, string blobName, TimeSpan expiry, CancellationToken ct = default);
}
