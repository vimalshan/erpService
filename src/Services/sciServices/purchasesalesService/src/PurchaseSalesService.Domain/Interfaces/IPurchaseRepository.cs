using PurchaseSalesService.Domain.Entities;

namespace PurchaseSalesService.Domain.Interfaces;

public interface IPurchaseRepository
{
    Task<PurchaseDetail?> GetByIdAsync(long serialNumber, CancellationToken ct = default);
    Task<IEnumerable<PurchaseDetail>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<PurchaseDetail>> GetByTrackingNumberAsync(long trackingNumber, CancellationToken ct = default);
    Task AddAsync(PurchaseDetail purchase, CancellationToken ct = default);
    Task UpdateAsync(PurchaseDetail purchase, CancellationToken ct = default);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
