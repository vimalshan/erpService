namespace ReferenceDataService.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ILovMasterRepository LovMasters { get; }
    ILovTypeMasterRepository LovTypeMasters { get; }
    IPathToSqlServerRepository PathToSqlServers { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
