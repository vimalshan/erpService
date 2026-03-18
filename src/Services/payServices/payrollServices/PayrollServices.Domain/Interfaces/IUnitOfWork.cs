namespace PayrollServices.Domain.Interfaces;

/// <summary>
/// Unit of work pattern interface
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IPayrollRepository PayrollRepository { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<bool> BeginTransactionAsync();
    Task<bool> CommitAsync();
    Task<bool> RollbackAsync();
}
