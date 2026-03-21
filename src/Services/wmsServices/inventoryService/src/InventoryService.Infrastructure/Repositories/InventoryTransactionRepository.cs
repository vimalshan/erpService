using Microsoft.EntityFrameworkCore;
using InventoryService.Domain.Entities;
using InventoryService.Domain.Interfaces;
using InventoryService.Infrastructure.Persistence;

namespace InventoryService.Infrastructure.Repositories;

public class InventoryTransactionRepository : IInventoryTransactionRepository
{
    private readonly InventoryDbContext _context;

    public InventoryTransactionRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<InventoryTransaction?> GetByIdAsync(long transactionId, CancellationToken ct = default)
    {
        return await _context.InventoryTransactions.FindAsync([transactionId], ct);
    }

    public async Task<IEnumerable<InventoryTransaction>> GetByProductAsync(int productId, CancellationToken ct = default)
    {
        return await _context.InventoryTransactions
            .Where(t => t.ProductId == productId)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<InventoryTransaction>> GetByWarehouseAsync(int warehouseId, CancellationToken ct = default)
    {
        return await _context.InventoryTransactions
            .Where(t => t.WarehouseId == warehouseId)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<InventoryTransaction>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken ct = default)
    {
        return await _context.InventoryTransactions
            .Where(t => t.TransactionDate >= from && t.TransactionDate <= to)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync(ct);
    }

    public async Task AddAsync(InventoryTransaction transaction, CancellationToken ct = default)
    {
        await _context.InventoryTransactions.AddAsync(transaction, ct);
    }

    public async Task AddRangeAsync(IEnumerable<InventoryTransaction> transactions, CancellationToken ct = default)
    {
        await _context.InventoryTransactions.AddRangeAsync(transactions, ct);
    }
}
