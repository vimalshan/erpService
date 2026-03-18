using LoanManagement.Domain.Entities;

namespace LoanManagement.Domain.Interfaces;

public interface IDisbursementRepository
{
    Task<LoanDisbursementSchedule?> GetByIdAsync(long disbId, CancellationToken cancellationToken = default);
    Task<IEnumerable<LoanDisbursementSchedule>> GetByLoanIdAsync(decimal loanId, CancellationToken cancellationToken = default);
    Task AddAsync(LoanDisbursementSchedule disbursement, CancellationToken cancellationToken = default);
    Task UpdateAsync(LoanDisbursementSchedule disbursement, CancellationToken cancellationToken = default);
    Task<long> GetNextIdAsync(CancellationToken cancellationToken = default);
}
