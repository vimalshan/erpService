using WMTransactional.Domain.Entities;

namespace WMTransactional.Domain.Interfaces;

public interface IPurchaseOrderRepository
{
    Task<PurchaseOrder?> GetByIdAsync(int poId, CancellationToken ct = default);
    Task<PurchaseOrder?> GetByNumberAsync(string poNumber, CancellationToken ct = default);
    Task<IEnumerable<PurchaseOrder>> GetBySupplierAsync(int supplierId, CancellationToken ct = default);
    Task<IEnumerable<PurchaseOrder>> GetByStatusAsync(string status, CancellationToken ct = default);
    Task<IEnumerable<PurchaseOrder>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(PurchaseOrder purchaseOrder, CancellationToken ct = default);
    Task UpdateAsync(PurchaseOrder purchaseOrder, CancellationToken ct = default);
}
