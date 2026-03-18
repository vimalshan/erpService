using MedicineManagement.Domain.Entities;
using MedicineManagement.Domain.Interfaces;
using MedicineManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedicineManagement.Infrastructure.Repositories;

public class DoctorAttendantRepository(MedicineManagementDbContext context) : IDoctorAttendantRepository
{
    public async Task<DoctorAttendant?> GetByCodeAsync(string code, CancellationToken ct = default)
        => await context.DoctorAttendants.FirstOrDefaultAsync(d => d.Code == code, ct);

    public async Task<IReadOnlyList<DoctorAttendant>> GetAllAsync(CancellationToken ct = default)
        => await context.DoctorAttendants.ToListAsync(ct);

    public async Task<IReadOnlyList<DoctorAttendant>> GetDoctorsAsync(CancellationToken ct = default)
        => await context.DoctorAttendants.Where(d => d.Flag == 'D').ToListAsync(ct);

    public async Task<IReadOnlyList<DoctorAttendant>> GetAttendantsAsync(CancellationToken ct = default)
        => await context.DoctorAttendants.Where(d => d.Flag == 'A').ToListAsync(ct);

    public async Task AddAsync(DoctorAttendant entity, CancellationToken ct = default)
        => await context.DoctorAttendants.AddAsync(entity, ct);

    public Task UpdateAsync(DoctorAttendant entity, CancellationToken ct = default)
    {
        context.DoctorAttendants.Update(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(string code, CancellationToken ct = default)
    {
        var entity = await GetByCodeAsync(code, ct);
        if (entity is not null) context.DoctorAttendants.Remove(entity);
    }
}
