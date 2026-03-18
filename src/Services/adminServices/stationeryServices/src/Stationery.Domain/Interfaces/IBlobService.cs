namespace Stationery.Domain.Interfaces;

public interface IBlobService
{
    Task<string> UploadAsync(Stream fileStream, string fileName, string contentType);
    Task<Stream> DownloadAsync(string blobName);
    Task DeleteAsync(string blobName);
}
