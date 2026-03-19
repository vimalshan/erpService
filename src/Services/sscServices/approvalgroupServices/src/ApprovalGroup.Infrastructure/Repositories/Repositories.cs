using Microsoft.EntityFrameworkCore;
using ApprovalGroup.Domain.Entities;
using ApprovalGroup.Domain.Interfaces;
using ApprovalGroup.Infrastructure.Persistence;

namespace ApprovalGroup.Infrastructure.Repositories;

public class ApprovalGroupRepository : IApprovalGroupRepository
{
    private readonly ApprovalGroupDbContext _context;

    public ApprovalGroupRepository(ApprovalGroupDbContext context) => _context = context;

    public async Task<ApprovalGroupMaster?> GetByIdAsync(long groupId, CancellationToken ct = default)
        => await _context.ApGroupMast
            .Include(g => g.GroupMaps).ThenInclude(m => m.UnitMaps)
            .Include(g => g.GroupMaps).ThenInclude(m => m.PayByMaps)
            .Include(g => g.GroupMaps).ThenInclude(m => m.MainCatMaps)
            .Include(g => g.UserMaps)
            .FirstOrDefaultAsync(g => g.GroupId == groupId, ct);

    public async Task<IEnumerable<ApprovalGroupMaster>> GetAllAsync(CancellationToken ct = default)
        => await _context.ApGroupMast
            .Include(g => g.GroupMaps)
            .Include(g => g.UserMaps)
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task<ApprovalGroupMaster> AddAsync(ApprovalGroupMaster group, CancellationToken ct = default)
    {
        await _context.ApGroupMast.AddAsync(group, ct);
        return group;
    }

    public Task UpdateAsync(ApprovalGroupMaster group, CancellationToken ct = default)
    {
        _context.ApGroupMast.Update(group);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long groupId, CancellationToken ct = default)
    {
        var group = await _context.ApGroupMast.FindAsync(new object[] { groupId }, ct);
        if (group is not null) _context.ApGroupMast.Remove(group);
    }

    public async Task<bool> ExistsAsync(long groupId, CancellationToken ct = default)
        => await _context.ApGroupMast.AnyAsync(g => g.GroupId == groupId, ct);

    public async Task<long> GetNextIdAsync(CancellationToken ct = default)
    {
        var max = await _context.ApGroupMast.MaxAsync(g => (long?)g.GroupId, ct) ?? 0;
        return max + 1;
    }
}

public class ApprovalGroupMapRepository : IApprovalGroupMapRepository
{
    private readonly ApprovalGroupDbContext _context;

    public ApprovalGroupMapRepository(ApprovalGroupDbContext context) => _context = context;

    public async Task<ApprovalGroupMap?> GetByIdAsync(long mapId, CancellationToken ct = default)
        => await _context.ApGroupMap
            .Include(m => m.UnitMaps)
            .Include(m => m.PayByMaps)
            .Include(m => m.MainCatMaps)
            .FirstOrDefaultAsync(m => m.MapId == mapId, ct);

    public async Task<IEnumerable<ApprovalGroupMap>> GetByGroupIdAsync(long groupId, CancellationToken ct = default)
        => await _context.ApGroupMap
            .Include(m => m.UnitMaps)
            .Include(m => m.PayByMaps)
            .Include(m => m.MainCatMaps)
            .Where(m => m.MapGroupId == groupId)
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task<ApprovalGroupMap> AddAsync(ApprovalGroupMap map, CancellationToken ct = default)
    {
        await _context.ApGroupMap.AddAsync(map, ct);
        return map;
    }

    public Task UpdateAsync(ApprovalGroupMap map, CancellationToken ct = default)
    {
        _context.ApGroupMap.Update(map);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long mapId, CancellationToken ct = default)
    {
        var map = await _context.ApGroupMap.FindAsync(new object[] { mapId }, ct);
        if (map is not null) _context.ApGroupMap.Remove(map);
    }

    public async Task<long> GetNextIdAsync(CancellationToken ct = default)
    {
        var max = await _context.ApGroupMap.MaxAsync(m => (long?)m.MapId, ct) ?? 0;
        return max + 1;
    }
}

public class ApprovalGroupUserMapRepository : IApprovalGroupUserMapRepository
{
    private readonly ApprovalGroupDbContext _context;

    public ApprovalGroupUserMapRepository(ApprovalGroupDbContext context) => _context = context;

    public async Task<ApprovalGroupUserMap?> GetByIdAsync(long mapId, CancellationToken ct = default)
        => await _context.ApGroupUserMap.FirstOrDefaultAsync(m => m.MapId == mapId, ct);

    public async Task<IEnumerable<ApprovalGroupUserMap>> GetByGroupIdAsync(long groupId, CancellationToken ct = default)
        => await _context.ApGroupUserMap.Where(m => m.MapGroupId == groupId).AsNoTracking().ToListAsync(ct);

    public async Task<ApprovalGroupUserMap> AddAsync(ApprovalGroupUserMap userMap, CancellationToken ct = default)
    {
        await _context.ApGroupUserMap.AddAsync(userMap, ct);
        return userMap;
    }

    public Task UpdateAsync(ApprovalGroupUserMap userMap, CancellationToken ct = default)
    {
        _context.ApGroupUserMap.Update(userMap);
        return Task.CompletedTask;
    }

    public async Task<long> GetNextIdAsync(CancellationToken ct = default)
    {
        var max = await _context.ApGroupUserMap.MaxAsync(m => (long?)m.MapId, ct) ?? 0;
        return max + 1;
    }
}

public class PullMatrixRepository : IPullMatrixRepository
{
    private readonly ApprovalGroupDbContext _context;

    public PullMatrixRepository(ApprovalGroupDbContext context) => _context = context;

    public async Task<PullMatrixDetail?> GetByIdAsync(long matId, CancellationToken ct = default)
        => await _context.PullMatrixDet.FirstOrDefaultAsync(m => m.MatId == matId, ct);

    public async Task<IEnumerable<PullMatrixDetail>> GetByUnitIdAsync(long unitId, CancellationToken ct = default)
        => await _context.PullMatrixDet.Where(m => m.MatUnitId == unitId).AsNoTracking().ToListAsync(ct);

    public async Task<PullMatrixDetail> AddAsync(PullMatrixDetail detail, CancellationToken ct = default)
    {
        await _context.PullMatrixDet.AddAsync(detail, ct);
        return detail;
    }

    public Task UpdateAsync(PullMatrixDetail detail, CancellationToken ct = default)
    {
        _context.PullMatrixDet.Update(detail);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long matId, CancellationToken ct = default)
    {
        var detail = await _context.PullMatrixDet.FindAsync(new object[] { matId }, ct);
        if (detail is not null) _context.PullMatrixDet.Remove(detail);
    }

    public async Task<long> GetNextIdAsync(CancellationToken ct = default)
    {
        var max = await _context.PullMatrixDet.MaxAsync(m => (long?)m.MatId, ct) ?? 0;
        return max + 1;
    }
}
