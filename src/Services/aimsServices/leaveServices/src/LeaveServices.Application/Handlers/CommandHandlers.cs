using MediatR;
using LeaveServices.Application.Commands.Leave;
using LeaveServices.Domain.Entities;
using LeaveServices.Domain.Interfaces;

namespace LeaveServices.Application.Handlers.Commands;

public sealed class ApplyLeaveCommandHandler : IRequestHandler<ApplyLeaveCommand, long>
{
    private readonly ILeaveDetailsRepository _leaveRepo;
    private readonly ILeaveCreditRepository  _creditRepo;

    public ApplyLeaveCommandHandler(
        ILeaveDetailsRepository leaveRepo,
        ILeaveCreditRepository creditRepo)
    {
        _leaveRepo  = leaveRepo;
        _creditRepo = creditRepo;
    }

    public async Task<long> Handle(ApplyLeaveCommand req, CancellationToken ct)
    {
        var balance = await _creditRepo.GetBalanceAsync(req.EmpSysId, req.LeaveId, ct);
        if (balance < req.AppliedDays)
            throw new InvalidOperationException($"Insufficient leave balance. Available: {balance}, Requested: {req.AppliedDays}");

        var nextId = await _leaveRepo.GetNextIdAsync(ct);
        var leave  = LeaveDetails.Apply(
            nextId, req.EmpSysId, req.FromDate, req.ToDate,
            req.AppType, req.LeaveId, req.TimeUnitId,
            req.AppliedDays, req.Reason, req.AppliedBy);

        await _leaveRepo.AddAsync(leave, ct);
        return nextId;
    }
}

public sealed class ApproveLeaveCommandHandler : IRequestHandler<ApproveLeaveCommand, Unit>
{
    private readonly ILeaveDetailsRepository  _leaveRepo;
    private readonly ILeaveApprovalRepository _aprRepo;

    public ApproveLeaveCommandHandler(
        ILeaveDetailsRepository leaveRepo,
        ILeaveApprovalRepository aprRepo)
    {
        _leaveRepo = leaveRepo;
        _aprRepo   = aprRepo;
    }

    public async Task<Unit> Handle(ApproveLeaveCommand req, CancellationToken ct)
    {
        var leave = await _leaveRepo.GetByIdAsync(req.LeaveDetailId, ct)
                    ?? throw new KeyNotFoundException($"Leave application {req.LeaveDetailId} not found.");

        switch (req.Status)
        {
            case "Y": leave.Approve(req.ApprovedBy); break;
            case "R": leave.Reject(req.ApprovedBy, req.Remarks ?? string.Empty); break;
            case "C": leave.Cancel(req.ApprovedBy); break;
            default:  throw new ArgumentException($"Invalid status: {req.Status}");
        }

        var aprId = await _aprRepo.GetNextIdAsync(ct);
        var apr   = LeaveDetailsApproval.Create(aprId, req.LeaveDetailId, req.Status, req.Remarks, req.ApprovedBy);

        await _leaveRepo.UpdateAsync(leave, ct);
        await _aprRepo.AddAsync(apr, ct);
        return Unit.Value;
    }
}

public sealed class CancelLeaveCommandHandler : IRequestHandler<CancelLeaveCommand, Unit>
{
    private readonly ILeaveDetailsRepository _leaveRepo;

    public CancelLeaveCommandHandler(ILeaveDetailsRepository leaveRepo) => _leaveRepo = leaveRepo;

    public async Task<Unit> Handle(CancelLeaveCommand req, CancellationToken ct)
    {
        var leave = await _leaveRepo.GetByIdAsync(req.LeaveDetailId, ct)
                    ?? throw new KeyNotFoundException($"Leave application {req.LeaveDetailId} not found.");
        leave.Cancel(req.CancelledBy);
        await _leaveRepo.UpdateAsync(leave, ct);
        return Unit.Value;
    }
}

public sealed class CreateLeaveMasterCommandHandler : IRequestHandler<CreateLeaveMasterCommand, long>
{
    private readonly ILeaveMasterRepository _repo;
    public CreateLeaveMasterCommandHandler(ILeaveMasterRepository repo) => _repo = repo;

    public async Task<long> Handle(CreateLeaveMasterCommand req, CancellationToken ct)
    {
        var nextId = await _repo.GetNextIdAsync(ct);
        var master = LeaveMaster.Create(nextId, req.Description, req.GenderSpecific,
            req.ApplicableForAll, req.MaxDaysPL, req.Encashable, req.CarryForward, req.CreatedBy);
        await _repo.AddAsync(master, ct);
        return nextId;
    }
}

public sealed class UpdateLeaveMasterCommandHandler : IRequestHandler<UpdateLeaveMasterCommand, Unit>
{
    private readonly ILeaveMasterRepository _repo;
    public UpdateLeaveMasterCommandHandler(ILeaveMasterRepository repo) => _repo = repo;

    public async Task<Unit> Handle(UpdateLeaveMasterCommand req, CancellationToken ct)
    {
        var master = await _repo.GetByIdAsync(req.LeaveId, ct)
                     ?? throw new KeyNotFoundException($"Leave type {req.LeaveId} not found.");
        master.Update(req.Description, req.GenderSpecific, req.ApplicableForAll,
            req.MaxDaysPL, req.Encashable, req.CarryForward, req.ModifiedBy);
        await _repo.UpdateAsync(master, ct);
        return Unit.Value;
    }
}

public sealed class CreditLeaveCommandHandler : IRequestHandler<CreditLeaveCommand, long>
{
    private readonly ILeaveCreditRepository _repo;
    public CreditLeaveCommandHandler(ILeaveCreditRepository repo) => _repo = repo;

    public async Task<long> Handle(CreditLeaveCommand req, CancellationToken ct)
    {
        var nextId = await _repo.GetNextIdAsync(ct);
        var credit = LeaveCredit.Create(nextId, req.EmpSysId, req.LeaveId, req.Flag,
            req.Year, req.Opening, req.Credited, req.ModifiedBy);
        await _repo.AddAsync(credit, ct);
        return nextId;
    }
}

public sealed class AddCompOffCommandHandler : IRequestHandler<AddCompOffCommand, long>
{
    private readonly ICompOffRepository _repo;
    public AddCompOffCommandHandler(ICompOffRepository repo) => _repo = repo;

    public async Task<long> Handle(AddCompOffCommand req, CancellationToken ct)
    {
        var nextId   = await _repo.GetNextIdAsync(ct);
        var compOff  = CompOffAdjust.Create(nextId, req.EmpSysId, req.CompOffDate, req.CreatedBy);
        await _repo.AddAsync(compOff, ct);
        return nextId;
    }
}
