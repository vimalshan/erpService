using MedicineManagement.Domain.Interfaces;
using MedicineManagement.Infrastructure.Persistence;

namespace MedicineManagement.Infrastructure.Repositories;

public class UnitOfWork(
    MedicineManagementDbContext context,
    IMedicineTypeRepository medicineTypes,
    IMedicinePackagingRepository medicinePackagings,
    IMedicineRepository medicines,
    IDoctorAttendantRepository doctorAttendants,
    IMedicineCreditRepository medicineCredits,
    IMedicineIssueRepository medicineIssues,
    IPurchaseRepository purchases) : IUnitOfWork
{
    public IMedicineTypeRepository MedicineTypes => medicineTypes;
    public IMedicinePackagingRepository MedicinePackagings => medicinePackagings;
    public IMedicineRepository Medicines => medicines;
    public IDoctorAttendantRepository DoctorAttendants => doctorAttendants;
    public IMedicineCreditRepository MedicineCredits => medicineCredits;
    public IMedicineIssueRepository MedicineIssues => medicineIssues;
    public IPurchaseRepository Purchases => purchases;

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await context.SaveChangesAsync(ct);

    public void Dispose()
    {
        context.Dispose();
        GC.SuppressFinalize(this);
    }
}
