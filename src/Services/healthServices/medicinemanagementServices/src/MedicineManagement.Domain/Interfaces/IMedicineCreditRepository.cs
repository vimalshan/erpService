using MedicineManagement.Domain.Entities;

namespace MedicineManagement.Domain.Interfaces;

public interface IMedicineCreditRepository
{
    Task<MedicineCredit?> GetByIdAsync(string companyCode, CancellationToken ct = default);
    Task<IReadOnlyList<MedicineCredit>> GetByMedicineCodeAsync(string medicineCode, CancellationToken ct = default);
    Task<IReadOnlyList<MedicineCredit>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken ct = default);
    Task<long> GetCurrentStockAsync(string medicineCode, CancellationToken ct = default);
    Task AddAsync(MedicineCredit entity, CancellationToken ct = default);
}
