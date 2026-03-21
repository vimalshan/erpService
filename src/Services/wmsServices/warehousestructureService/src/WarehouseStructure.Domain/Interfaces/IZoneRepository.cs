using WarehouseStructure.Domain.Entities;

namespace WarehouseStructure.Domain.Interfaces;

public interface IZoneRepository
{
    Task<IEnumerable<Zone>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<Zone>> GetByWarehouseIdAsync(int warehouseId, CancellationToken ct = default);
    Task<Zone?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Zone> AddAsync(Zone zone, CancellationToken ct = default);
    Task UpdateAsync(Zone zone, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
}
