namespace SciTransactional.Application.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadAsync(string containerName, string blobName, Stream content,
        string contentType, CancellationToken ct = default);
    Task<Stream?> DownloadAsync(string containerName, string blobName,
        CancellationToken ct = default);
    Task<bool> DeleteAsync(string containerName, string blobName,
        CancellationToken ct = default);
}

public interface IDapperContext
{
    Task<IReadOnlyList<T>> QueryStoredProcAsync<T>(string storedProcedure,
        object? parameters = null);
    Task<T?> QuerySingleStoredProcAsync<T>(string storedProcedure,
        object? parameters = null);
}

public interface IRabbitMqPublisher
{
    Task PublishAsync<T>(string exchange, string routingKey, T message,
        CancellationToken ct = default);
}
