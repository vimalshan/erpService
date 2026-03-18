namespace CheckupManagementService.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using Shared.Core.Repositories;
using CheckupManagementService.Infrastructure.Persistence;

/// <summary>
/// Generic implementation of the IRepository pattern for Entity Framework Core
/// Provides CRUD operations for any entity type
/// </summary>
/// <typeparam name="TAggregate">The entity type</typeparam>
/// <typeparam name="TId">The primary key type</typeparam>
public class GenericRepository<TAggregate, TId> : IRepository<TAggregate, TId>
    where TAggregate : class
    where TId : notnull
{
    private readonly CheckupManagementDbContext _dbContext;
    private readonly DbSet<TAggregate> _dbSet;

    public GenericRepository(CheckupManagementDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dbSet = dbContext.Set<TAggregate>();
    }

    /// <summary>
    /// Get entity by its primary key asynchronously
    /// </summary>
    public async Task<TAggregate?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync(new object[] { id }, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Get all entities asynchronously
    /// </summary>
    public async Task<IEnumerable<TAggregate>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Find entities matching the predicate asynchronously
    /// </summary>
    public async Task<IEnumerable<TAggregate>> FindAsync(Func<TAggregate, bool> predicate, CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(_dbSet.Where(predicate).ToList());
    }

    /// <summary>
    /// Add a new entity asynchronously
    /// </summary>
    public async Task AddAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
    {
        if (aggregate == null)
            throw new ArgumentNullException(nameof(aggregate));

        await _dbSet.AddAsync(aggregate, cancellationToken);
    }

    /// <summary>
    /// Update an existing entity asynchronously
    /// </summary>
    public async Task UpdateAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
    {
        if (aggregate == null)
            throw new ArgumentNullException(nameof(aggregate));

        _dbSet.Update(aggregate);
        await Task.CompletedTask;
    }

    /// <summary>
    /// Delete an entity asynchronously
    /// </summary>
    public async Task DeleteAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
    {
        if (aggregate == null)
            throw new ArgumentNullException(nameof(aggregate));

        _dbSet.Remove(aggregate);
        await Task.CompletedTask;
    }

    /// <summary>
    /// Save all changes to the database asynchronously
    /// </summary>
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
