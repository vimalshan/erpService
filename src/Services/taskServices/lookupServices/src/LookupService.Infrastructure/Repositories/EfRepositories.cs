using LookupService.Domain.Entities;
using LookupService.Domain.Interfaces;
using LookupService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LookupService.Infrastructure.Repositories;

public class LovMasterRepository(LookupDbContext db) : ILovMasterRepository
{
    public async Task<LovMaster?> GetByIdAsync(long lovId, CancellationToken ct = default)
        => await db.LovMasters.FirstOrDefaultAsync(x => x.LovId == lovId, ct);

    public async Task<IEnumerable<LovMaster>> GetAllAsync(CancellationToken ct = default)
        => await db.LovMasters.ToListAsync(ct);

    public async Task<IEnumerable<LovMaster>> GetByTypeAsync(string lovType, CancellationToken ct = default)
        => await db.LovMasters.Where(x => x.LovType == lovType).ToListAsync(ct);

    public async Task AddAsync(LovMaster entity, CancellationToken ct = default)
        => await db.LovMasters.AddAsync(entity, ct);

    public void Update(LovMaster entity) => db.LovMasters.Update(entity);
    public void Delete(LovMaster entity) => db.LovMasters.Remove(entity);
}

public class LovTypeMasterRepository(LookupDbContext db) : ILovTypeMasterRepository
{
    public async Task<LovTypeMaster?> GetByCodeAsync(string typeCode, CancellationToken ct = default)
        => await db.LovTypeMasters.FirstOrDefaultAsync(x => x.LovTypeCode == typeCode, ct);

    public async Task<IEnumerable<LovTypeMaster>> GetAllAsync(CancellationToken ct = default)
        => await db.LovTypeMasters.ToListAsync(ct);

    public async Task AddAsync(LovTypeMaster entity, CancellationToken ct = default)
        => await db.LovTypeMasters.AddAsync(entity, ct);

    public void Update(LovTypeMaster entity) => db.LovTypeMasters.Update(entity);
    public void Delete(LovTypeMaster entity) => db.LovTypeMasters.Remove(entity);
}

public class ProcessMasterRepository(LookupDbContext db) : IProcessMasterRepository
{
    public async Task<ProcessMaster?> GetByIdAsync(decimal processId, CancellationToken ct = default)
        => await db.ProcessMasters.FirstOrDefaultAsync(x => x.ProcessId == processId, ct);

    public async Task<IEnumerable<ProcessMaster>> GetAllAsync(CancellationToken ct = default)
        => await db.ProcessMasters.ToListAsync(ct);

    public async Task AddAsync(ProcessMaster entity, CancellationToken ct = default)
        => await db.ProcessMasters.AddAsync(entity, ct);

    public void Update(ProcessMaster entity) => db.ProcessMasters.Update(entity);
    public void Delete(ProcessMaster entity) => db.ProcessMasters.Remove(entity);
}

public class PanelMasterRepository(LookupDbContext db) : IPanelMasterRepository
{
    public async Task<PanelMaster?> GetByIdAsync(decimal panelId, CancellationToken ct = default)
        => await db.PanelMasters.FirstOrDefaultAsync(x => x.PanelId == panelId, ct);

    public async Task<IEnumerable<PanelMaster>> GetAllAsync(CancellationToken ct = default)
        => await db.PanelMasters.ToListAsync(ct);

    public async Task AddAsync(PanelMaster entity, CancellationToken ct = default)
        => await db.PanelMasters.AddAsync(entity, ct);

    public void Update(PanelMaster entity) => db.PanelMasters.Update(entity);
}

public class UnitProcessMapRepository(LookupDbContext db) : IUnitProcessMapRepository
{
    public async Task<IEnumerable<UnitProcessMap>> GetByUnitCodeAsync(string unitCode, CancellationToken ct = default)
        => await db.UnitProcessMaps.Where(x => x.UpUnitCode == unitCode).ToListAsync(ct);

    public async Task AddAsync(UnitProcessMap entity, CancellationToken ct = default)
        => await db.UnitProcessMaps.AddAsync(entity, ct);

    public void Delete(UnitProcessMap entity) => db.UnitProcessMaps.Remove(entity);
}

public class LovUnitMapRepository(LookupDbContext db) : ILovUnitMapRepository
{
    public async Task<IEnumerable<LovUnitMap>> GetByLovIdAsync(long lovId, CancellationToken ct = default)
        => await db.LovUnitMaps.Where(x => x.LuLovId == lovId).ToListAsync(ct);

    public async Task AddAsync(LovUnitMap entity, CancellationToken ct = default)
        => await db.LovUnitMaps.AddAsync(entity, ct);

    public void Delete(LovUnitMap entity) => db.LovUnitMaps.Remove(entity);
}

public class UnitLovAccessMasterRepository(LookupDbContext db) : IUnitLovAccessMasterRepository
{
    public async Task<UnitLovAccessMaster?> GetByIdAsync(decimal accessMastId, CancellationToken ct = default)
        => await db.UnitLovAccessMasters
            .Include(x => x.AccessDetails)
            .FirstOrDefaultAsync(x => x.UaAccessMastId == accessMastId, ct);

    public async Task<IEnumerable<UnitLovAccessMaster>> GetAllAsync(CancellationToken ct = default)
        => await db.UnitLovAccessMasters.ToListAsync(ct);

    public async Task AddAsync(UnitLovAccessMaster entity, CancellationToken ct = default)
        => await db.UnitLovAccessMasters.AddAsync(entity, ct);

    public void Update(UnitLovAccessMaster entity) => db.UnitLovAccessMasters.Update(entity);
}
