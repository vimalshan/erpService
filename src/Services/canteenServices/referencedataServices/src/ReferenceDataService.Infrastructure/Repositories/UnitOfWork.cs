using ReferenceDataService.Domain.Interfaces;
using ReferenceDataService.Infrastructure.Persistence;

namespace ReferenceDataService.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ReferenceDataDbContext _context;

    public ILovMasterRepository LovMasters { get; }
    public ILovTypeMasterRepository LovTypeMasters { get; }
    public IPathToSqlServerRepository PathToSqlServers { get; }

    public UnitOfWork(
        ReferenceDataDbContext context,
        ILovMasterRepository lovMasters,
        ILovTypeMasterRepository lovTypeMasters,
        IPathToSqlServerRepository pathToSqlServers)
    {
        _context = context;
        LovMasters = lovMasters;
        LovTypeMasters = lovTypeMasters;
        PathToSqlServers = pathToSqlServers;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
