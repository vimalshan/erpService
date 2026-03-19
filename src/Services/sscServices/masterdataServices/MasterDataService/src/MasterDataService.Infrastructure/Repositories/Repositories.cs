using Microsoft.EntityFrameworkCore;
using MasterDataService.Domain.Entities;
using MasterDataService.Domain.Interfaces;
using MasterDataService.Infrastructure.Persistence.EfCore;

namespace MasterDataService.Infrastructure.Repositories;

public class LovMasterRepository(MasterDataDbContext db) : ILovMasterRepository
{
    public async Task<LovMaster?> GetByIdAsync(long id, CancellationToken ct = default)
        => await db.LovMasters.FindAsync([id], ct);

    public async Task<IReadOnlyList<LovMaster>> GetAllAsync(CancellationToken ct = default)
        => await db.LovMasters.ToListAsync(ct);

    public async Task<IReadOnlyList<LovMaster>> GetByTypeAsync(string lovType, CancellationToken ct = default)
        => await db.LovMasters.Where(l => l.LovType == lovType).ToListAsync(ct);

    public async Task AddAsync(LovMaster entity, CancellationToken ct = default)
        => await db.LovMasters.AddAsync(entity, ct);

    public void Update(LovMaster entity) => db.LovMasters.Update(entity);
    public void Delete(LovMaster entity) => db.LovMasters.Remove(entity);
}

public class LovTypeMasterRepository(MasterDataDbContext db) : ILovTypeMasterRepository
{
    public async Task<LovTypeMaster?> GetByIdAsync(string typeCode, CancellationToken ct = default)
        => await db.LovTypeMasters.FindAsync([typeCode], ct);

    public async Task<IReadOnlyList<LovTypeMaster>> GetAllAsync(CancellationToken ct = default)
        => await db.LovTypeMasters.ToListAsync(ct);

    public async Task AddAsync(LovTypeMaster entity, CancellationToken ct = default)
        => await db.LovTypeMasters.AddAsync(entity, ct);

    public void Update(LovTypeMaster entity) => db.LovTypeMasters.Update(entity);
    public void Delete(LovTypeMaster entity) => db.LovTypeMasters.Remove(entity);
}

public class HoldTypeMasterRepository(MasterDataDbContext db) : IHoldTypeMasterRepository
{
    public async Task<HoldTypeMaster?> GetByIdAsync(long id, CancellationToken ct = default)
        => await db.HoldTypeMasters.FindAsync([id], ct);

    public async Task<IReadOnlyList<HoldTypeMaster>> GetAllAsync(CancellationToken ct = default)
        => await db.HoldTypeMasters.ToListAsync(ct);

    public async Task AddAsync(HoldTypeMaster entity, CancellationToken ct = default)
        => await db.HoldTypeMasters.AddAsync(entity, ct);

    public void Update(HoldTypeMaster entity) => db.HoldTypeMasters.Update(entity);
    public void Delete(HoldTypeMaster entity) => db.HoldTypeMasters.Remove(entity);
}

public class LocationScanParamRepository(MasterDataDbContext db) : ILocationScanParamRepository
{
    public async Task<LocationScanParam?> GetByIdAsync(long id, CancellationToken ct = default)
        => await db.LocationScanParams.FindAsync([id], ct);

    public async Task<IReadOnlyList<LocationScanParam>> GetByLocationIdAsync(long locationId, CancellationToken ct = default)
        => await db.LocationScanParams.Where(l => l.LocationId == locationId).ToListAsync(ct);

    public async Task<IReadOnlyList<LocationScanParam>> GetAllAsync(CancellationToken ct = default)
        => await db.LocationScanParams.ToListAsync(ct);

    public async Task AddAsync(LocationScanParam entity, CancellationToken ct = default)
        => await db.LocationScanParams.AddAsync(entity, ct);

    public void Update(LocationScanParam entity) => db.LocationScanParams.Update(entity);
    public void Delete(LocationScanParam entity) => db.LocationScanParams.Remove(entity);
}

public class ScannerMasterRepository(MasterDataDbContext db) : IScannerMasterRepository
{
    public async Task<ScannerMaster?> GetByIdAsync(long id, CancellationToken ct = default)
        => await db.ScannerMasters.FindAsync([id], ct);

    public async Task<IReadOnlyList<ScannerMaster>> GetAllAsync(CancellationToken ct = default)
        => await db.ScannerMasters.ToListAsync(ct);

    public async Task<IReadOnlyList<ScannerMaster>> GetByLocationIdAsync(long locationId, CancellationToken ct = default)
        => await db.ScannerMasters.Where(s => s.DeviceLocationId == locationId).ToListAsync(ct);

    public async Task AddAsync(ScannerMaster entity, CancellationToken ct = default)
        => await db.ScannerMasters.AddAsync(entity, ct);

    public void Update(ScannerMaster entity) => db.ScannerMasters.Update(entity);
    public void Delete(ScannerMaster entity) => db.ScannerMasters.Remove(entity);
}
