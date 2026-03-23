using AttendanceService.Application.DTOs;
using AttendanceService.Application.Queries.AttendanceBatch;
using AttendanceService.Application.Queries.Attendance;
using AttendanceService.Application.Queries.SwipePunch;
using MediatR;

namespace AttendanceService.API.GraphQL;

public class AttendanceQuery
{
    public async Task<IEnumerable<SwipePunchDto>> GetSwipePunches(
        long empSysId, DateTime? from, DateTime? to,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetSwipePunchesByEmployeeQuery(empSysId, from, to), ct);

    public async Task<AttendanceBatchDto?> GetBatch(
        long batchId,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAttendanceBatchQuery(batchId), ct);

    public async Task<AttendancePercentageDto> GetAttendancePercentage(
        long empSysId, DateTime monthStart, DateTime monthEnd,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAttendancePercentageQuery(empSysId, monthStart, monthEnd), ct);
}
