using Microsoft.EntityFrameworkCore;
using WMTransactional.Domain.Entities;
using WMTransactional.Domain.Interfaces;
using WMTransactional.Infrastructure.Persistence;

namespace WMTransactional.Infrastructure.Repositories;

public class PurchaseOrderRepository : IPurchaseOrderRepository
{
    private readonly WMTransactionalDbContext _context;

    public PurchaseOrderRepository(WMTransactionalDbContext context)
    {
        _context = context;
    }

    public async Task<PurchaseOrder?> GetByIdAsync(int poId, CancellationToken ct = default)
    {
        return await _context.PurchaseOrders
            .Include(p => p.Lines)
            .FirstOrDefaultAsync(p => p.PoId == poId, ct);
    }

    public async Task<PurchaseOrder?> GetByNumberAsync(string poNumber, CancellationToken ct = default)
    {
        return await _context.PurchaseOrders
            .Include(p => p.Lines)
            .FirstOrDefaultAsync(p => p.PoNumber == poNumber, ct);
    }

    public async Task<IEnumerable<PurchaseOrder>> GetBySupplierAsync(int supplierId, CancellationToken ct = default)
    {
        return await _context.PurchaseOrders
            .Include(p => p.Lines)
            .Where(p => p.SupplierId == supplierId)
            .OrderByDescending(p => p.CreatedDate)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<PurchaseOrder>> GetByStatusAsync(string status, CancellationToken ct = default)
    {
        return await _context.PurchaseOrders
            .Include(p => p.Lines)
            .Where(p => p.Status == status)
            .OrderByDescending(p => p.CreatedDate)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<PurchaseOrder>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.PurchaseOrders
            .Include(p => p.Lines)
            .OrderByDescending(p => p.CreatedDate)
            .ToListAsync(ct);
    }

    public async Task AddAsync(PurchaseOrder purchaseOrder, CancellationToken ct = default)
    {
        await _context.PurchaseOrders.AddAsync(purchaseOrder, ct);
    }

    public Task UpdateAsync(PurchaseOrder purchaseOrder, CancellationToken ct = default)
    {
        _context.PurchaseOrders.Update(purchaseOrder);
        return Task.CompletedTask;
    }
}
