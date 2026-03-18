namespace MedicineManagement.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IMedicineTypeRepository MedicineTypes { get; }
    IMedicinePackagingRepository MedicinePackagings { get; }
    IMedicineRepository Medicines { get; }
    IDoctorAttendantRepository DoctorAttendants { get; }
    IMedicineCreditRepository MedicineCredits { get; }
    IMedicineIssueRepository MedicineIssues { get; }
    IPurchaseRepository Purchases { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
