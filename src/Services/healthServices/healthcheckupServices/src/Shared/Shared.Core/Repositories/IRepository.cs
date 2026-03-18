namespace Shared.Core.Repositories;

/// <summary>
/// Generic repository interface for CRUD operations
/// Implements the Repository pattern for data access abstraction
/// </summary>
public interface IRepository<TAggregate, TId> where TAggregate : class where TId : notnull
{
    Task<TAggregate?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TAggregate>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<TAggregate>> FindAsync(Func<TAggregate, bool> predicate, CancellationToken cancellationToken = default);
    Task AddAsync(TAggregate aggregate, CancellationToken cancellationToken = default);
    Task UpdateAsync(TAggregate aggregate, CancellationToken cancellationToken = default);
    Task DeleteAsync(TAggregate aggregate, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
