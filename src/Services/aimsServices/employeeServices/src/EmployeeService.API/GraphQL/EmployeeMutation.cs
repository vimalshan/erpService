using HotChocolate;
using MediatR;
using EmployeeService.Application.Commands.AssignApprover;
using EmployeeService.Application.Commands.MapCalendar;
using EmployeeService.Application.Commands.RecordTimeInfo;
using EmployeeService.Application.DTOs;

namespace EmployeeService.API.GraphQL;

public sealed class EmployeeMutation
{
    public async Task<EmployeeApproverDto> AssignApprover(
        AssignApproverInput input,
        [Service] IMediator mediator,
        CancellationToken ct)
    {
        return await mediator.Send(
            new AssignApproverCommand(input.EmpSysId, input.ApproverSysId, input.Level, input.AssignedBy), ct);
    }

    public async Task<EmployeeCalendarDto> MapCalendar(
        MapCalendarInput input,
        [Service] IMediator mediator,
        CancellationToken ct)
    {
        return await mediator.Send(
            new MapCalendarCommand(input.EmpSysId, input.CalendarId, input.MappedBy), ct);
    }

    public async Task<EmployeeTimeInfoDto> RecordTimeInfo(
        RecordTimeInfoInput input,
        [Service] IMediator mediator,
        CancellationToken ct)
    {
        return await mediator.Send(
            new RecordTimeInfoCommand(input.EmpSysId, input.AttFlag, input.ModifiedBy), ct);
    }
}

public record AssignApproverInput(long EmpSysId, long ApproverSysId, int Level, long AssignedBy);
public record MapCalendarInput(long EmpSysId, int CalendarId, long MappedBy);
public record RecordTimeInfoInput(long EmpSysId, char AttFlag, long ModifiedBy);
