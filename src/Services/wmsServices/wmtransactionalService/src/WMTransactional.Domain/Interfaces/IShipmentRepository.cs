using WMTransactional.Domain.Entities;

namespace WMTransactional.Domain.Interfaces;

public interface IShipmentRepository
{
    Task<Shipment?> GetByIdAsync(int shipmentId, CancellationToken ct = default);
    Task<Shipment?> GetByNumberAsync(string shipmentNumber, CancellationToken ct = default);
    Task<IEnumerable<Shipment>> GetBySalesOrderAsync(int soId, CancellationToken ct = default);
    Task<IEnumerable<Shipment>> GetByStatusAsync(string status, CancellationToken ct = default);
    Task<IEnumerable<Shipment>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(Shipment shipment, CancellationToken ct = default);
    Task UpdateAsync(Shipment shipment, CancellationToken ct = default);
}
