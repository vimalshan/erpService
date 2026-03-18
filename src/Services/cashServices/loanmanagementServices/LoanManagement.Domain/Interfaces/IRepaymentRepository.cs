using LoanManagement.Domain.Entities;

namespace LoanManagement.Domain.Interfaces;

public interface IRepaymentRepository
{
    Task<LoanRepaymentSchedule?> GetByIdAsync(long repayId, CancellationToken cancellationToken = default);
    Task<IEnumerable<LoanRepaymentSchedule>> GetByLoanIdAsync(decimal loanId, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<LoanRepaymentSchedule> repayments, CancellationToken cancellationToken = default);
    Task UpdateAsync(LoanRepaymentSchedule repayment, CancellationToken cancellationToken = default);
    Task<long> GetNextIdAsync(CancellationToken cancellationToken = default);
}
