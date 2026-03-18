using MediatR;
using TimesheetService.Application.Commands.ApproveTimesheet;
using TimesheetService.Application.Commands.CreateTimesheet;
using TimesheetService.Application.Commands.RejectTimesheet;
using TimesheetService.Application.Commands.SubmitTimesheet;
using TimesheetService.Application.DTOs;

namespace TimesheetService.API.GraphQL.Mutations;

public sealed class TimesheetMutation
{
    public async Task<TimesheetDto> CreateTimesheet(
        [Service] IMediator mediator,
        long employeeId,
        DateOnly timesheetDate,
        DateOnly workDate,
        TimeOnly? startTime,
        TimeOnly? endTime,
        decimal? totalHours,
        long? projectId,
        long? taskId,
        string? workDescription,
        long createdBy,
        CancellationToken cancellationToken)
        => await mediator.Send(new CreateTimesheetCommand(
            employeeId, timesheetDate, workDate, startTime, endTime,
            totalHours, projectId, taskId, workDescription, createdBy), cancellationToken);

    public async Task<TimesheetDto> SubmitTimesheet(
        [Service] IMediator mediator,
        long timesheetId,
        long updatedBy,
        CancellationToken cancellationToken)
        => await mediator.Send(new SubmitTimesheetCommand(timesheetId, updatedBy), cancellationToken);

    public async Task<TimesheetDto> ApproveTimesheet(
        [Service] IMediator mediator,
        long timesheetId,
        long approverId,
        CancellationToken cancellationToken)
        => await mediator.Send(new ApproveTimesheetCommand(timesheetId, approverId), cancellationToken);

    public async Task<TimesheetDto> RejectTimesheet(
        [Service] IMediator mediator,
        long timesheetId,
        long approverId,
        string rejectionReason,
        CancellationToken cancellationToken)
        => await mediator.Send(new RejectTimesheetCommand(timesheetId, approverId, rejectionReason), cancellationToken);
}
