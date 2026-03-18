using MedicineManagement.Domain.Entities;

namespace MedicineManagement.Domain.Interfaces;

public interface IMedicineTypeRepository
{
    Task<MedicineType?> GetByCodeAsync(string typeCode, CancellationToken ct = default);
    Task<IReadOnlyList<MedicineType>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(MedicineType entity, CancellationToken ct = default);
    Task UpdateAsync(MedicineType entity, CancellationToken ct = default);
    Task DeleteAsync(string typeCode, CancellationToken ct = default);
}
