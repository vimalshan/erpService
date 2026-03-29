using Microsoft.EntityFrameworkCore;
using Masters.Application.Interfaces;
using Masters.Domain.Entities;

namespace Masters.Infrastructure.Persistence.Repositories;

public class LovMasterRepository : ILovMasterRepository
{
    private readonly MastersDbContext _context;

    public LovMasterRepository(MastersDbContext context)
    {
        _context = context;
    }

    public async Task<LovMaster?> GetByIdAsync(long lovId, CancellationToken cancellationToken = default)
    {
        return await _context.LovMasters
            .FirstOrDefaultAsync(x => x.LovId == lovId, cancellationToken);
    }

    public async Task<IEnumerable<LovMaster>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.LovMasters.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<LovMaster>> GetByTypeAsync(string lovType, CancellationToken cancellationToken = default)
    {
        var typeKey = Domain.ValueObjects.LovTypeCode.Create(lovType);
        return await _context.LovMasters
            .Where(x => x.LovType == typeKey)
            .ToListAsync(cancellationToken);
    }

    public async Task<LovMaster> AddAsync(LovMaster entity, CancellationToken cancellationToken = default)
    {
        await _context.LovMasters.AddAsync(entity, cancellationToken);
        return entity;
    }

    public Task UpdateAsync(LovMaster entity, CancellationToken cancellationToken = default)
    {
        _context.LovMasters.Update(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long lovId, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(lovId, cancellationToken);
        if (entity != null)
        {
            _context.LovMasters.Remove(entity);
        }
    }

    public async Task<bool> ExistsAsync(long lovId, CancellationToken cancellationToken = default)
    {
        return await _context.LovMasters.AnyAsync(x => x.LovId == lovId, cancellationToken);
    }
}
