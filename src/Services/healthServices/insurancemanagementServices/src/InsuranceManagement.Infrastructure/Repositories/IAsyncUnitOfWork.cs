namespace InsuranceManagement.Infrastructure.Repositories;

/// <summary>
/// Async Unit of Work pattern interface
/// </summary>
public interface IAsyncUnitOfWork : IDisposable, IAsyncDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
}
