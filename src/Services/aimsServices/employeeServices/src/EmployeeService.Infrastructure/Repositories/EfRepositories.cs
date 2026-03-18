using Microsoft.EntityFrameworkCore;
using EmployeeService.Domain.Entities;
using EmployeeService.Domain.Interfaces;
using EmployeeService.Infrastructure.Persistence;

namespace EmployeeService.Infrastructure.Repositories;

public sealed class EmployeeTimeInfoRepository : IEmployeeTimeInfoRepository
{
    private readonly EmployeeDbContext _ctx;
    public EmployeeTimeInfoRepository(EmployeeDbContext ctx) => _ctx = ctx;

    public Task<EmployeeTimeInfo?> GetByIdAsync(long id, CancellationToken ct) =>
        _ctx.EmpTimeInfos.FirstOrDefaultAsync(x => x.TimeInfoId == id, ct);

    public async Task<IEnumerable<EmployeeTimeInfo>> GetByEmployeeIdAsync(long empSysId, CancellationToken ct) =>
        await _ctx.EmpTimeInfos
            .Where(x => x.EmpSysId.Value == empSysId)
            .ToListAsync(ct);

    public async Task<long> GetNextIdAsync(CancellationToken ct)
    {
        var max = await _ctx.EmpTimeInfos.MaxAsync(x => (long?)x.TimeInfoId, ct) ?? 0L;
        return max + 1;
    }

    public async Task AddAsync(EmployeeTimeInfo entity, CancellationToken ct)
    {
        await _ctx.EmpTimeInfos.AddAsync(entity, ct);
        await _ctx.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(EmployeeTimeInfo entity, CancellationToken ct)
    {
        _ctx.EmpTimeInfos.Update(entity);
        await _ctx.SaveChangesAsync(ct);
    }
}

public sealed class EmployeeApproverRepository : IEmployeeApproverRepository
{
    private readonly EmployeeDbContext _ctx;
    public EmployeeApproverRepository(EmployeeDbContext ctx) => _ctx = ctx;

    public Task<EmployeeApprover?> GetByIdAsync(int id, CancellationToken ct) =>
        _ctx.EmployeeApprovers.FirstOrDefaultAsync(x => x.ApproverId == id, ct);

    public async Task<IEnumerable<EmployeeApprover>> GetByEmployeeIdAsync(long empSysId, CancellationToken ct) =>
        await _ctx.EmployeeApprovers
            .Where(x => x.EmpSysId.Value == empSysId)
            .ToListAsync(ct);

    public async Task<int> GetNextIdAsync(CancellationToken ct)
    {
        var max = await _ctx.EmployeeApprovers.MaxAsync(x => (int?)x.ApproverId, ct) ?? 0;
        return max + 1;
    }

    public async Task AddAsync(EmployeeApprover entity, CancellationToken ct)
    {
        await _ctx.EmployeeApprovers.AddAsync(entity, ct);
        await _ctx.SaveChangesAsync(ct);
    }
}

public sealed class EmployeeCalendarRepository : IEmployeeCalendarRepository
{
    private readonly EmployeeDbContext _ctx;
    public EmployeeCalendarRepository(EmployeeDbContext ctx) => _ctx = ctx;

    public Task<EmployeeCalendar?> GetByIdAsync(long id, CancellationToken ct) =>
        _ctx.EmployeeCalendars.FirstOrDefaultAsync(x => x.EmpCalId == id, ct);

    public async Task<IEnumerable<EmployeeCalendar>> GetByEmployeeIdAsync(long empSysId, CancellationToken ct) =>
        await _ctx.EmployeeCalendars
            .Where(x => x.EmpSysId.Value == empSysId)
            .ToListAsync(ct);

    public async Task<long> GetNextIdAsync(CancellationToken ct)
    {
        var max = await _ctx.EmployeeCalendars.MaxAsync(x => (long?)x.EmpCalId, ct) ?? 0L;
        return max + 1;
    }

