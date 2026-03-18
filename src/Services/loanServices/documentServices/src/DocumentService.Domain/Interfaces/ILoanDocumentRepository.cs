using DocumentService.Domain.Entities;

namespace DocumentService.Domain.Interfaces;

public interface ILoanDocumentRepository
{
    Task<LoanDocument?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<LoanDocument>> GetByLoanIdAsync(long loanId, CancellationToken cancellationToken = default);
    Task<IEnumerable<LoanDocument>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(LoanDocument document, CancellationToken cancellationToken = default);
    Task UpdateAsync(LoanDocument document, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}
