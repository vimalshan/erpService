using Microsoft.EntityFrameworkCore;
using UtilityService.Domain.Entities;
using UtilityService.Domain.Interfaces;
using UtilityService.Infrastructure.Data;

namespace UtilityService.Infrastructure.Repositories;

public class ToadPlanSqlRepository : IToadPlanSqlRepository
{
    private readonly ApplicationDbContext _context;

    public ToadPlanSqlRepository(ApplicationDbContext context) => _context = context;

    public async Task<ToadPlanSql?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _context.ToadPlanSqls.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<ToadPlanSql?> GetByStatementIdAsync(string statementId, CancellationToken cancellationToken = default)
        => await _context.ToadPlanSqls.FirstOrDefaultAsync(
            x => x.StatementId.Value == statementId, cancellationToken);

    public async Task<IEnumerable<ToadPlanSql>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.ToadPlanSqls
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<ToadPlanSql>> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
        => await _context.ToadPlanSqls
            .Where(x => x.Username == username)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<ToadPlanSql>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        => await _context.ToadPlanSqls
            .OrderByDescending(x => x.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

    public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
        => await _context.ToadPlanSqls.CountAsync(cancellationToken);

    public async Task AddAsync(ToadPlanSql entity, CancellationToken cancellationToken = default)
    {
        await _context.ToadPlanSqls.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(ToadPlanSql entity, CancellationToken cancellationToken = default)
    {
        _context.ToadPlanSqls.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity is not null)
        {
            _context.ToadPlanSqls.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(string statementId, CancellationToken cancellationToken = default)
        => await _context.ToadPlanSqls.AnyAsync(
            x => x.StatementId.Value == statementId, cancellationToken);
}
