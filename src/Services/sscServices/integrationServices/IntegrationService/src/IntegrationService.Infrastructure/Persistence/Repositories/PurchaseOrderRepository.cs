using IntegrationService.Domain.Entities;
using IntegrationService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IntegrationService.Infrastructure.Persistence.Repositories;

public class PurchaseOrderRepository(IntegrationDbContext context) : IPurchaseOrderRepository
{
    public async Task<PurchaseOrder?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        => await context.PurchaseOrders.FindAsync([id], cancellationToken);

    public async Task<IEnumerable<PurchaseOrder>> GetAllAsync(CancellationToken cancellationToken = default)
        => await context.PurchaseOrders.ToListAsync(cancellationToken);

    public async Task AddAsync(PurchaseOrder entity, CancellationToken cancellationToken = default)
        => await context.PurchaseOrders.AddAsync(entity, cancellationToken);

    public Task UpdateAsync(PurchaseOrder entity, CancellationToken cancellationToken = default)
    {
        context.PurchaseOrders.Update(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity is not null) context.PurchaseOrders.Remove(entity);
    }

    public async Task<PurchaseOrder?> GetByOraclePoIdAsync(long oraclePoId, CancellationToken cancellationToken = default)
        => await context.PurchaseOrders.FirstOrDefaultAsync(p => p.OraclePoId == oraclePoId, cancellationToken);

    public async Task<IEnumerable<PurchaseOrder>> GetByVendorSiteIdAsync(long vendorSiteId, CancellationToken cancellationToken = default)
        => await context.PurchaseOrders.Where(p => p.VendorSiteId == vendorSiteId).ToListAsync(cancellationToken);

    public async Task<PurchaseOrder?> GetWithMaterialReceiptsAsync(long id, CancellationToken cancellationToken = default)
        => await context.PurchaseOrders
            .Include(p => p.MaterialReceipts)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
}
