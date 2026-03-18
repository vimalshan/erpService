using MediatR;
using LeaveServices.Application.DTOs;
using LeaveServices.Application.Queries.Leave;
using LeaveServices.Domain.Interfaces;
using LeaveServices.Domain.ValueObjects;

namespace LeaveServices.Application.Handlers.Queries;

public sealed class GetLeaveDetailByIdHandler : IRequestHandler<GetLeaveDetailByIdQuery, LeaveDetailsDto?>
{
    private readonly ILeaveDetailsRepository _repo;
    private readonly ILeaveMasterRepository  _masterRepo;
    public GetLeaveDetailByIdHandler(ILeaveDetailsRepository repo, ILeaveMasterRepository masterRepo)
    { _repo = repo; _masterRepo = masterRepo; }

    public async Task<LeaveDetailsDto?> Handle(GetLeaveDetailByIdQuery req, CancellationToken ct)
    {
        var e = await _repo.GetByIdAsync(req.LeaveDetailId, ct);
        if (e is null) return null;
        var master = await _masterRepo.GetByIdAsync(e.LeaveId, ct);
        return new LeaveDetailsDto(e.LeaveDetailId, e.LeaveEmpSysId, e.LeaveAppFrom, e.LeaveAppTo,
            e.LeaveAppType, e.LeaveId, master?.LeaveDescription,
            e.LeaveAppStatus, LeaveStatus.From(e.LeaveAppStatus).DisplayName,
            e.LeaveAppliedDays, e.LeaveReason, e.LeaveEnteredOn, e.LeaveEnteredBy);
    }
}

public sealed class GetLeavesByEmployeeHandler : IRequestHandler<GetLeavesByEmployeeQuery, IEnumerable<LeaveDetailsDto>>
{
    private readonly ILeaveDetailsRepository _repo;
    private readonly ILeaveMasterRepository  _masterRepo;
    public GetLeavesByEmployeeHandler(ILeaveDetailsRepository repo, ILeaveMasterRepository masterRepo)
    { _repo = repo; _masterRepo = masterRepo; }

    public async Task<IEnumerable<LeaveDetailsDto>> Handle(GetLeavesByEmployeeQuery req, CancellationToken ct)
    {
        var list   = await _repo.GetByEmployeeAsync(req.EmpSysId, ct);
        var master = await _masterRepo.GetAllAsync(ct);
        var dict   = master.ToDictionary(m => m.LeaveId);
        return list.Select(e => new LeaveDetailsDto(
            e.LeaveDetailId, e.LeaveEmpSysId, e.LeaveAppFrom, e.LeaveAppTo,
            e.LeaveAppType, e.LeaveId, dict.TryGetValue(e.LeaveId, out var m) ? m.LeaveDescription : null,
            e.LeaveAppStatus, LeaveStatus.From(e.LeaveAppStatus).DisplayName,
            e.LeaveAppliedDays, e.LeaveReason, e.LeaveEnteredOn, e.LeaveEnteredBy));
    }
}

public sealed class GetPendingLeavesHandler : IRequestHandler<GetPendingLeavesQuery, IEnumerable<LeaveDetailsDto>>
{
    private readonly ILeaveDetailsRepository _repo;
    private readonly ILeaveMasterRepository  _masterRepo;
    public GetPendingLeavesHandler(ILeaveDetailsRepository repo, ILeaveMasterRepository masterRepo)
    { _repo = repo; _masterRepo = masterRepo; }

    public async Task<IEnumerable<LeaveDetailsDto>> Handle(GetPendingLeavesQuery req, CancellationToken ct)
    {
        var list   = await _repo.GetPendingAsync(ct);
        var master = await _masterRepo.GetAllAsync(ct);
        var dict   = master.ToDictionary(m => m.LeaveId);
        return list.Select(e => new LeaveDetailsDto(
            e.LeaveDetailId, e.LeaveEmpSysId, e.LeaveAppFrom, e.LeaveAppTo,
            e.LeaveAppType, e.LeaveId, dict.TryGetValue(e.LeaveId, out var m) ? m.LeaveDescription : null,
            e.LeaveAppStatus, "Pending", e.LeaveAppliedDays, e.LeaveReason, e.LeaveEnteredOn, e.LeaveEnteredBy));
    }
}

public sealed class GetLeaveMasterHandler : IRequestHandler<GetLeaveMasterQuery, IEnumerable<LeaveMasterDto>>
{
    private readonly ILeaveMasterRepository _repo;
    public GetLeaveMasterHandler(ILeaveMasterRepository repo) => _repo = repo;

