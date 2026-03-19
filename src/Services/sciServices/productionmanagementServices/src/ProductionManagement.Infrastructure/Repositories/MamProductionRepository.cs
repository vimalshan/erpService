using Microsoft.EntityFrameworkCore;
using ProductionManagement.Domain.Entities;
using ProductionManagement.Domain.Interfaces;
using ProductionManagement.Infrastructure.Persistence;

namespace ProductionManagement.Infrastructure.Repositories;

public class MamProductionRepository : IMamProductionRepository
{
    private readonly ProductionManagementDbContext _context;

    public MamProductionRepository(ProductionManagementDbContext context) => _context = context;

    public async Task<IReadOnlyList<MamProductionDet>> GetProductionDetailsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.MamProductionDets.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<MamProductionDet>> GetProductionDetailsByFgAsync(int fgCode, CancellationToken cancellationToken = default)
    {
        return await _context.MamProductionDets
            .Where(d => d.ProductionFg == fgCode)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<MamProductionDet> AddDetailAsync(MamProductionDet detail, CancellationToken cancellationToken = default)
    {
        await _context.MamProductionDets.AddAsync(detail, cancellationToken);
        return detail;
    }

    public async Task<IReadOnlyList<MamProductionMap>> GetProductionMapsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.MamProductionMaps.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<MamProductionMap> AddMapAsync(MamProductionMap map, CancellationToken cancellationToken = default)
    {
        await _context.MamProductionMaps.AddAsync(map, cancellationToken);
        return map;
    }
}
