namespace MasterDataService.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ILovMasterRepository LovMasters { get; }
    ILovTypeMasterRepository LovTypeMasters { get; }
    IHoldTypeMasterRepository HoldTypeMasters { get; }
    ILocationScanParamRepository LocationScanParams { get; }
    IScannerMasterRepository ScannerMasters { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
