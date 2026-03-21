using UnitService.Domain.Entities;

namespace UnitService.Domain.Interfaces;

public interface IEquipmentRepository
{
    Task<EquipmentMaster?> GetByIdAsync(int equipmentId, CancellationToken ct = default);
    Task<IEnumerable<EquipmentMaster>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<EquipmentMaster>> GetByUnitCodeAsync(string unitCode, CancellationToken ct = default);
    Task AddAsync(EquipmentMaster equipment, CancellationToken ct = default);
    void Update(EquipmentMaster equipment);
    void Delete(EquipmentMaster equipment);
}
