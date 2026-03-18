using MedicineManagement.Domain.Entities;

namespace MedicineManagement.Domain.Interfaces;

public interface IDoctorAttendantRepository
{
    Task<DoctorAttendant?> GetByCodeAsync(string code, CancellationToken ct = default);
    Task<IReadOnlyList<DoctorAttendant>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<DoctorAttendant>> GetDoctorsAsync(CancellationToken ct = default);
    Task<IReadOnlyList<DoctorAttendant>> GetAttendantsAsync(CancellationToken ct = default);
    Task AddAsync(DoctorAttendant entity, CancellationToken ct = default);
    Task UpdateAsync(DoctorAttendant entity, CancellationToken ct = default);
    Task DeleteAsync(string code, CancellationToken ct = default);
}
