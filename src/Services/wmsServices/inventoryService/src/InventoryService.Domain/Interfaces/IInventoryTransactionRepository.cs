using InventoryService.Domain.Entities;

namespace InventoryService.Domain.Interfaces;

public interface IInventoryTransactionRepository
{
    Task<InventoryTransaction?> GetByIdAsync(long transactionId, CancellationToken ct = default);
    Task<IEnumerable<InventoryTransaction>> GetByProductAsync(int productId, CancellationToken ct = default);
    Task<IEnumerable<InventoryTransaction>> GetByWarehouseAsync(int warehouseId, CancellationToken ct = default);
    Task<IEnumerable<InventoryTransaction>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken ct = default);
    Task AddAsync(InventoryTransaction transaction, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<InventoryTransaction> transactions, CancellationToken ct = default);
}
