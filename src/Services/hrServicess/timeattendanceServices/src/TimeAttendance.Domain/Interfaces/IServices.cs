namespace TimeAttendance.Domain.Interfaces;

public interface IMessagePublisher
{
    Task PublishAsync<T>(string topic, T message, CancellationToken cancellationToken = default) where T : class;
}

public interface IBlobStorageService
{
    Task<string> UploadAsync(string containerName, string blobName, Stream content, string contentType, CancellationToken cancellationToken = default);
    Task<Stream?> DownloadAsync(string containerName, string blobName, CancellationToken cancellationToken = default);
    Task DeleteAsync(string containerName, string blobName, CancellationToken cancellationToken = default);
    Task<string> GetSasUrlAsync(string containerName, string blobName, TimeSpan expiry, CancellationToken cancellationToken = default);
}
