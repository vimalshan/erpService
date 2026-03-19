using MasterDataService.Domain.Entities;

namespace MasterDataService.Domain.Interfaces;

public interface ILovMasterRepository
{
    Task<LovMaster?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<LovMaster>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<LovMaster>> GetByTypeAsync(string lovType, CancellationToken ct = default);
    Task AddAsync(LovMaster entity, CancellationToken ct = default);
    void Update(LovMaster entity);
    void Delete(LovMaster entity);
}

public interface ILovTypeMasterRepository
{
    Task<LovTypeMaster?> GetByIdAsync(string typeCode, CancellationToken ct = default);
    Task<IReadOnlyList<LovTypeMaster>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(LovTypeMaster entity, CancellationToken ct = default);
    void Update(LovTypeMaster entity);
    void Delete(LovTypeMaster entity);
}

public interface IHoldTypeMasterRepository
{
    Task<HoldTypeMaster?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<HoldTypeMaster>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(HoldTypeMaster entity, CancellationToken ct = default);
    void Update(HoldTypeMaster entity);
    void Delete(HoldTypeMaster entity);
}

public interface ILocationScanParamRepository
{
    Task<LocationScanParam?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<LocationScanParam>> GetByLocationIdAsync(long locationId, CancellationToken ct = default);
    Task<IReadOnlyList<LocationScanParam>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(LocationScanParam entity, CancellationToken ct = default);
    void Update(LocationScanParam entity);
    void Delete(LocationScanParam entity);
}

public interface IScannerMasterRepository
{
    Task<ScannerMaster?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<ScannerMaster>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<ScannerMaster>> GetByLocationIdAsync(long locationId, CancellationToken ct = default);
    Task AddAsync(ScannerMaster entity, CancellationToken ct = default);
    void Update(ScannerMaster entity);
    void Delete(ScannerMaster entity);
}
