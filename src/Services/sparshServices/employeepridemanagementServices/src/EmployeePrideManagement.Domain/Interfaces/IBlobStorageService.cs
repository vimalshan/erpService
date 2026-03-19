namespace EmployeePrideManagement.Domain.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadImageAsync(Stream imageStream, string fileName, string contentType, CancellationToken cancellationToken = default);
    Task<Stream?> DownloadImageAsync(string blobName, CancellationToken cancellationToken = default);
    Task DeleteImageAsync(string blobName, CancellationToken cancellationToken = default);
    Task<string> GetImageUrlAsync(string blobName, CancellationToken cancellationToken = default);
}
