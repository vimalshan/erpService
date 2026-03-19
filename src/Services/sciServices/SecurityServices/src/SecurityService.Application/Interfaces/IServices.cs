namespace SecurityService.Application.Interfaces;

public interface ICurrentUserService
{
    long? UserId { get; }
    string? UserCode { get; }
    bool IsAuthenticated { get; }
}

public interface IDateTimeService
{
    DateTime Now { get; }
    DateTime UtcNow { get; }
}

public interface IBlobStorageService
{
    Task<string> UploadAsync(string containerName, string blobName, Stream content, string contentType, CancellationToken cancellationToken = default);
    Task<Stream?> DownloadAsync(string containerName, string blobName, CancellationToken cancellationToken = default);
    Task DeleteAsync(string containerName, string blobName, CancellationToken cancellationToken = default);
    Task<string> GetSasUriAsync(string containerName, string blobName, TimeSpan expiry);
}

public interface IMessagePublisher
{
    Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken cancellationToken = default) where T : class;
}
