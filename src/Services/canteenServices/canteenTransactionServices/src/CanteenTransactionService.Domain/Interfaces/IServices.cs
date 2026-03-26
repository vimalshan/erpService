namespace CanteenTransactionService.Domain.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadImageAsync(string blobName, Stream imageStream, string contentType, CancellationToken ct = default);
    Task<Stream?> DownloadImageAsync(string blobName, CancellationToken ct = default);
    Task DeleteImageAsync(string blobName, CancellationToken ct = default);
}

public interface IMessagePublisher
{
    Task PublishAsync<T>(T message, string routingKey, CancellationToken ct = default) where T : class;
}
