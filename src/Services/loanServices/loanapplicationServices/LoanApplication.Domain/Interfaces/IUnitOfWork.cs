namespace LoanApplication.Domain.Interfaces;

/// <summary>
/// Unit of Work interface for managing transactions and repositories
/// </summary>
public interface IUnitOfWork : IAsyncDisposable
{
    /// <summary>
    /// Loan Application repository
    /// </summary>
    ILoanApplicationRepository LoanApplications { get; }

    /// <summary>
    /// Begin transaction
    /// </summary>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Commit transaction
    /// </summary>
    Task CommitAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Rollback transaction
    /// </summary>
    Task RollbackAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Save all changes
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
