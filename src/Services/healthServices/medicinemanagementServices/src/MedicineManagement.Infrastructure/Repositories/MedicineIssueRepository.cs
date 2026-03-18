using MedicineManagement.Domain.Entities;
using MedicineManagement.Domain.Interfaces;
using MedicineManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedicineManagement.Infrastructure.Repositories;

public class MedicineIssueRepository(MedicineManagementDbContext context) : IMedicineIssueRepository
{
    public async Task<IReadOnlyList<MedicineIssue>> GetByVisitNumberAsync(string visitNumber, CancellationToken ct = default)
        => await context.MedicineIssues.Where(i => i.VisitNumber == visitNumber).ToListAsync(ct);

    public async Task<IReadOnlyList<MedicineIssue>> GetByMedicineCodeAsync(string medicineCode, CancellationToken ct = default)
        => await context.MedicineIssues.Where(i => i.MedicineCode == medicineCode).ToListAsync(ct);

    public async Task AddAsync(MedicineIssue entity, CancellationToken ct = default)
        => await context.MedicineIssues.AddAsync(entity, ct);
}
