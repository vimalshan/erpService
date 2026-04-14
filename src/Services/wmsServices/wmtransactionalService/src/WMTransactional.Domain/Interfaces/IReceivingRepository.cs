using WMTransactional.Domain.Entities;

namespace WMTransactional.Domain.Interfaces;

public interface IReceivingRepository
{
    Task<Receiving?> GetByIdAsync(int receivingId, CancellationToken ct = default);
    Task<Receiving?> GetByNumberAsync(string receivingNumber, CancellationToken ct = default);
    Task<IEnumerable<Receiving>> GetByPurchaseOrderAsync(int poId, CancellationToken ct = default);
    Task<IEnumerable<Receiving>> GetByStatusAsync(string status, CancellationToken ct = default);
    Task<IEnumerable<Receiving>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(Receiving receiving, CancellationToken ct = default);
    Task UpdateAsync(Receiving receiving, CancellationToken ct = default);
}
