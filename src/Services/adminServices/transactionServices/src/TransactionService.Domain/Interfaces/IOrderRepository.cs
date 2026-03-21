namespace TransactionService.Domain.Interfaces;

using TransactionService.Domain.Entities;

public interface IOrderRepository : IRepository<OrderMain>
{
    Task<OrderMain?> GetByIdWithDetailsAsync(long orderMainId, CancellationToken ct = default);
    Task<IEnumerable<OrderMain>> GetByLocationAsync(long locationId, CancellationToken ct = default);
    Task<IEnumerable<OrderMain>> GetByVendorAsync(long vendorId, CancellationToken ct = default);
    Task<long> GetNextOrderMainIdAsync(CancellationToken ct = default);
    Task<long> GetNextOrderSubIdAsync(CancellationToken ct = default);
}