    public async Task<IEnumerable<LeaveMasterDto>> Handle(GetLeaveMasterQuery req, CancellationToken ct)
    {
        var list = await _repo.GetAllAsync(ct);
        return list.Select(m => new LeaveMasterDto(m.LeaveId, m.LeaveDescription,
            m.LeaveGenderSpecific, m.LeaveApplicableForAll, m.LeaveMaxDaysPL,
            m.LeaveEncashable, m.LeaveCarryForward));
    }
}

public sealed class GetLeaveMasterByIdHandler : IRequestHandler<GetLeaveMasterByIdQuery, LeaveMasterDto?>
{
    private readonly ILeaveMasterRepository _repo;
    public GetLeaveMasterByIdHandler(ILeaveMasterRepository repo) => _repo = repo;

    public async Task<LeaveMasterDto?> Handle(GetLeaveMasterByIdQuery req, CancellationToken ct)
    {
        var m = await _repo.GetByIdAsync(req.LeaveId, ct);
        return m is null ? null : new LeaveMasterDto(m.LeaveId, m.LeaveDescription,
            m.LeaveGenderSpecific, m.LeaveApplicableForAll, m.LeaveMaxDaysPL,
            m.LeaveEncashable, m.LeaveCarryForward);
    }
}

public sealed class GetLeaveBalanceHandler : IRequestHandler<GetLeaveBalanceQuery, decimal>
{
    private readonly ILeaveCreditRepository _repo;
    public GetLeaveBalanceHandler(ILeaveCreditRepository repo) => _repo = repo;

    public async Task<decimal> Handle(GetLeaveBalanceQuery req, CancellationToken ct) =>
        await _repo.GetBalanceAsync(req.EmpSysId, req.LeaveId, ct);
}

public sealed class GetLeaveBalanceAllHandler : IRequestHandler<GetLeaveBalanceAllQuery, IEnumerable<LeaveCreditDto>>
{
    private readonly ILeaveCreditRepository _creditRepo;
    private readonly ILeaveMasterRepository _masterRepo;
    public GetLeaveBalanceAllHandler(ILeaveCreditRepository creditRepo, ILeaveMasterRepository masterRepo)
    { _creditRepo = creditRepo; _masterRepo = masterRepo; }

    public async Task<IEnumerable<LeaveCreditDto>> Handle(GetLeaveBalanceAllQuery req, CancellationToken ct)
    {
        var credits = await _creditRepo.GetByEmployeeAsync(req.EmpSysId, req.Year, ct);
        var masters = await _masterRepo.GetAllAsync(ct);
        var dict    = masters.ToDictionary(m => m.LeaveId);
        return credits.Select(c => new LeaveCreditDto(
            c.CreditId, c.CreditEmpSysId, c.CreditLeaveId,
            dict.TryGetValue(c.CreditLeaveId, out var m) ? m.LeaveDescription : null,
            c.CreditYear, c.CreditOpening, c.CreditCredited, c.CreditUtilized,
            c.CreditClosing, c.AvailableBalance));
    }
}

public sealed class GetLeaveApprovalHistoryHandler : IRequestHandler<GetLeaveApprovalHistoryQuery, IEnumerable<LeaveApprovalDto>>
{
    private readonly ILeaveApprovalRepository _repo;
    public GetLeaveApprovalHistoryHandler(ILeaveApprovalRepository repo) => _repo = repo;

    public async Task<IEnumerable<LeaveApprovalDto>> Handle(GetLeaveApprovalHistoryQuery req, CancellationToken ct)
    {
        var list = await _repo.GetByDetailIdAsync(req.LeaveDetailId, ct);
        return list.Select(a => new LeaveApprovalDto(a.LeaveAprId, a.LeaveAprDetailId,
            a.LeaveAprApproveStatus, a.LeaveAprRemarks, a.LeaveAprApprovedOn, a.LeaveAprApprovedBy));
    }
}

public sealed class GetCompOffByEmployeeHandler : IRequestHandler<GetCompOffByEmployeeQuery, IEnumerable<CompOffDto>>
{
    private readonly ICompOffRepository _repo;
    public GetCompOffByEmployeeHandler(ICompOffRepository repo) => _repo = repo;

    public async Task<IEnumerable<CompOffDto>> Handle(GetCompOffByEmployeeQuery req, CancellationToken ct)
    {
        var list = await _repo.GetAvailableByEmployeeAsync(req.EmpSysId, ct);
        return list.Select(c => new CompOffDto(c.CompOffId, c.CompOffEmpSysId,
            c.CompOffCompOffDate, c.CompOffUsedDate, c.CompOffStatus));
    }
}
