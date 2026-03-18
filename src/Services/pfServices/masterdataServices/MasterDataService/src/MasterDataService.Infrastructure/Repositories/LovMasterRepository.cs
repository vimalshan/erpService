using MasterDataService.Application.Interfaces;
using MasterDataService.Domain.Entities;
using MasterDataService.Domain.Interfaces;
using MasterDataService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MasterDataService.Infrastructure.Repositories;

public class LovMasterRepository : ILovMasterRepository
{
    private readonly MasterDataDbContext _context;

    public LovMasterRepository(MasterDataDbContext context) => _context = context;

    public async Task<LovMaster?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _context.LovMasters.FindAsync([id], cancellationToken);
    }

    public async Task<LovMaster?> GetByIdAsync(decimal lovId, CancellationToken cancellationToken = default)
    {
        return await _context.LovMasters.FindAsync([lovId], cancellationToken);
    }

    public async Task<IEnumerable<LovMaster>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.LovMasters.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<LovMaster>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default)
    {
        return await _context.LovMasters
            .Where(x => x.LovCategory == category && x.LovStatus == "A")
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<decimal> GetNextIdAsync(CancellationToken cancellationToken = default)
    {
        var maxId = await _context.LovMasters.MaxAsync(x => (decimal?)x.LovId, cancellationToken);
        return (maxId ?? 0) + 1;
    }

    public async Task AddAsync(LovMaster entity, CancellationToken cancellationToken = default)
    {
        await _context.LovMasters.AddAsync(entity, cancellationToken);
    }

    public void Update(LovMaster entity) => _context.LovMasters.Update(entity);
    public void Delete(LovMaster entity) => _context.LovMasters.Remove(entity);
}
