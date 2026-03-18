namespace LocationServices.Domain.Repositories;

/// <summary>Unit of Work interface — coordinates repositories in a transaction</summary>
public interface IUnitOfWork : IDisposable
{
    ILocationAppMapRepository LocationAppMaps { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);
    Task BeginTransactionAsync(CancellationToken ct = default);
    Task CommitTransactionAsync(CancellationToken ct = default);
    Task RollbackTransactionAsync(CancellationToken ct = default);
}
