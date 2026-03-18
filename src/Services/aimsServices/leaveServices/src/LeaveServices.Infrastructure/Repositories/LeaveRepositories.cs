using Microsoft.EntityFrameworkCore;
using LeaveServices.Domain.Entities;
using LeaveServices.Domain.Interfaces;
using LeaveServices.Infrastructure.Data;

namespace LeaveServices.Infrastructure.Repositories;

public sealed class LeaveDetailsRepository : ILeaveDetailsRepository
{
    private readonly LeaveDbContext _ctx;
    public LeaveDetailsRepository(LeaveDbContext ctx) => _ctx = ctx;

    public Task<LeaveDetails?> GetByIdAsync(long id, CancellationToken ct) =>
        _ctx.LeaveDetails.FirstOrDefaultAsync(x => x.LeaveDetailId == id, ct);

    public async Task<IEnumerable<LeaveDetails>> GetByEmployeeAsync(long empSysId, CancellationToken ct) =>
        await _ctx.LeaveDetails.Where(x => x.LeaveEmpSysId == empSysId).ToListAsync(ct);

    public async Task<IEnumerable<LeaveDetails>> GetPendingAsync(CancellationToken ct) =>
        await _ctx.LeaveDetails.Where(x => x.LeaveAppStatus == "P").ToListAsync(ct);

    public async Task AddAsync(LeaveDetails entity, CancellationToken ct)
    {
        await _ctx.LeaveDetails.AddAsync(entity, ct);
        await _ctx.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(LeaveDetails entity, CancellationToken ct)
    {
        _ctx.LeaveDetails.Update(entity);
        await _ctx.SaveChangesAsync(ct);
    }

    public async Task<long> GetNextIdAsync(CancellationToken ct) =>
        await _ctx.LeaveDetails.AnyAsync(ct)
            ? await _ctx.LeaveDetails.MaxAsync(x => x.LeaveDetailId, ct) + 1
            : 1L;
}

public sealed class LeaveMasterRepository : ILeaveMasterRepository
{
    private readonly LeaveDbContext _ctx;
    public LeaveMasterRepository(LeaveDbContext ctx) => _ctx = ctx;

    public Task<LeaveMaster?> GetByIdAsync(long id, CancellationToken ct) =>
        _ctx.LeaveMasters.FirstOrDefaultAsync(x => x.LeaveId == id, ct);

    public async Task<IEnumerable<LeaveMaster>> GetAllAsync(CancellationToken ct) =>
        await _ctx.LeaveMasters.ToListAsync(ct);

    public async Task AddAsync(LeaveMaster entity, CancellationToken ct)
    {
        await _ctx.LeaveMasters.AddAsync(entity, ct);
        await _ctx.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(LeaveMaster entity, CancellationToken ct)
    {
        _ctx.LeaveMasters.Update(entity);
        await _ctx.SaveChangesAsync(ct);
    }

    public async Task<long> GetNextIdAsync(CancellationToken ct) =>
        await _ctx.LeaveMasters.AnyAsync(ct)
            ? await _ctx.LeaveMasters.MaxAsync(x => x.LeaveId, ct) + 1
            : 1L;
}

public sealed class LeaveCreditRepository : ILeaveCreditRepository
{
    private readonly LeaveDbContext _ctx;
    public LeaveCreditRepository(LeaveDbContext ctx) => _ctx = ctx;

    public Task<LeaveCredit?> GetByIdAsync(long id, CancellationToken ct) =>
        _ctx.LeaveCredits.FirstOrDefaultAsync(x => x.CreditId == id, ct);

    public async Task<IEnumerable<LeaveCredit>> GetByEmployeeAsync(long empSysId, int year, CancellationToken ct) =>
        await _ctx.LeaveCredits.Where(x => x.CreditEmpSysId == empSysId && x.CreditYear == year).ToListAsync(ct);

    public async Task<decimal> GetBalanceAsync(long empSysId, long leaveId, CancellationToken ct)
    {
        var year    = DateTime.UtcNow.Year;
        var credit  = await _ctx.LeaveCredits
            .Where(x => x.CreditEmpSysId == empSysId && x.CreditLeaveId == leaveId && x.CreditYear == year)
            .FirstOrDefaultAsync(ct);
        return credit?.AvailableBalance ?? 0m;
    }

    public async Task AddAsync(LeaveCredit entity, CancellationToken ct)
    {
        await _ctx.LeaveCredits.AddAsync(entity, ct);
        await _ctx.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(LeaveCredit entity, CancellationToken ct)
    {
        _ctx.LeaveCredits.Update(entity);
        await _ctx.SaveChangesAsync(ct);
    }

    public async Task<long> GetNextIdAsync(CancellationToken ct) =>
        await _ctx.LeaveCredits.AnyAsync(ct)
            ? await _ctx.LeaveCredits.MaxAsync(x => x.CreditId, ct) + 1
            : 1L;
}

public sealed class LeaveApprovalRepository : ILeaveApprovalRepository
{
    private readonly LeaveDbContext _ctx;
    public LeaveApprovalRepository(LeaveDbContext ctx) => _ctx = ctx;

    public async Task AddAsync(LeaveDetailsApproval entity, CancellationToken ct)
    {
        await _ctx.LeaveApprovals.AddAsync(entity, ct);
        await _ctx.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<LeaveDetailsApproval>> GetByDetailIdAsync(long detailId, CancellationToken ct) =>
        await _ctx.LeaveApprovals.Where(x => x.LeaveAprDetailId == detailId).ToListAsync(ct);

    public async Task<long> GetNextIdAsync(CancellationToken ct) =>
        await _ctx.LeaveApprovals.AnyAsync(ct)
            ? await _ctx.LeaveApprovals.MaxAsync(x => x.LeaveAprId, ct) + 1
            : 1L;
}

public sealed class LeaveRulesRepository : ILeaveRulesRepository
{
    private readonly LeaveDbContext _ctx;
    public LeaveRulesRepository(LeaveDbContext ctx) => _ctx = ctx;

    public Task<LeaveRules?> GetByLeaveIdAsync(long leaveId, CancellationToken ct) =>
        _ctx.LeaveRules.FirstOrDefaultAsync(x => x.RuleLeaveId == leaveId, ct);

    public async Task<IEnumerable<LeaveRules>> GetAllAsync(CancellationToken ct) =>
        await _ctx.LeaveRules.ToListAsync(ct);

    public async Task AddAsync(LeaveRules entity, CancellationToken ct)
    {
        await _ctx.LeaveRules.AddAsync(entity, ct);
        await _ctx.SaveChangesAsync(ct);
    }
}

public sealed class CompOffRepository : ICompOffRepository
{
    private readonly LeaveDbContext _ctx;
    public CompOffRepository(LeaveDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<CompOffAdjust>> GetAvailableByEmployeeAsync(long empSysId, CancellationToken ct) =>
        await _ctx.CompOffAdjustments.Where(x => x.CompOffEmpSysId == empSysId && x.CompOffStatus == "A").ToListAsync(ct);

    public async Task AddAsync(CompOffAdjust entity, CancellationToken ct)
    {
        await _ctx.CompOffAdjustments.AddAsync(entity, ct);
        await _ctx.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(CompOffAdjust entity, CancellationToken ct)
    {
        _ctx.CompOffAdjustments.Update(entity);
        await _ctx.SaveChangesAsync(ct);
    }

    public async Task<long> GetNextIdAsync(CancellationToken ct) =>
        await _ctx.CompOffAdjustments.AnyAsync(ct)
            ? await _ctx.CompOffAdjustments.MaxAsync(x => x.CompOffId, ct) + 1
            : 1L;
}
