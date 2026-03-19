using PurchaseSalesService.Domain.Entities;

namespace PurchaseSalesService.Domain.Interfaces;

public interface ISaleRepository
{
    Task<SaleMain?> GetByIdAsync(long serialNumber, CancellationToken ct = default);
    Task<IEnumerable<SaleMain>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<SaleMain>> GetByTrackingNumberAsync(long trackingNumber, CancellationToken ct = default);
    Task AddAsync(SaleMain sale, CancellationToken ct = default);
    Task UpdateAsync(SaleMain sale, CancellationToken ct = default);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
