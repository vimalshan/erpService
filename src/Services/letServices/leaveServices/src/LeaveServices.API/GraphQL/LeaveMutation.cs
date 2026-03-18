using LeaveServices.Application.DTOs;
using LeaveServices.Application.Features.LeaveEncashments.Commands;
using LeaveServices.Application.Features.LeaveRequests.Commands;
using LeaveServices.Application.Features.LossOfPay.Commands;
using MediatR;

namespace LeaveServices.API.GraphQL;

public sealed class LeaveMutation
{
    public async Task<LeaveRequestDto> CreateLeaveRequest(
        [Service] IMediator mediator,
        long reqNum, int finyearSrlno, string empUserId, string? supUserId,
        CancellationToken ct) =>
        await mediator.Send(new CreateLeaveRequestCommand(reqNum, finyearSrlno, empUserId, supUserId), ct);

    public async Task<LeaveEncashmentDto> ApplyEncashment(
        [Service] IMediator mediator,
        long empSysId, string leaveType, int encashmentDays, decimal basicSalary,
        DateOnly requestDate, long requestedBy,
        CancellationToken ct) =>
        await mediator.Send(new ApplyLeaveEncashmentCommand(empSysId, leaveType, encashmentDays, basicSalary, requestDate, requestedBy), ct);

    public async Task<LeaveEncashmentDto> UpdateEncashmentStatus(
        [Service] IMediator mediator,
        long encashmentId, char newStatus, long modifiedBy,
        CancellationToken ct) =>
        await mediator.Send(new UpdateEncashmentStatusCommand(encashmentId, newStatus, modifiedBy), ct);

    public async Task<LossOfPayDto> RecordLossOfPay(
        [Service] IMediator mediator,
        long empSysId, int lopDays, DateOnly lopMonth, string? remarks, long recordedBy,
        CancellationToken ct) =>
        await mediator.Send(new RecordLossOfPayCommand(empSysId, lopDays, lopMonth, remarks, recordedBy), ct);
}
