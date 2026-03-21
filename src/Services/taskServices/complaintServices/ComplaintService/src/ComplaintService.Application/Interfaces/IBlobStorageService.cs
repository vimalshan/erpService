namespace ComplaintService.Application.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadFileAsync(Stream stream, string fileName, string contentType, CancellationToken ct = default);
    Task<Stream> DownloadFileAsync(string blobName, CancellationToken ct = default);
    Task DeleteFileAsync(string blobName, CancellationToken ct = default);
}
