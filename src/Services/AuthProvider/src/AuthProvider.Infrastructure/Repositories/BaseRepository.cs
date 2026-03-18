using AuthProvider.Domain.Interfaces;
using AuthProvider.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthProvider.Infrastructure.Repositories;

/// <summary>Generic EF Core repository base implementation (Repository pattern).</summary>
public abstract class BaseRepository<T> : IRepository<T> where T : class
{
    protected readonly AuthDbContext Context;
    protected readonly DbSet<T> DbSet;

    protected BaseRepository(AuthDbContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await DbSet.FindAsync(new object[] { id }, ct);

    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default) =>
        await DbSet.ToListAsync(ct);

    public virtual async Task AddAsync(T entity, CancellationToken ct = default) =>
        await DbSet.AddAsync(entity, ct);

    public virtual void Update(T entity) => DbSet.Update(entity);

    public virtual void Remove(T entity) => DbSet.Remove(entity);
}
