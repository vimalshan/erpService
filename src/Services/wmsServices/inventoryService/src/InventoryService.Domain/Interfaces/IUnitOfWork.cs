namespace InventoryService.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IStockLevelRepository StockLevels { get; }
    IInventoryTransactionRepository InventoryTransactions { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
