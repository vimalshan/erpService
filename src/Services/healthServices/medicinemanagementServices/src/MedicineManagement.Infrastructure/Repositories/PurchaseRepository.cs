using MedicineManagement.Domain.Entities;
using MedicineManagement.Domain.Interfaces;
using MedicineManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedicineManagement.Infrastructure.Repositories;

public class PurchaseRepository(MedicineManagementDbContext context) : IPurchaseRepository
{
    public async Task<PurchaseMain?> GetByIdAsync(string companyCode, long transactionNumber, CancellationToken ct = default)
        => await context.PurchaseMains.Include(p => p.LineItems)
            .FirstOrDefaultAsync(p => p.CompanyCode == companyCode && p.TransactionNumber == transactionNumber, ct);

    public async Task<IReadOnlyList<PurchaseMain>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken ct = default)
        => await context.PurchaseMains.Include(p => p.LineItems)
            .Where(p => p.InvoiceDate >= from && p.InvoiceDate <= to).ToListAsync(ct);

    public async Task<IReadOnlyList<PurchaseMain>> GetByVendorAsync(string vendorName, CancellationToken ct = default)
        => await context.PurchaseMains.Include(p => p.LineItems)
            .Where(p => p.VendorName.Contains(vendorName)).ToListAsync(ct);

    public async Task AddAsync(PurchaseMain entity, CancellationToken ct = default)
        => await context.PurchaseMains.AddAsync(entity, ct);

    public Task UpdateAsync(PurchaseMain entity, CancellationToken ct = default)
    {
        context.PurchaseMains.Update(entity);
        return Task.CompletedTask;
    }
}
