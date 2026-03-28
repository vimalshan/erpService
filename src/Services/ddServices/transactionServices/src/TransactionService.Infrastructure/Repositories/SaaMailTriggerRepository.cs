using Microsoft.EntityFrameworkCore;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Infrastructure.Repositories;

public class SaaMailTriggerRepository : ISaaMailTriggerRepository
{
    private readonly Data.TransactionDbContext _context;

    public SaaMailTriggerRepository(Data.TransactionDbContext context)
    {
        _context = context;
    }

    public async Task<SaaMailTrigger?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.SaaMailTriggers
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<SaaMailTrigger>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaaMailTriggers
            .Where(a => !a.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SaaMailTrigger>> GetByQuarterAsync(long quarterId, CancellationToken cancellationToken = default)
    {
        return await _context.SaaMailTriggers
            .Where(a => a.QuarterId == quarterId && !a.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<SaaMailTrigger> AddAsync(SaaMailTrigger entity, CancellationToken cancellationToken = default)
    {
        await _context.SaaMailTriggers.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(SaaMailTrigger entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.SaaMailTriggers.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
            _context.SaaMailTriggers.Update(entity);
        }
    }
}
