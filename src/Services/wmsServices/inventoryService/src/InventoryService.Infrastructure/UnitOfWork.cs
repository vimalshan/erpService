using InventoryService.Domain.Interfaces;
using InventoryService.Infrastructure.Persistence;
using InventoryService.Infrastructure.Repositories;

namespace InventoryService.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly InventoryDbContext _context;
    private IStockLevelRepository? _stockLevels;
    private IInventoryTransactionRepository? _inventoryTransactions;

    public UnitOfWork(InventoryDbContext context)
    {
        _context = context;
    }

    public IStockLevelRepository StockLevels =>
        _stockLevels ??= new StockLevelRepository(_context);

    public IInventoryTransactionRepository InventoryTransactions =>
        _inventoryTransactions ??= new InventoryTransactionRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
