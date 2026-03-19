using MasterDataService.Domain.Common;
using MasterDataService.Domain.Interfaces;
using MasterDataService.Infrastructure.Persistence.EfCore;
using MediatR;

namespace MasterDataService.Infrastructure.Repositories;

public class UnitOfWork(MasterDataDbContext db, IMediator mediator) : IUnitOfWork
{
    private ILovMasterRepository? _lovMasters;
    private ILovTypeMasterRepository? _lovTypeMasters;
    private IHoldTypeMasterRepository? _holdTypeMasters;
    private ILocationScanParamRepository? _locationScanParams;
    private IScannerMasterRepository? _scannerMasters;

    public ILovMasterRepository LovMasters => _lovMasters ??= new LovMasterRepository(db);
    public ILovTypeMasterRepository LovTypeMasters => _lovTypeMasters ??= new LovTypeMasterRepository(db);
    public IHoldTypeMasterRepository HoldTypeMasters => _holdTypeMasters ??= new HoldTypeMasterRepository(db);
    public ILocationScanParamRepository LocationScanParams => _locationScanParams ??= new LocationScanParamRepository(db);
    public IScannerMasterRepository ScannerMasters => _scannerMasters ??= new ScannerMasterRepository(db);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        // Dispatch domain events before saving
        var domainEntities = db.ChangeTracker
            .Entries<BaseEntity<long>>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = domainEntities.SelectMany(e => e.DomainEvents).ToList();
        domainEntities.ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent, ct);
        }

        return await db.SaveChangesAsync(ct);
    }

    public void Dispose() => db.Dispose();
}
