using MedicineManagement.Domain.Entities;

namespace MedicineManagement.Domain.Interfaces;

public interface IMedicineRepository
{
    Task<Medicine?> GetByCodeAsync(string medicineCode, CancellationToken ct = default);
    Task<IReadOnlyList<Medicine>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Medicine>> SearchByNameAsync(string name, CancellationToken ct = default);
    Task AddAsync(Medicine entity, CancellationToken ct = default);
    Task UpdateAsync(Medicine entity, CancellationToken ct = default);
    Task DeleteAsync(string medicineCode, CancellationToken ct = default);
    Task<IReadOnlyList<Medicine>> GetLowStockMedicinesAsync(CancellationToken ct = default);
}
