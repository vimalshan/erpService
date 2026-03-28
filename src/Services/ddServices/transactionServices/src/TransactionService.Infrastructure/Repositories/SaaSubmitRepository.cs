using Microsoft.EntityFrameworkCore;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Infrastructure.Repositories;

public class SaaSubmitRepository : ISaaSubmitRepository
{
    private readonly Data.TransactionDbContext _context;

    public SaaSubmitRepository(Data.TransactionDbContext context)
    {
        _context = context;
    }

    public async Task<SaaSubmit?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.SaaSubmits
            .Include(s => s.Period)
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<SaaSubmit>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaaSubmits
            .Include(s => s.Period)
            .Where(a => !a.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SaaSubmit>> GetByPeriodAsync(long periodId, CancellationToken cancellationToken = default)
    {
        return await _context.SaaSubmits
            .Include(s => s.Period)
            .Where(a => a.PeriodId == periodId && !a.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<SaaSubmit?> GetByPeriodAndBusinessAsync(long periodId, long busId, CancellationToken cancellationToken = default)
    {
        return await _context.SaaSubmits
            .Include(s => s.Period)
            .FirstOrDefaultAsync(a => a.PeriodId == periodId && a.BusId == busId && !a.IsDeleted, cancellationToken);
    }

    public async Task<SaaSubmit> AddAsync(SaaSubmit entity, CancellationToken cancellationToken = default)
    {
        await _context.SaaSubmits.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(SaaSubmit entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.SaaSubmits.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
            _context.SaaSubmits.Update(entity);
        }
    }
}
