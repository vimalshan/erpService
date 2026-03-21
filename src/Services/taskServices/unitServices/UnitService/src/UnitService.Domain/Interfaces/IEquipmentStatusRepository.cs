using UnitService.Domain.Entities;

namespace UnitService.Domain.Interfaces;

public interface IEquipmentStatusRepository
{
    Task<EquipmentStatus?> GetByIdAsync(int statusId, CancellationToken ct = default);
    Task<IEnumerable<EquipmentStatus>> GetByEquipmentIdAsync(int equipmentId, CancellationToken ct = default);
    Task AddAsync(EquipmentStatus status, CancellationToken ct = default);
    void Update(EquipmentStatus status);
}
