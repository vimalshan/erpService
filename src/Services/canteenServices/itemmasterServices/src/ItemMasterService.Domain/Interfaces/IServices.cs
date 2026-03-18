namespace ItemMasterService.Domain.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadItemImageAsync(long itemCode, Stream imageStream, string contentType, CancellationToken ct = default);
    Task<Stream?> DownloadItemImageAsync(long itemCode, CancellationToken ct = default);
    Task DeleteItemImageAsync(long itemCode, CancellationToken ct = default);
}

public interface IMessagePublisher
{
    Task PublishAsync<T>(T message, string routingKey, CancellationToken ct = default) where T : class;
}
