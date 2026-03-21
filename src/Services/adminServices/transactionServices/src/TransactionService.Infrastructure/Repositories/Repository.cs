namespace TransactionService.Infrastructure.Repositories;

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TransactionService.Domain.Interfaces;
using TransactionService.Infrastructure.Persistence;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly TransactionDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(TransactionDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(long id, CancellationToken ct = default)
        => await _dbSet.FindAsync([id], ct);

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default)
        => await _dbSet.ToListAsync(ct);

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        => await _dbSet.Where(predicate).ToListAsync(ct);

    public async Task AddAsync(T entity, CancellationToken ct = default)
        => await _dbSet.AddAsync(entity, ct);

    public void Update(T entity)
        => _dbSet.Attach(entity).State = EntityState.Modified;

    public void Remove(T entity)
        => _dbSet.Remove(entity);
}
