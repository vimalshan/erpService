using GroupIncentiveService.Domain.Entities;
using GroupIncentiveService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GroupIncentiveService.Infrastructure.Persistence.Repositories;

public class GroupMasterRepository : IGroupMasterRepository
{
    private readonly GroupIncentiveDbContext _context;

    public GroupMasterRepository(GroupIncentiveDbContext context) => _context = context;

    public async Task<GroupMaster?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _context.GroupMasters.Include(g => g.EmployeeMappings)
            .FirstOrDefaultAsync(g => g.GroupId == id, ct);

    public async Task<GroupMaster?> GetByNameAsync(string name, CancellationToken ct = default)
        => await _context.GroupMasters.FirstOrDefaultAsync(g => g.GroupName == name, ct);

    public async Task<IEnumerable<GroupMaster>> GetAllAsync(bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _context.GroupMasters.AsQueryable();
        if (activeOnly) query = query.Where(g => g.GroupStatus == "Y");
        return await query.OrderBy(g => g.GroupName).ToListAsync(ct);
    }

    public async Task<GroupMaster> AddAsync(GroupMaster group, CancellationToken ct = default)
    {
        await _context.GroupMasters.AddAsync(group, ct);
        return group;
    }

    public Task UpdateAsync(GroupMaster group, CancellationToken ct = default)
    {
        _context.GroupMasters.Update(group);
        return Task.CompletedTask;
    }

    public async Task<int> GetNextIdAsync(CancellationToken ct = default)
    {
        var max = await _context.GroupMasters.MaxAsync(g => (int?)g.GroupId, ct) ?? 0;
        return max + 1;
    }
}

public class GroupEmployeeMapRepository : IGroupEmployeeMapRepository
{
    private readonly GroupIncentiveDbContext _context;

    public GroupEmployeeMapRepository(GroupIncentiveDbContext context) => _context = context;

    public async Task<GroupEmployeeMap?> GetByIdAsync(long id, CancellationToken ct = default)
        => await _context.GroupEmployeeMaps.FindAsync(new object[] { id }, ct);

    public async Task<IEnumerable<GroupEmployeeMap>> GetByGroupIdAsync(int groupId, CancellationToken ct = default)
        => await _context.GroupEmployeeMaps.Where(m => m.GrpEmpMapGroupId == groupId).ToListAsync(ct);

    public async Task<IEnumerable<GroupEmployeeMap>> GetByEmployeeIdAsync(long employeeId, CancellationToken ct = default)
        => await _context.GroupEmployeeMaps.Where(m => m.GrpEmpMapEmpSysId == employeeId).ToListAsync(ct);

    public async Task<GroupEmployeeMap> AddAsync(GroupEmployeeMap mapping, CancellationToken ct = default)
    {
        await _context.GroupEmployeeMaps.AddAsync(mapping, ct);
        return mapping;
    }

    public Task UpdateAsync(GroupEmployeeMap mapping, CancellationToken ct = default)
    {
        _context.GroupEmployeeMaps.Update(mapping);
        return Task.CompletedTask;
    }

    public async Task<long> GetNextIdAsync(CancellationToken ct = default)
    {
        var max = await _context.GroupEmployeeMaps.MaxAsync(m => (long?)m.GrpEmpMapId, ct) ?? 0;
        return max + 1;
    }
}

public class GroupIncentiveMainRepository : IGroupIncentiveMainRepository
{
    private readonly GroupIncentiveDbContext _context;

    public GroupIncentiveMainRepository(GroupIncentiveDbContext context) => _context = context;

    public async Task<GroupIncentiveMain?> GetByIdAsync(long id, CancellationToken ct = default)
        => await _context.GroupIncentiveMains.FirstOrDefaultAsync(m => m.GrpIncId == id, ct);

    public async Task<GroupIncentiveMain?> GetByIdWithDetailsAsync(long id, CancellationToken ct = default)
        => await _context.GroupIncentiveMains
            .Include(m => m.Group)
            .Include(m => m.Details)
            .Include(m => m.Approvals)
            .FirstOrDefaultAsync(m => m.GrpIncId == id, ct);

    public async Task<IEnumerable<GroupIncentiveMain>> GetByGroupIdAsync(int groupId, CancellationToken ct = default)
        => await _context.GroupIncentiveMains
            .Where(m => m.GrpIncGroupId == groupId)
            .OrderByDescending(m => m.GrpIncIncYear).ThenByDescending(m => m.GrpIncIncMonth)
            .ToListAsync(ct);

    public async Task<IEnumerable<GroupIncentiveMain>> GetPendingAsync(CancellationToken ct = default)
        => await _context.GroupIncentiveMains
            .Where(m => m.GrpIncAppStatus == "P")
            .Include(m => m.Group)
            .ToListAsync(ct);