    public async Task AddAsync(EmployeeCalendar entity, CancellationToken ct)
    {
        await _ctx.EmployeeCalendars.AddAsync(entity, ct);
        await _ctx.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(EmployeeCalendar entity, CancellationToken ct)
    {
        _ctx.EmployeeCalendars.Update(entity);
        await _ctx.SaveChangesAsync(ct);
    }
}

public sealed class EmployeePatternRepository : IEmployeePatternRepository
{
    private readonly EmployeeDbContext _ctx;
    public EmployeePatternRepository(EmployeeDbContext ctx) => _ctx = ctx;

    public Task<EmployeePattern?> GetByIdAsync(long id, CancellationToken ct) =>
        _ctx.EmployeePatterns.FirstOrDefaultAsync(x => x.EmpPatternId == id, ct);

    public async Task<IEnumerable<EmployeePattern>> GetByEmployeeIdAsync(long empSysId, CancellationToken ct) =>
        await _ctx.EmployeePatterns
            .Where(x => x.EmpSysId.Value == empSysId)
            .ToListAsync(ct);

    public async Task<long> GetNextIdAsync(CancellationToken ct)
    {
        var max = await _ctx.EmployeePatterns.MaxAsync(x => (long?)x.EmpPatternId, ct) ?? 0L;
        return max + 1;
    }

    public async Task AddAsync(EmployeePattern entity, CancellationToken ct)
    {
        await _ctx.EmployeePatterns.AddAsync(entity, ct);
        await _ctx.SaveChangesAsync(ct);
    }
}

public sealed class EmployeeShiftRepository : IEmployeeShiftRepository
{
    private readonly EmployeeDbContext _ctx;
    public EmployeeShiftRepository(EmployeeDbContext ctx) => _ctx = ctx;

    public Task<EmployeeShift?> GetByIdAsync(long id, CancellationToken ct) =>
        _ctx.EmployeeShifts.FirstOrDefaultAsync(x => x.EmpShiftId == id, ct);

    public async Task<IEnumerable<EmployeeShift>> GetByEmployeeIdAsync(long empSysId, CancellationToken ct) =>
        await _ctx.EmployeeShifts
            .Where(x => x.EmpSysId.Value == empSysId)
            .ToListAsync(ct);

    public async Task<long> GetNextIdAsync(CancellationToken ct)
    {
        var max = await _ctx.EmployeeShifts.MaxAsync(x => (long?)x.EmpShiftId, ct) ?? 0L;
        return max + 1;
    }

    public async Task AddAsync(EmployeeShift entity, CancellationToken ct)
    {
        await _ctx.EmployeeShifts.AddAsync(entity, ct);
        await _ctx.SaveChangesAsync(ct);
    }
}

public sealed class EmployeeShiftPatternRepository : IEmployeeShiftPatternRepository
{
    private readonly EmployeeDbContext _ctx;
    public EmployeeShiftPatternRepository(EmployeeDbContext ctx) => _ctx = ctx;

    public Task<EmployeeShiftPattern?> GetByIdAsync(long id, CancellationToken ct) =>
        _ctx.EmployeeShiftPatterns.FirstOrDefaultAsync(x => x.EmpShiftId == id, ct);

    public async Task<IEnumerable<EmployeeShiftPattern>> GetByEmployeeIdAsync(long empSysId, CancellationToken ct) =>
        await _ctx.EmployeeShiftPatterns
            .Where(x => x.EmpSysId != null && x.EmpSysId.Value == empSysId)
            .ToListAsync(ct);

    public async Task<long> GetNextIdAsync(CancellationToken ct)
    {
        var max = await _ctx.EmployeeShiftPatterns.MaxAsync(x => (long?)x.EmpShiftId, ct) ?? 0L;
        return max + 1;
    }

    public async Task AddAsync(EmployeeShiftPattern entity, CancellationToken ct)
    {
        await _ctx.EmployeeShiftPatterns.AddAsync(entity, ct);
        await _ctx.SaveChangesAsync(ct);
    }
}
