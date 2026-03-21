using PurchaseOrderService.Application.DTOs;

namespace PurchaseOrderService.Application.Interfaces;

public interface IPurchaseOrderReadRepository
{
    Task<PurchaseOrderDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PurchaseOrderDto?> GetByPoNumberAsync(string poNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<PurchaseOrderSummaryDto>> GetAllAsync(int page, int pageSize, string? status = null, CancellationToken cancellationToken = default);
    Task<int> GetCountAsync(string? status = null, CancellationToken cancellationToken = default);
}
