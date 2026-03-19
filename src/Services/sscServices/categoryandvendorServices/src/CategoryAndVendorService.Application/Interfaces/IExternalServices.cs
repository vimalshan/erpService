namespace CategoryAndVendorService.Application.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadFileAsync(string containerName, string fileName, Stream content, string contentType, CancellationToken ct = default);
    Task<Stream?> DownloadFileAsync(string containerName, string fileName, CancellationToken ct = default);
    Task<bool> DeleteFileAsync(string containerName, string fileName, CancellationToken ct = default);
}

public interface IMessagePublisher
{
    Task PublishAsync<T>(string queueName, T message, CancellationToken ct = default);
}
