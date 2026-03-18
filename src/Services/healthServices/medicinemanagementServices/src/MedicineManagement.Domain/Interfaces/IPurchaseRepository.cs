using MedicineManagement.Domain.Entities;

namespace MedicineManagement.Domain.Interfaces;

public interface IPurchaseRepository
{
    Task<PurchaseMain?> GetByIdAsync(string companyCode, long transactionNumber, CancellationToken ct = default);
    Task<IReadOnlyList<PurchaseMain>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken ct = default);
    Task<IReadOnlyList<PurchaseMain>> GetByVendorAsync(string vendorName, CancellationToken ct = default);
    Task AddAsync(PurchaseMain entity, CancellationToken ct = default);
    Task UpdateAsync(PurchaseMain entity, CancellationToken ct = default);
}
