using Microsoft.EntityFrameworkCore;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Infrastructure.Repositories;

public class SaaBudgetRepository : ISaaBudgetRepository
{
    private readonly Data.TransactionDbContext _context;

    public SaaBudgetRepository(Data.TransactionDbContext context)
    {
        _context = context;
    }

    public async Task<SaaBudget?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.SaaBudgets
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<SaaBudget>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaaBudgets
            .Where(a => !a.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SaaBudget>> GetByYearAsync(long yearId, CancellationToken cancellationToken = default)
    {
        return await _context.SaaBudgets
            .Where(a => a.YearId == yearId && !a.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<SaaBudget?> GetByBusinessAndYearAsync(long businessId, long yearId, CancellationToken cancellationToken = default)
    {
        return await _context.SaaBudgets
            .FirstOrDefaultAsync(a => a.BusinessId == businessId && a.YearId == yearId && !a.IsDeleted, cancellationToken);
    }

    public async Task<SaaBudget> AddAsync(SaaBudget entity, CancellationToken cancellationToken = default)
    {
        await _context.SaaBudgets.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(SaaBudget entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.SaaBudgets.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
            _context.SaaBudgets.Update(entity);
        }
    }
}
