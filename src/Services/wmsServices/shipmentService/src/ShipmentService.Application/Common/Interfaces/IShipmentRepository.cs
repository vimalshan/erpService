using ShipmentService.Domain.Entities;
using ShipmentService.Domain.Enums;

namespace ShipmentService.Application.Common.Interfaces;

public interface IShipmentRepository
{
    Task<Shipment?> GetByIdAsync(int shipmentId, CancellationToken cancellationToken = default);
    Task<Shipment?> GetByNumberAsync(string shipmentNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<Shipment>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Shipment>> GetByStatusAsync(ShipmentStatus status, CancellationToken cancellationToken = default);
    Task<IEnumerable<Shipment>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);
    Task<Shipment> AddAsync(Shipment shipment, CancellationToken cancellationToken = default);
    Task UpdateAsync(Shipment shipment, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string shipmentNumber, CancellationToken cancellationToken = default);
}
