using AttendanceService.Application.Commands.AttendanceBatch;
using AttendanceService.Application.Commands.SwipePunch;
using AttendanceService.Application.DTOs;
using MediatR;

namespace AttendanceService.API.GraphQL;

public class AttendanceMutation
{
    public async Task<SwipePunchDto> RecordSwipePunch(
        long empSysId, DateTime punchTime, string gateNo, string punchStatus,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new RecordSwipePunchCommand(empSysId, punchTime, gateNo, punchStatus), ct);

    public async Task<AttendanceBatchDto> ProcessMonthlyAttendance(
        DateTime monthStart, DateTime monthEnd, long processedBy,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new ProcessMonthlyAttendanceCommand(monthStart, monthEnd, processedBy), ct);
}
