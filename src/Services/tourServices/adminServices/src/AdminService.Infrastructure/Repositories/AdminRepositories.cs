using Microsoft.EntityFrameworkCore;
using AdminService.Domain.Entities;
using AdminService.Domain.Interfaces;
using AdminService.Infrastructure.Data;

namespace AdminService.Infrastructure.Repositories;

public class AdminMasterRepository : IAdminMasterRepository
{
    private readonly AdminDbContext _context;

    public AdminMasterRepository(AdminDbContext context) => _context = context;

    public async Task<AdminMaster?> GetByIdAsync(string adminId, CancellationToken ct = default)
        => await _context.AdminMasters
            .Include(a => a.UserMaps)
            .Include(a => a.AccessRights)
            .FirstOrDefaultAsync(a => a.AdminId == adminId, ct);

    public async Task<IReadOnlyList<AdminMaster>> GetAllAsync(CancellationToken ct = default)
        => await _context.AdminMasters.AsNoTracking().ToListAsync(ct);

    public async Task<AdminMaster> AddAsync(AdminMaster entity, CancellationToken ct = default)
    {
        await _context.AdminMasters.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(AdminMaster entity, CancellationToken ct = default)
    {
        _context.AdminMasters.Update(entity);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(string adminId, CancellationToken ct = default)
    {
        var entity = await _context.AdminMasters.FindAsync(new object[] { adminId }, ct);
        if (entity is not null)
        {
            _context.AdminMasters.Remove(entity);
            await _context.SaveChangesAsync(ct);
        }
    }
}

public class AdminUserMapRepository : IAdminUserMapRepository
{
    private readonly AdminDbContext _context;

    public AdminUserMapRepository(AdminDbContext context) => _context = context;

    public async Task<AdminUserMap?> GetByIdAsync(string mapId, CancellationToken ct = default)
        => await _context.AdminUserMaps.FirstOrDefaultAsync(x => x.AdminMapId == mapId, ct);

    public async Task<IReadOnlyList<AdminUserMap>> GetByAdminIdAsync(string adminId, CancellationToken ct = default)
        => await _context.AdminUserMaps.Where(x => x.AdminId == adminId).AsNoTracking().ToListAsync(ct);

    public async Task<IReadOnlyList<AdminUserMap>> GetAllAsync(CancellationToken ct = default)
        => await _context.AdminUserMaps.AsNoTracking().ToListAsync(ct);

    public async Task<AdminUserMap> AddAsync(AdminUserMap entity, CancellationToken ct = default)
    {
        await _context.AdminUserMaps.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(AdminUserMap entity, CancellationToken ct = default)
    {
        _context.AdminUserMaps.Update(entity);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(string mapId, CancellationToken ct = default)
    {
        var entity = await _context.AdminUserMaps.FindAsync(new object[] { mapId }, ct);
        if (entity is not null)
        {
            _context.AdminUserMaps.Remove(entity);
            await _context.SaveChangesAsync(ct);
        }
    }
}

public class AdminFinUserMapRepository : IAdminFinUserMapRepository
{
    private readonly AdminDbContext _context;

    public AdminFinUserMapRepository(AdminDbContext context) => _context = context;

    public async Task<AdminFinUserMap?> GetByIdAsync(string financeMapId, CancellationToken ct = default)
        => await _context.AdminFinUserMaps.FirstOrDefaultAsync(x => x.FinanceMapId == financeMapId, ct);

    public async Task<IReadOnlyList<AdminFinUserMap>> GetAllAsync(CancellationToken ct = default)
        => await _context.AdminFinUserMaps.AsNoTracking().ToListAsync(ct);

    public async Task<AdminFinUserMap> AddAsync(AdminFinUserMap entity, CancellationToken ct = default)
    {
        await _context.AdminFinUserMaps.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(AdminFinUserMap entity, CancellationToken ct = default)
    {
        _context.AdminFinUserMaps.Update(entity);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(string financeMapId, CancellationToken ct = default)
    {
        var entity = await _context.AdminFinUserMaps.FindAsync(new object[] { financeMapId }, ct);
        if (entity is not null)
        {
            _context.AdminFinUserMaps.Remove(entity);
            await _context.SaveChangesAsync(ct);
        }
    }
}

public class AdminAccessRightsRepository : IAdminAccessRightsRepository
{
    private readonly AdminDbContext _context;

    public AdminAccessRightsRepository(AdminDbContext context) => _context = context;

    public async Task<AdminAccessRights?> GetByIdAsync(string rightsId, CancellationToken ct = default)
        => await _context.AdminAccessRights
            .Include(x => x.AccessRightsLogs)
            .FirstOrDefaultAsync(x => x.AdminRightsId == rightsId, ct);

    public async Task<IReadOnlyList<AdminAccessRights>> GetByLocationIdAsync(string locationId, CancellationToken ct = default)
        => await _context.AdminAccessRights.Where(x => x.AdminLocationId == locationId).AsNoTracking().ToListAsync(ct);

    public async Task<IReadOnlyList<AdminAccessRights>> GetAllAsync(CancellationToken ct = default)
        => await _context.AdminAccessRights.AsNoTracking().ToListAsync(ct);

    public async Task<AdminAccessRights> AddAsync(AdminAccessRights entity, CancellationToken ct = default)
    {
        await _context.AdminAccessRights.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(AdminAccessRights entity, CancellationToken ct = default)
    {
        _context.AdminAccessRights.Update(entity);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(string rightsId, CancellationToken ct = default)
    {
        var entity = await _context.AdminAccessRights.FindAsync(new object[] { rightsId }, ct);
        if (entity is not null)
        {
            _context.AdminAccessRights.Remove(entity);
            await _context.SaveChangesAsync(ct);
        }
    }
}

public class AdminAccessRightsLogRepository : IAdminAccessRightsLogRepository
{
    private readonly AdminDbContext _context;

    public AdminAccessRightsLogRepository(AdminDbContext context) => _context = context;

    public async Task<IReadOnlyList<AdminAccessRightsLog>> GetByRightsIdAsync(string rightsId, CancellationToken ct = default)
        => await _context.AdminAccessRightsLogs.Where(x => x.AdminRightsId == rightsId).AsNoTracking().ToListAsync(ct);

    public async Task<AdminAccessRightsLog> AddAsync(AdminAccessRightsLog entity, CancellationToken ct = default)
    {
        await _context.AdminAccessRightsLogs.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);
        return entity;
    }
}
