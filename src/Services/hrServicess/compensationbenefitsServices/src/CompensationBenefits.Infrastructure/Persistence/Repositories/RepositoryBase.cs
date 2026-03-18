using CompensationBenefits.Domain.Interfaces;
using CompensationBenefits.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CompensationBenefits.Infrastructure.Persistence.Repositories;

public abstract class RepositoryBase<T>(CompensationBenefitsDbContext context)
    : IRepository<T> where T : class
{
    protected readonly CompensationBenefitsDbContext _context = context;

    public virtual async Task<T?> GetByIdAsync(long id, CancellationToken ct = default)
        => await _context.Set<T>().FindAsync([id], ct);

    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default)
        => await _context.Set<T>().ToListAsync(ct);

    public virtual async Task AddAsync(T entity, CancellationToken ct = default)
        => await _context.Set<T>().AddAsync(entity, ct);

    public virtual void Update(T entity)
        => _context.Set<T>().Update(entity);

    public virtual void Remove(T entity)
        => _context.Set<T>().Remove(entity);

    public virtual async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);
}
