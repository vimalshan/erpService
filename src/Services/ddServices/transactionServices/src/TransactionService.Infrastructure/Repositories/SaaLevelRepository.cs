using Microsoft.EntityFrameworkCore;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Infrastructure.Repositories;

public class SaaLevelRepository : ISaaLevelRepository
{
    private readonly Data.TransactionDbContext _context;

    public SaaLevelRepository(Data.TransactionDbContext context)
    {
        _context = context;
    }

    public async Task<SaaLevel?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.SaaLevels
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<SaaLevel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaaLevels
            .Where(a => !a.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SaaLevel>> GetActiveLevelsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaaLevels
            .Where(a => !a.IsDeleted && (a.LevelCloseDate == null || a.LevelCloseDate > DateTime.UtcNow))
            .ToListAsync(cancellationToken);
    }

    public async Task<SaaLevel> AddAsync(SaaLevel entity, CancellationToken cancellationToken = default)
    {
        await _context.SaaLevels.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(SaaLevel entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.SaaLevels.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
            _context.SaaLevels.Update(entity);
        }
    }
}
