namespace RecruitmentService.Application.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadAsync(Stream content, string fileName, string containerName, CancellationToken ct = default);
    Task<Stream> DownloadAsync(string fileName, string containerName, CancellationToken ct = default);
    Task DeleteAsync(string fileName, string containerName, CancellationToken ct = default);
    Task<bool> ExistsAsync(string fileName, string containerName, CancellationToken ct = default);
    string GetPublicUri(string fileName, string containerName);
}
