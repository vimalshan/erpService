using MedicineManagement.Domain.Entities;
using MedicineManagement.Domain.Interfaces;
using MedicineManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedicineManagement.Infrastructure.Repositories;

public class MedicineRepository(MedicineManagementDbContext context) : IMedicineRepository
{
    public async Task<Medicine?> GetByCodeAsync(string medicineCode, CancellationToken ct = default)
        => await context.Medicines.Include(m => m.MedicineType)
            .FirstOrDefaultAsync(m => m.MedicineCode == medicineCode, ct);

    public async Task<IReadOnlyList<Medicine>> GetAllAsync(CancellationToken ct = default)
        => await context.Medicines.Include(m => m.MedicineType).ToListAsync(ct);

    public async Task<IReadOnlyList<Medicine>> SearchByNameAsync(string name, CancellationToken ct = default)
        => await context.Medicines.Where(m => m.MedicineName.Contains(name)).ToListAsync(ct);

    public async Task AddAsync(Medicine entity, CancellationToken ct = default)
        => await context.Medicines.AddAsync(entity, ct);

    public Task UpdateAsync(Medicine entity, CancellationToken ct = default)
    {
        context.Medicines.Update(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(string medicineCode, CancellationToken ct = default)
    {
        var entity = await GetByCodeAsync(medicineCode, ct);
        if (entity is not null) context.Medicines.Remove(entity);
    }

    public async Task<IReadOnlyList<Medicine>> GetLowStockMedicinesAsync(CancellationToken ct = default)
        => await context.Medicines.Where(m => m.OrderLevelMin != null).ToListAsync(ct);
}
