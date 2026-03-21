using Microsoft.EntityFrameworkCore;
using WarehouseStructure.Domain.Entities;
using WarehouseStructure.Domain.Interfaces;
using WarehouseStructure.Infrastructure.Persistence;

namespace WarehouseStructure.Infrastructure.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly WarehouseStructureDbContext _context;

    public WarehouseRepository(WarehouseStructureDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Warehouse>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Warehouses
            .Include(w => w.Zones)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<Warehouse?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Warehouses
            .Include(w => w.Zones)
            .FirstOrDefaultAsync(w => w.Id == id, ct);
    }

    public async Task<Warehouse?> GetByCodeAsync(string code, CancellationToken ct = default)
    {
        return await _context.Warehouses
            .Include(w => w.Zones)
            .FirstOrDefaultAsync(w => w.Code == code, ct);
    }

    public async Task<Warehouse> AddAsync(Warehouse warehouse, CancellationToken ct = default)
    {
        await _context.Warehouses.AddAsync(warehouse, ct);
        await _context.SaveChangesAsync(ct);
        return warehouse;
    }

    public async Task UpdateAsync(Warehouse warehouse, CancellationToken ct = default)
    {
        _context.Warehouses.Update(warehouse);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var warehouse = await _context.Warehouses.FindAsync(new object[] { id }, ct);
        if (warehouse is not null)
        {
            _context.Warehouses.Remove(warehouse);
            await _context.SaveChangesAsync(ct);
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
    {
        return await _context.Warehouses.AnyAsync(w => w.Id == id, ct);
    }
}
