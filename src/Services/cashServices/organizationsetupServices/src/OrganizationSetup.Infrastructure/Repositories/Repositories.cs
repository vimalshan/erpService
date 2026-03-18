using Microsoft.EntityFrameworkCore;
using OrganizationSetup.Application.Interfaces;
using OrganizationSetup.Domain.Entities;
using OrganizationSetup.Infrastructure.Persistence;

namespace OrganizationSetup.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly OrganizationSetupDbContext _context;

    public RoleRepository(OrganizationSetupDbContext context) => _context = context;

    public async Task<DealRole?> GetByIdAsync(long roleId, CancellationToken ct = default) =>
        await _context.DealRoles.FirstOrDefaultAsync(x => x.RoleId == roleId, ct);

    public async Task<IEnumerable<DealRole>> GetAllAsync(CancellationToken ct = default) =>
        await _context.DealRoles.ToListAsync(ct);

    public async Task<DealRole?> GetByNameAsync(string roleName, CancellationToken ct = default) =>
        await _context.DealRoles.FirstOrDefaultAsync(x => x.RoleName.Value == roleName, ct);

    public async Task AddAsync(DealRole role, CancellationToken ct = default)
    {
        await _context.DealRoles.AddAsync(role, ct);
    }

    public Task UpdateAsync(DealRole role, CancellationToken ct = default)
    {
        _context.DealRoles.Update(role);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long roleId, CancellationToken ct = default)
    {
        var role = await GetByIdAsync(roleId, ct);
        if (role is not null)
            _context.DealRoles.Remove(role);
    }
}

public class UserMapRepository : IUserMapRepository
{
    private readonly OrganizationSetupDbContext _context;

    public UserMapRepository(OrganizationSetupDbContext context) => _context = context;

    public async Task<DealUserMap?> GetByIdAsync(long mapId, CancellationToken ct = default) =>
        await _context.DealUserMaps.Include(x => x.Role).FirstOrDefaultAsync(x => x.RoleMapId == mapId, ct);

    public async Task<IEnumerable<DealUserMap>> GetByOrgAsync(long orgId, CancellationToken ct = default) =>
        await _context.DealUserMaps.Where(x => x.RoleOrgId == orgId).Include(x => x.Role).ToListAsync(ct);

    public async Task<IEnumerable<DealUserMap>> GetByEmployeeAsync(long empSysId, CancellationToken ct = default) =>
        await _context.DealUserMaps.Where(x => x.RoleEmpSysId == empSysId).Include(x => x.Role).ToListAsync(ct);

    public async Task AddAsync(DealUserMap map, CancellationToken ct = default) =>
        await _context.DealUserMaps.AddAsync(map, ct);

    public Task UpdateAsync(DealUserMap map, CancellationToken ct = default)
    {
        _context.DealUserMaps.Update(map);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long mapId, CancellationToken ct = default)
    {
        var map = await GetByIdAsync(mapId, ct);
        if (map is not null)
            _context.DealUserMaps.Remove(map);
    }
}

public class OrgParamsRepository : IOrgParamsRepository
{
    private readonly OrganizationSetupDbContext _context;

    public OrgParamsRepository(OrganizationSetupDbContext context) => _context = context;

    public async Task<DealOrgParams?> GetByIdAsync(long paramId, CancellationToken ct = default) =>
        await _context.DealOrgParams.FirstOrDefaultAsync(x => x.OrgParamId == paramId, ct);

    public async Task<IEnumerable<DealOrgParams>> GetByOrgAsync(long orgId, CancellationToken ct = default) =>
        await _context.DealOrgParams.Where(x => x.OrgId == orgId).ToListAsync(ct);

    public async Task<DealOrgParams?> GetByTypeAsync(long orgId, string paramType, CancellationToken ct = default) =>
        await _context.DealOrgParams.FirstOrDefaultAsync(x => x.OrgId == orgId && x.OrgParamType.Value == paramType, ct);

    public async Task AddAsync(DealOrgParams param, CancellationToken ct = default) =>
        await _context.DealOrgParams.AddAsync(param, ct);

    public Task UpdateAsync(DealOrgParams param, CancellationToken ct = default)
    {
        _context.DealOrgParams.Update(param);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long paramId, CancellationToken ct = default)
    {
        var param = await GetByIdAsync(paramId, ct);
        if (param is not null)
            _context.DealOrgParams.Remove(param);
    }
}

public class PpLimitRepository : IPpLimitRepository
{
    private readonly OrganizationSetupDbContext _context;

    public PpLimitRepository(OrganizationSetupDbContext context) => _context = context;

    public async Task<DealPpLimit?> GetByIdAsync(long limitId, CancellationToken ct = default) =>
        await _context.DealPpLimits.FirstOrDefaultAsync(x => x.PpLimitId == limitId, ct);

    public async Task<IEnumerable<DealPpLimit>> GetByOrgAndYearAsync(long orgId, int finYear, CancellationToken ct = default) =>
        await _context.DealPpLimits.Where(x => x.PpOrgId == orgId && x.PpFinYear == finYear).ToListAsync(ct);

    public async Task AddAsync(DealPpLimit limit, CancellationToken ct = default) =>
        await _context.DealPpLimits.AddAsync(limit, ct);

    public Task UpdateAsync(DealPpLimit limit, CancellationToken ct = default)
    {
        _context.DealPpLimits.Update(limit);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long limitId, CancellationToken ct = default)
    {
        var limit = await GetByIdAsync(limitId, ct);
        if (limit is not null)
            _context.DealPpLimits.Remove(limit);
    }
}
