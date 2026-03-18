using LeaveServices.Application.DTOs;
using LeaveServices.Application.Features.LeaveEncashments.Queries;
using LeaveServices.Application.Features.LeaveRequests.Queries;
using LeaveServices.Application.Features.LossOfPay.Queries;
using MediatR;

namespace LeaveServices.API.GraphQL;

public sealed class LeaveQuery
{
    public async Task<LeaveRequestDto?> GetLeaveRequest(
        [Service] IMediator mediator, long reqNum, CancellationToken ct) =>
        await mediator.Send(new GetLeaveRequestByIdQuery(reqNum), ct);

    public async Task<IEnumerable<LeaveRequestDto>> GetLeaveRequestsByEmployee(
        [Service] IMediator mediator, string empUserId, CancellationToken ct) =>
        await mediator.Send(new GetLeaveRequestsByEmployeeQuery(empUserId), ct);

    public async Task<LeaveEncashmentDto?> GetEncashment(
        [Service] IMediator mediator, long encashmentId, CancellationToken ct) =>
        await mediator.Send(new GetEncashmentByIdQuery(encashmentId), ct);

    public async Task<IEnumerable<LeaveEncashmentDto>> GetEncashmentsByEmployee(
        [Service] IMediator mediator, long empSysId, CancellationToken ct) =>
        await mediator.Send(new GetEncashmentsByEmployeeQuery(empSysId), ct);

    public async Task<IEnumerable<LossOfPayDto>> GetLossOfPayByEmployee(
        [Service] IMediator mediator, long empSysId, CancellationToken ct) =>
        await mediator.Send(new GetLossOfPayByEmployeeQuery(empSysId), ct);
}
