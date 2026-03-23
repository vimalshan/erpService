using MediatR;
using LeaveServices.Application.Commands.Leave;
using LeaveServices.Application.Queries.Leave;
using LeaveServices.Application.DTOs;

namespace LeaveServices.API.GraphQL;

// ── Query Type ──────────────────────────────────────────────────────────────
public sealed class Query
{
    public async Task<IEnumerable<LeaveMasterDto>> GetLeaveMastersAsync(
        [Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(new GetLeaveMasterQuery(), ct);

    public async Task<LeaveMasterDto?> GetLeaveMasterByIdAsync(
        long leaveId,
        [Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(new GetLeaveMasterByIdQuery(leaveId), ct);

    public async Task<IEnumerable<LeaveDetailsDto>> GetLeavesByEmployeeAsync(
        long empId,
        [Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(new GetLeavesByEmployeeQuery(empId), ct);

    public async Task<LeaveDetailsDto?> GetLeaveDetailByIdAsync(
        long leaveDetailId,
        [Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(new GetLeaveDetailByIdQuery(leaveDetailId), ct);

    public async Task<IEnumerable<LeaveCreditDto>> GetLeaveBalancesAsync(
        long empId, int year,
        [Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(new GetLeaveBalanceAllQuery(empId, year), ct);

    public async Task<decimal> GetLeaveBalanceAsync(
        long empId, long leaveId,
        [Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(new GetLeaveBalanceQuery(empId, leaveId), ct);

    public async Task<IEnumerable<LeaveApprovalDto>> GetApprovalHistoryAsync(
        long leaveDetailId,
        [Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(new GetLeaveApprovalHistoryQuery(leaveDetailId), ct);
}

// ── Mutation Type ───────────────────────────────────────────────────────────
public sealed class Mutation
{
    public async Task<long> ApplyLeaveAsync(
        ApplyLeaveInput input,
        [Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(new ApplyLeaveCommand(
            input.EmpSysId, input.LeaveId, input.FromDate, input.ToDate,
            input.AppType, input.TimeUnitId, input.AppliedDays, input.Reason, input.AppliedBy), ct);

    public async Task<bool> ApproveLeaveAsync(
        long leaveDetailId, string status, string? remarks, long approvedBy,
        [Service] IMediator mediator, CancellationToken ct)
    {
        await mediator.Send(new ApproveLeaveCommand(leaveDetailId, status, remarks, approvedBy), ct);
        return true;
    }

    public async Task<long> CreateLeaveMasterAsync(
        CreateLeaveMasterInput input,
        [Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(new CreateLeaveMasterCommand(
            input.Description,
            input.GenderSpecific.Length > 0 ? input.GenderSpecific[0] : 'A',
            input.ApplicableForAll.Length > 0 ? input.ApplicableForAll[0] : 'Y',
            input.MaxDaysPL,
            input.Encashable.Length > 0 ? input.Encashable[0] : 'N',
            input.CarryForward.Length > 0 ? input.CarryForward[0] : 'N',
            input.CreatedBy), ct);
}

// ── Input types ─────────────────────────────────────────────────────────────
public record ApplyLeaveInput(
    long     EmpSysId,
    long     LeaveId,
    DateTime FromDate,
    DateTime ToDate,
    string   AppType,
    int      TimeUnitId,
    decimal  AppliedDays,
    string?  Reason,
    long     AppliedBy);

public record CreateLeaveMasterInput(
    string Description,
    string GenderSpecific,
    string ApplicableForAll,
    int    MaxDaysPL,
    string Encashable,
    string CarryForward,
    long   CreatedBy);
