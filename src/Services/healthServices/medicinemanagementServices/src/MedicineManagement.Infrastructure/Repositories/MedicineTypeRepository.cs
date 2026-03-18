using MedicineManagement.Domain.Entities;
using MedicineManagement.Domain.Interfaces;
using MedicineManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedicineManagement.Infrastructure.Repositories;

public class MedicineTypeRepository(MedicineManagementDbContext context) : IMedicineTypeRepository
{
    public async Task<MedicineType?> GetByCodeAsync(string typeCode, CancellationToken ct = default)
        => await context.MedicineTypes.FirstOrDefaultAsync(t => t.TypeCode == typeCode, ct);

    public async Task<IReadOnlyList<MedicineType>> GetAllAsync(CancellationToken ct = default)
        => await context.MedicineTypes.ToListAsync(ct);

    public async Task AddAsync(MedicineType entity, CancellationToken ct = default)
        => await context.MedicineTypes.AddAsync(entity, ct);

    public Task UpdateAsync(MedicineType entity, CancellationToken ct = default)
    {
        context.MedicineTypes.Update(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(string typeCode, CancellationToken ct = default)
    {
        var entity = await GetByCodeAsync(typeCode, ct);
        if (entity is not null) context.MedicineTypes.Remove(entity);
    }
}
