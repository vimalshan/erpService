using DeductionService.Domain.Entities;
using DeductionService.Domain.ValueObjects;

namespace DeductionService.Domain.Interfaces;

public interface IAdhocPayDeductionRepository
{
    Task<AdhocPayDeduction?> GetByIdAsync(long systemId, CancellationToken ct = default);
    Task<IEnumerable<AdhocPayDeduction>> GetByEmployeeAsync(long employeeNumber, CancellationToken ct = default);
    Task<IEnumerable<AdhocPayDeduction>> GetByMonthYearAsync(MonthYear period, CancellationToken ct = default);
    Task AddAsync(AdhocPayDeduction deduction, CancellationToken ct = default);
    Task UpdateAsync(AdhocPayDeduction deduction, CancellationToken ct = default);
    Task<IEnumerable<AdhocPayDeductionHistory>> GetHistoryByEmployeeAsync(long employeeNumber, CancellationToken ct = default);
}

public interface IDeductionAccessRepository
{
    Task<DeductionAccess?> GetByAccessNumberAsync(long accessNumber, CancellationToken ct = default);
    Task<IEnumerable<DeductionAccess>> GetActiveByUnitAsync(long unitCode, CancellationToken ct = default);
    Task AddAsync(DeductionAccess access, CancellationToken ct = default);
    Task UpdateAsync(DeductionAccess access, CancellationToken ct = default);
}

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
