using Microsoft.EntityFrameworkCore;
using InventoryService.Domain.Entities;
using InventoryService.Domain.Interfaces;
using InventoryService.Infrastructure.Persistence;

namespace InventoryService.Infrastructure.Repositories;

public class StockLevelRepository : IStockLevelRepository
{
    private readonly InventoryDbContext _context;

    public StockLevelRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<StockLevel?> GetByIdAsync(long stockLevelId, CancellationToken ct = default)
    {
        return await _context.StockLevels.FindAsync([stockLevelId], ct);
    }

    public async Task<StockLevel?> GetByProductAndBinAsync(int productId, int binId, CancellationToken ct = default)
    {
        return await _context.StockLevels
            .FirstOrDefaultAsync(s => s.ProductId == productId && s.BinId == binId, ct);
    }

    public async Task<IEnumerable<StockLevel>> GetByWarehouseAsync(int warehouseId, CancellationToken ct = default)
    {
        return await _context.StockLevels
            .Where(s => s.WarehouseId == warehouseId)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<StockLevel>> GetByProductAsync(int productId, CancellationToken ct = default)
    {
        return await _context.StockLevels
            .Where(s => s.ProductId == productId)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<StockLevel>> GetLowStockItemsAsync(CancellationToken ct = default)
    {
        return await _context.StockLevels
            .Where(s => s.ReorderLevel != null &&
                        (s.QuantityOnHand - s.QuantityAllocated - s.QuantityReserved) <= s.ReorderLevel)
            .ToListAsync(ct);
    }

    public async Task AddAsync(StockLevel stockLevel, CancellationToken ct = default)
    {
        await _context.StockLevels.AddAsync(stockLevel, ct);
    }

    public Task UpdateAsync(StockLevel stockLevel, CancellationToken ct = default)
    {
        _context.StockLevels.Update(stockLevel);
        return Task.CompletedTask;
    }

    public async Task<decimal> GetAvailableStockAsync(int productId, int? warehouseId = null, int? binId = null, CancellationToken ct = default)
    {
        var query = _context.StockLevels.Where(s => s.ProductId == productId);

        if (binId.HasValue)
            query = query.Where(s => s.BinId == binId.Value);
        else if (warehouseId.HasValue)
            query = query.Where(s => s.WarehouseId == warehouseId.Value);

        return await query.SumAsync(s => s.QuantityOnHand - s.QuantityAllocated - s.QuantityReserved, ct);
    }
}
