namespace Shared.Core.UnitOfWork;

/// <summary>
/// Unit of Work pattern - Coordinates multiple repositories and ensures atomic transactions
/// </summary>
public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Generic Unit of Work interface for managing specific aggregate repositories
/// </summary>
public interface IUnitOfWork<TDbContext> : IUnitOfWork where TDbContext : class
{
    TDbContext Context { get; }
    Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken = default);
}
