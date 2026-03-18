using MedicineManagement.Domain.Entities;

namespace MedicineManagement.Domain.Interfaces;

public interface IMedicinePackagingRepository
{
    Task<MedicinePackaging?> GetByCodeAsync(string packagingCode, CancellationToken ct = default);
    Task<IReadOnlyList<MedicinePackaging>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(MedicinePackaging entity, CancellationToken ct = default);
    Task UpdateAsync(MedicinePackaging entity, CancellationToken ct = default);
    Task DeleteAsync(string packagingCode, CancellationToken ct = default);
}
