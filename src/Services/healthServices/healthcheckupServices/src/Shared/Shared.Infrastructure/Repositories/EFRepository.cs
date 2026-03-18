using Microsoft.EntityFrameworkCore;
using Shared.Core.Domain;
using Shared.Core.Repositories;

namespace Shared.Infrastructure.Repositories;

/// <summary>
/// Generic repository implementation using Entity Framework Core
/// Provides CRUD operations for aggregate roots
/// </summary>
public class EFRepository<TAggregate, TId> : IRepository<TAggregate, TId>
    where TAggregate : class
    where TId : notnull
{
    protected readonly DbContext _dbContext;
    protected readonly DbSet<TAggregate> _dbSet;

    public EFRepository(DbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dbSet = dbContext.Set<TAggregate>();
    }

    public virtual async Task<TAggregate?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
    }

    public virtual async Task<IEnumerable<TAggregate>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    public virtual async Task<IEnumerable<TAggregate>> FindAsync(Func<TAggregate, bool> predicate, CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(_dbSet.AsEnumerable().Where(predicate));
    }

    public virtual async Task AddAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(aggregate, cancellationToken);
    }

    public virtual async Task UpdateAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(aggregate);
        await Task.CompletedTask;
    }

    public virtual async Task DeleteAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
    {
        _dbSet.Remove(aggregate);
        await Task.CompletedTask;
    }

    public virtual async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
