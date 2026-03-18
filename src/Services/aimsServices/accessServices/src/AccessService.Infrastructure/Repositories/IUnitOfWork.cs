namespace AccessService.Infrastructure.Repositories;

/// <summary>
/// Unit of Work pattern for coordinating repository operations
/// </summary>
public interface IUnitOfWork : IAsyncDisposable
{
    IUserMapRepository UserMaps { get; }
    IUserRoleRepository UserRoles { get; }
    IMenuRepository Menus { get; }
    ISPARSHMenuRepository SPARSHMenus { get; }
    ISPARSHMenuAccessRepository SPARSHMenuAccess { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<bool> BeginTransactionAsync();
    Task<bool> CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
