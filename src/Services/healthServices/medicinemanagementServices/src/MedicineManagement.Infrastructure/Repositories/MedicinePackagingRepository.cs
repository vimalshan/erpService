using MedicineManagement.Domain.Entities;
using MedicineManagement.Domain.Interfaces;
using MedicineManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedicineManagement.Infrastructure.Repositories;

public class MedicinePackagingRepository(MedicineManagementDbContext context) : IMedicinePackagingRepository
{
    public async Task<MedicinePackaging?> GetByCodeAsync(string packagingCode, CancellationToken ct = default)
        => await context.MedicinePackagings.FirstOrDefaultAsync(p => p.PackagingCode == packagingCode, ct);

    public async Task<IReadOnlyList<MedicinePackaging>> GetAllAsync(CancellationToken ct = default)
        => await context.MedicinePackagings.ToListAsync(ct);

    public async Task AddAsync(MedicinePackaging entity, CancellationToken ct = default)
        => await context.MedicinePackagings.AddAsync(entity, ct);

    public Task UpdateAsync(MedicinePackaging entity, CancellationToken ct = default)
    {
        context.MedicinePackagings.Update(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(string packagingCode, CancellationToken ct = default)
    {
        var entity = await GetByCodeAsync(packagingCode, ct);
        if (entity is not null) context.MedicinePackagings.Remove(entity);
    }
}
