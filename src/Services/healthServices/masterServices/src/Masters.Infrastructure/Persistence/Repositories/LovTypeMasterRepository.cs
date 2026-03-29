using Microsoft.EntityFrameworkCore;
using Masters.Application.Interfaces;
using Masters.Domain.Entities;

namespace Masters.Infrastructure.Persistence.Repositories;

public class LovTypeMasterRepository : ILovTypeMasterRepository
{
    private readonly MastersDbContext _context;

    public LovTypeMasterRepository(MastersDbContext context)
    {
        _context = context;
    }

    public async Task<LovTypeMaster?> GetByIdAsync(string lovTypeCode, CancellationToken cancellationToken = default)
    {
        var key = Domain.ValueObjects.LovTypeCode.Create(lovTypeCode);
        return await _context.LovTypeMasters
            .Include(x => x.LovValues)
            .FirstOrDefaultAsync(x => x.LovTypeCode == key, cancellationToken);
    }

    public async Task<IEnumerable<LovTypeMaster>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.LovTypeMasters
            .Include(x => x.LovValues)
            .ToListAsync(cancellationToken);
    }

    public async Task<LovTypeMaster> AddAsync(LovTypeMaster entity, CancellationToken cancellationToken = default)
    {
        await _context.LovTypeMasters.AddAsync(entity, cancellationToken);
        return entity;
    }

    public Task UpdateAsync(LovTypeMaster entity, CancellationToken cancellationToken = default)
    {
        _context.LovTypeMasters.Update(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(string lovTypeCode, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(lovTypeCode, cancellationToken);
        if (entity != null)
        {
            _context.LovTypeMasters.Remove(entity);
        }
    }

    public async Task<bool> ExistsAsync(string lovTypeCode, CancellationToken cancellationToken = default)
    {
        var key = Domain.ValueObjects.LovTypeCode.Create(lovTypeCode);
        return await _context.LovTypeMasters
            .AnyAsync(x => x.LovTypeCode == key, cancellationToken);
    }
}