    public async Task<GroupIncentiveMain> AddAsync(GroupIncentiveMain incentive, CancellationToken ct = default)
    {
        await _context.GroupIncentiveMains.AddAsync(incentive, ct);
        return incentive;
    }

    public Task UpdateAsync(GroupIncentiveMain incentive, CancellationToken ct = default)
    {
        _context.GroupIncentiveMains.Update(incentive);
        return Task.CompletedTask;
    }

    public async Task<long> GetNextIdAsync(CancellationToken ct = default)
    {
        var max = await _context.GroupIncentiveMains.MaxAsync(m => (long?)m.GrpIncId, ct) ?? 0;
        return max + 1;
    }
}

public class GroupIncentiveDetRepository : IGroupIncentiveDetRepository
{
    private readonly GroupIncentiveDbContext _context;

    public GroupIncentiveDetRepository(GroupIncentiveDbContext context) => _context = context;

    public async Task<GroupIncentiveDet?> GetByIdAsync(long id, CancellationToken ct = default)
        => await _context.GroupIncentiveDets.FindAsync(new object[] { id }, ct);

    public async Task<IEnumerable<GroupIncentiveDet>> GetByMainIdAsync(long mainId, CancellationToken ct = default)
        => await _context.GroupIncentiveDets.Where(d => d.GrpIncDetMainId == mainId).ToListAsync(ct);

    public async Task<GroupIncentiveDet> AddAsync(GroupIncentiveDet detail, CancellationToken ct = default)
    {
        await _context.GroupIncentiveDets.AddAsync(detail, ct);
        return detail;
    }

    public async Task AddRangeAsync(IEnumerable<GroupIncentiveDet> details, CancellationToken ct = default)
        => await _context.GroupIncentiveDets.AddRangeAsync(details, ct);

    public Task UpdateAsync(GroupIncentiveDet detail, CancellationToken ct = default)
    {
        _context.GroupIncentiveDets.Update(detail);
        return Task.CompletedTask;
    }

    public async Task<long> GetNextIdAsync(CancellationToken ct = default)
    {
        var max = await _context.GroupIncentiveDets.MaxAsync(d => (long?)d.GrpIncDetId, ct) ?? 0;
        return max + 1;
    }
}

public class GroupIncentiveBreakRepository : IGroupIncentiveBreakRepository
{
    private readonly GroupIncentiveDbContext _context;

    public GroupIncentiveBreakRepository(GroupIncentiveDbContext context) => _context = context;

    public async Task<GroupIncentiveBreak?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _context.GroupIncentiveBreaks.FindAsync(new object[] { id }, ct);

    public async Task<IEnumerable<GroupIncentiveBreak>> GetByGroupIdAsync(int groupId, CancellationToken ct = default)
        => await _context.GroupIncentiveBreaks.Where(b => b.GrpIncBrkGroupId == groupId).ToListAsync(ct);

    public async Task<GroupIncentiveBreak> AddAsync(GroupIncentiveBreak breakRule, CancellationToken ct = default)
    {
        await _context.GroupIncentiveBreaks.AddAsync(breakRule, ct);
        return breakRule;
    }

    public Task UpdateAsync(GroupIncentiveBreak breakRule, CancellationToken ct = default)
    {
        _context.GroupIncentiveBreaks.Update(breakRule);
        return Task.CompletedTask;
    }

    public async Task<int> GetNextIdAsync(CancellationToken ct = default)
    {
        var max = await _context.GroupIncentiveBreaks.MaxAsync(b => (int?)b.GrpIncBrkId, ct) ?? 0;
        return max + 1;
    }
}

public class GroupIncentiveApprovalRepository : IGroupIncentiveApprovalRepository
{
    private readonly GroupIncentiveDbContext _context;

    public GroupIncentiveApprovalRepository(GroupIncentiveDbContext context) => _context = context;

    public async Task<IEnumerable<GroupIncentiveApproval>> GetByMainIdAsync(long mainId, CancellationToken ct = default)
        => await _context.GroupIncentiveApprovals.Where(a => a.GrpIncAppMainId == mainId).ToListAsync(ct);

    public async Task<GroupIncentiveApproval> AddAsync(GroupIncentiveApproval approval, CancellationToken ct = default)
    {
        await _context.GroupIncentiveApprovals.AddAsync(approval, ct);
        return approval;
    }

    public async Task<long> GetNextIdAsync(CancellationToken ct = default)
    {
        var max = await _context.GroupIncentiveApprovals.MaxAsync(a => (long?)a.GrpIncAppId, ct) ?? 0;
        return max + 1;
    }
}
