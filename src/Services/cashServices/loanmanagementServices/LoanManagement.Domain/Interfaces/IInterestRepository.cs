using LoanManagement.Domain.Entities;

namespace LoanManagement.Domain.Interfaces;

public interface IInterestRepository
{
    Task<LoanInterest?> GetByIdAsync(long intId, CancellationToken cancellationToken = default);
    Task<IEnumerable<LoanInterest>> GetByLoanIdAsync(decimal loanId, CancellationToken cancellationToken = default);
    Task AddAsync(LoanInterest interest, CancellationToken cancellationToken = default);
    Task<long> GetNextIdAsync(CancellationToken cancellationToken = default);
}
