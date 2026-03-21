namespace FinanceService.Application.Common.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadAsync(string containerName, string fileName, Stream content, string contentType);
    Task<Stream?> DownloadAsync(string containerName, string fileName);
    Task<bool> DeleteAsync(string containerName, string fileName);
    Task<string> GetBlobUrlAsync(string containerName, string fileName);
}
