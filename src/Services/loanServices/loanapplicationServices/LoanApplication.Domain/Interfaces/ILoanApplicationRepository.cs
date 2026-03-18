using LoanApplication.Domain.Aggregates;

namespace LoanApplication.Domain.Interfaces;

/// <summary>
/// Repository interface for LoanApplication aggregate
/// </summary>
public interface ILoanApplicationRepository
{
    /// <summary>
    /// Get loan application by ID
    /// </summary>
    Task<LoanApplicationAggregate?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get loan applications by employee ID
    /// </summary>
    Task<IEnumerable<LoanApplicationAggregate>> GetByEmployeeIdAsync(long employeeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all loan applications
    /// </summary>
    Task<IEnumerable<LoanApplicationAggregate>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get pending applications (status = P or C)
    /// </summary>
    Task<IEnumerable<LoanApplicationAggregate>> GetPendingAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Add loan application
    /// </summary>
    Task AddAsync(LoanApplicationAggregate loanApplication, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update loan application
    /// </summary>
    Task UpdateAsync(LoanApplicationAggregate loanApplication, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete loan application (soft delete)
    /// </summary>
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Save changes to database
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
