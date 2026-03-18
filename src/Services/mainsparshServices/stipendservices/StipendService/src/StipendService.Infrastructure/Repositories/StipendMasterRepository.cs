using Microsoft.EntityFrameworkCore;
using StipendService.Domain.Entities;
using StipendService.Domain.Interfaces;
using StipendService.Infrastructure.Persistence;

namespace StipendService.Infrastructure.Repositories;

public class StipendMasterRepository : IStipendMasterRepository
{
    private readonly StipendDbContext _context;

    public StipendMasterRepository(StipendDbContext context) => _context = context;

    public async Task<StipendMaster?> GetByIdAsync(long id, CancellationToken cancellationToken = default) =>
        await _context.StipendMasters.FindAsync([id], cancellationToken);

    public async Task<IEnumerable<StipendMaster>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _context.StipendMasters.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<StipendMaster?> GetActiveByCategory(long categoryId, long rankId, CancellationToken cancellationToken = default) =>
        await _context.StipendMasters
            .Where(m => m.ResearchCategoryId == categoryId
                     && m.SrfRankId == rankId
                     && m.Status == "A"
                     && m.EffectiveFrom <= DateTime.UtcNow
                     && (m.EffectiveTo == null || m.EffectiveTo >= DateTime.UtcNow))
            .FirstOrDefaultAsync(cancellationToken);

    public async Task AddAsync(StipendMaster stipendMaster, CancellationToken cancellationToken = default) =>
        await _context.StipendMasters.AddAsync(stipendMaster, cancellationToken);

    public Task UpdateAsync(StipendMaster stipendMaster, CancellationToken cancellationToken = default)
    {
        _context.StipendMasters.Update(stipendMaster);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(long categoryId, long rankId, CancellationToken cancellationToken = default) =>
        await _context.StipendMasters
            .AnyAsync(m => m.ResearchCategoryId == categoryId && m.SrfRankId == rankId, cancellationToken);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await _context.SaveChangesAsync(cancellationToken);
}
