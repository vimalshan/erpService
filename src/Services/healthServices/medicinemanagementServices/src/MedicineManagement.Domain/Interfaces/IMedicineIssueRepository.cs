using MedicineManagement.Domain.Entities;

namespace MedicineManagement.Domain.Interfaces;

public interface IMedicineIssueRepository
{
    Task<IReadOnlyList<MedicineIssue>> GetByVisitNumberAsync(string visitNumber, CancellationToken ct = default);
    Task<IReadOnlyList<MedicineIssue>> GetByMedicineCodeAsync(string medicineCode, CancellationToken ct = default);
    Task AddAsync(MedicineIssue entity, CancellationToken ct = default);
}
