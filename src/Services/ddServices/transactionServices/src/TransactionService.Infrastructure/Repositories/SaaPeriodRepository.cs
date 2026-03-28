using Microsoft.EntityFrameworkCore;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Infrastructure.Repositories;

public class SaaPeriodRepository : ISaaPeriodRepository
{
    private readonly Data.TransactionDbContext _context;

    public SaaPeriodRepository(Data.TransactionDbContext context)
    {
        _context = context;
    }

    public async Task<SaaPeriod?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.SaaPeriods
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<SaaPeriod>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaaPeriods
            .Where(a => !a.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SaaPeriod>> GetByYearAsync(long yearId, CancellationToken cancellationToken = default)
    {
        return await _context.SaaPeriods
            .Where(a => a.YearId == yearId && !a.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<SaaPeriod?> GetOpenPeriodAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaaPeriods
            .FirstOrDefaultAsync(a => a.Status == 'O' && !a.IsDeleted, cancellationToken);
    }

    public async Task<SaaPeriod> AddAsync(SaaPeriod entity, CancellationToken cancellationToken = default)
    {
        await _context.SaaPeriods.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(SaaPeriod entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.SaaPeriods.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
            _context.SaaPeriods.Update(entity);
        }
    }
}
