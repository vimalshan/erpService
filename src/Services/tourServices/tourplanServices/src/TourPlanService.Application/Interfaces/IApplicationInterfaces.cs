namespace TourPlanService.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}

public interface IBlobStorageService
{
    Task<string> UploadAsync(string containerName, string blobName, Stream content, string contentType, CancellationToken cancellationToken = default);
    Task<Stream?> DownloadAsync(string containerName, string blobName, CancellationToken cancellationToken = default);
    Task DeleteAsync(string containerName, string blobName, CancellationToken cancellationToken = default);
    Task<string> GetBlobUrlAsync(string containerName, string blobName);
}

public interface IMessagePublisher
{
    Task PublishAsync<T>(string exchangeName, string routingKey, T message, CancellationToken cancellationToken = default)
        where T : class;
}

public interface ICurrentUser
{
    string? UserId { get; }
    string? UserName { get; }
    IEnumerable<string> Roles { get; }
    bool IsAuthenticated { get; }
}
