using LoanService.Domain.Entities;

namespace LoanService.Domain.Interfaces;

public interface ILoanRepository
{
    Task<LoanMain?> GetByIdAsync(long loanNo, CancellationToken ct = default);
    Task<IReadOnlyList<LoanMain>> GetByMemberIdAsync(long memberId, CancellationToken ct = default);
    Task<IReadOnlyList<LoanMain>> GetActiveLoansAsync(CancellationToken ct = default);
    Task AddAsync(LoanMain loan, CancellationToken ct = default);
    void Update(LoanMain loan);
    Task<bool> ExistsAsync(long loanNo, CancellationToken ct = default);
}
