using Microsoft.EntityFrameworkCore;
using WarehouseStructure.Domain.Entities;
using WarehouseStructure.Domain.Interfaces;
using WarehouseStructure.Infrastructure.Persistence;

namespace WarehouseStructure.Infrastructure.Repositories;

public class ZoneRepository : IZoneRepository
{
    private readonly WarehouseStructureDbContext _context;

    public ZoneRepository(WarehouseStructureDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Zone>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Zones.AsNoTracking().ToListAsync(ct);
    }

    public async Task<IEnumerable<Zone>> GetByWarehouseIdAsync(int warehouseId, CancellationToken ct = default)
    {
        return await _context.Zones
            .Where(z => z.WarehouseId == warehouseId)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<Zone?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Zones.FirstOrDefaultAsync(z => z.Id == id, ct);
    }

    public async Task<Zone> AddAsync(Zone zone, CancellationToken ct = default)
    {
        await _context.Zones.AddAsync(zone, ct);
        await _context.SaveChangesAsync(ct);
        return zone;
    }

    public async Task UpdateAsync(Zone zone, CancellationToken ct = default)
    {
        _context.Zones.Update(zone);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var zone = await _context.Zones.FindAsync(new object[] { id }, ct);
        if (zone is not null)
        {
            _context.Zones.Remove(zone);
            await _context.SaveChangesAsync(ct);
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
    {
        return await _context.Zones.AnyAsync(z => z.Id == id, ct);
    }
}
