using Microsoft.EntityFrameworkCore;
using PurchaseOrderService.Domain.Entities;
using PurchaseOrderService.Domain.Interfaces;
using PurchaseOrderService.Infrastructure.Persistence;

namespace PurchaseOrderService.Infrastructure.Repositories;

public class PurchaseOrderRepository : IPurchaseOrderRepository
{
    private readonly PurchaseOrderDbContext _context;

    public PurchaseOrderRepository(PurchaseOrderDbContext context)
    {
        _context = context;
    }

    public async Task<PurchaseOrder?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.PurchaseOrders
            .Include(po => po.Lines)
            .FirstOrDefaultAsync(po => po.Id == id, cancellationToken);
    }

    public async Task<PurchaseOrder?> GetByPoNumberAsync(string poNumber, CancellationToken cancellationToken = default)
    {
        return await _context.PurchaseOrders
            .Include(po => po.Lines)
            .FirstOrDefaultAsync(po => po.PoNumber == poNumber, cancellationToken);
    }

    public async Task<PurchaseOrder> AddAsync(PurchaseOrder purchaseOrder, CancellationToken cancellationToken = default)
    {
        var entry = await _context.PurchaseOrders.AddAsync(purchaseOrder, cancellationToken);
        return entry.Entity;
    }

    public Task UpdateAsync(PurchaseOrder purchaseOrder, CancellationToken cancellationToken = default)
    {
        _context.PurchaseOrders.Update(purchaseOrder);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(PurchaseOrder purchaseOrder, CancellationToken cancellationToken = default)
    {
        _context.PurchaseOrders.Remove(purchaseOrder);
        return Task.CompletedTask;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
