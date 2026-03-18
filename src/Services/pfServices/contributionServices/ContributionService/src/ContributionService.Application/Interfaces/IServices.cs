namespace ContributionService.Application.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadAsync(string containerName, string fileName, Stream content, string contentType, CancellationToken ct = default);
    Task<Stream?> DownloadAsync(string containerName, string fileName, CancellationToken ct = default);
    Task<bool> DeleteAsync(string containerName, string fileName, CancellationToken ct = default);
    Task<string> GetUrlAsync(string containerName, string fileName, CancellationToken ct = default);
}

public interface IMessagePublisher
{
    Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken ct = default);
}

public interface IDapperQueryService
{
    Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object? parameters = null, CancellationToken ct = default);
    Task<T?> QuerySingleOrDefaultAsync<T>(string sql, object? parameters = null, CancellationToken ct = default);
    Task<int> ExecuteAsync(string sql, object? parameters = null, CancellationToken ct = default);
}
