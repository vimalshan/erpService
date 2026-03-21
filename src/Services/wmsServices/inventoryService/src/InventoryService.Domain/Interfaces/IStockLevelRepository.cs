using InventoryService.Domain.Entities;

namespace InventoryService.Domain.Interfaces;

public interface IStockLevelRepository
{
    Task<StockLevel?> GetByIdAsync(long stockLevelId, CancellationToken ct = default);
    Task<StockLevel?> GetByProductAndBinAsync(int productId, int binId, CancellationToken ct = default);
    Task<IEnumerable<StockLevel>> GetByWarehouseAsync(int warehouseId, CancellationToken ct = default);
    Task<IEnumerable<StockLevel>> GetByProductAsync(int productId, CancellationToken ct = default);
    Task<IEnumerable<StockLevel>> GetLowStockItemsAsync(CancellationToken ct = default);
    Task AddAsync(StockLevel stockLevel, CancellationToken ct = default);
    Task UpdateAsync(StockLevel stockLevel, CancellationToken ct = default);
    Task<decimal> GetAvailableStockAsync(int productId, int? warehouseId = null, int? binId = null, CancellationToken ct = default);
}
