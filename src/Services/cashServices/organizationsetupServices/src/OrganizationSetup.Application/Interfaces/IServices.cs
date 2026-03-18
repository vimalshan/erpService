namespace OrganizationSetup.Application.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadAsync(string containerName, string blobName, Stream content, CancellationToken ct = default);
    Task<Stream> DownloadAsync(string containerName, string blobName, CancellationToken ct = default);
    Task<bool> DeleteAsync(string containerName, string blobName, CancellationToken ct = default);
}

public interface IMessagePublisher
{
    Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken ct = default);
}

public interface IUnitOfWork : IDisposable
{
    IRoleRepository Roles { get; }
    IUserMapRepository UserMaps { get; }
    IOrgParamsRepository OrgParams { get; }
    IPpLimitRepository PpLimits { get; }
    
    Task<int> SaveChangesAsync(CancellationToken ct = default);
    Task BeginTransactionAsync(CancellationToken ct = default);
    Task CommitAsync(CancellationToken ct = default);
    Task RollbackAsync(CancellationToken ct = default);
}

public interface ICurrentUserService
{
    long? UserId { get; }
    long? OrganizationId { get; }
    IReadOnlyCollection<string> Roles { get; }
    bool HasPermission(string permission);
}
