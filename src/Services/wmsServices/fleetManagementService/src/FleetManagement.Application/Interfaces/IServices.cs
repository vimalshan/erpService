namespace FleetManagement.Application.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadFileAsync(string containerName, string fileName, Stream content, string contentType, CancellationToken ct = default);
    Task<Stream?> DownloadFileAsync(string containerName, string fileName, CancellationToken ct = default);
    Task DeleteFileAsync(string containerName, string fileName, CancellationToken ct = default);
    Task<string> GetFileUrlAsync(string containerName, string fileName, CancellationToken ct = default);
}

public interface IMessagePublisher
{
    Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken ct = default);
}

public interface IDapperQueryService
{
    Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null, CancellationToken ct = default);
    Task<T?> QuerySingleOrDefaultAsync<T>(string sql, object? param = null, CancellationToken ct = default);
    Task<int> ExecuteAsync(string sql, object? param = null, CancellationToken ct = default);
}
