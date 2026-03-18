namespace LovService.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ILovTypeRepository LovTypes { get; }
    ILovMasterRepository LovMasters { get; }
    IItemDataRepository ItemData { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);
    Task BeginTransactionAsync(CancellationToken ct = default);
    Task CommitTransactionAsync(CancellationToken ct = default);
    Task RollbackTransactionAsync(CancellationToken ct = default);
}
