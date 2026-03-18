using MediatR;

namespace ObjectiveService.Application.Interfaces;

/// <summary>
/// Interface for publishing domain events
/// </summary>
public interface IDomainEventService
{
    Task PublishAsync<T>(T domainEvent, CancellationToken cancellationToken = default) where T : INotification;
}

/// <summary>
/// Interface for repository pattern
/// </summary>
public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(decimal id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(decimal id, CancellationToken cancellationToken = default);
    IQueryable<T> AsQueryable();
}

/// <summary>
/// Unit of Work interface
/// </summary>
public interface IUnitOfWork : IAsyncDisposable
{
    IRepository<TEntity> Repository<TEntity>() where TEntity : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Interface for database context
/// </summary>
public interface IApplicationDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
