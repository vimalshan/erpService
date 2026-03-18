namespace AccessService.Application.Interfaces;

/// <summary>
/// Unit of Work abstraction for the Application layer
/// Defines contracts for all repositories and transaction management
/// </summary>
public interface IUnitOfWork : IDisposable
{
    // Repository properties - strongly typed for specific aggregates
    // Infrastructure will provide implementations
    dynamic UserMaps { get; }
    dynamic UserRoles { get; }
    dynamic Menus { get; }
    dynamic SPARSHMenus { get; }
    dynamic SPARSHMenuAccesses { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Generic repository interface for base CRUD operations
/// </summary>
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
}
