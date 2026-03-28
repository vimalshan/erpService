namespace AuthorizationService.Domain.Interfaces;

/// <summary>
/// Unit of Work interface for transaction management
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IRepository<Entities.Right> Rights { get; }
    IRepository<Entities.SpecialInput> SpecialInputs { get; }
    IRepository<Entities.SpecialInputMaster> SpecialInputMasters { get; }
    ITrackerRightRepository TrackerRights { get; }
    IUserRightRepository UserRights { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync();
}
