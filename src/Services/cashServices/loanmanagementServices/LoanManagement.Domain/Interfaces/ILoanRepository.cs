using LoanManagement.Domain.Entities;

namespace LoanManagement.Domain.Interfaces;

public interface ILoanRepository
{
    Task<LoanMain?> GetByIdAsync(decimal loanId, CancellationToken cancellationToken = default);
    Task<LoanMain?> GetByKeyAsync(string loanKey, CancellationToken cancellationToken = default);
    Task<IEnumerable<LoanMain>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<LoanMain>> GetByOrganizationAsync(decimal orgId, CancellationToken cancellationToken = default);
    Task AddAsync(LoanMain loan, CancellationToken cancellationToken = default);
    Task UpdateAsync(LoanMain loan, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(decimal loanId, CancellationToken cancellationToken = default);
    Task<decimal> GetNextIdAsync(CancellationToken cancellationToken = default);
}
