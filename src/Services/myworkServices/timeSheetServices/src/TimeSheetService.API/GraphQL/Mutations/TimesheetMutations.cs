using MediatR;
using TimeSheetService.Application.Commands.CreateTcProject;
using TimeSheetService.Application.Commands.DeleteTimesheet;
using TimeSheetService.Application.Commands.SubmitTimesheet;
using TimeSheetService.Application.Commands.SubmitTcTimesheet;
using TimeSheetService.Application.Commands.UpdateTimesheet;
using TimeSheetService.Application.DTOs;

namespace TimeSheetService.API.GraphQL.Mutations;

public class TimesheetMutations
{
    public async Task<TimesheetEntryDto> SubmitTimesheet(
        [Service] IMediator mediator,
        long timeId, long employeeSysId, DateTime timeDate,
        DateTime? timeIn, DateTime? timeOut, long totalHours,
        string? remarks, string entryTypeCode, long modifiedBy,
        CancellationToken cancellationToken)
        => await mediator.Send(new SubmitTimesheetCommand
        {
            TimeId = timeId, EmployeeSysId = employeeSysId, TimeDate = timeDate,
            TimeIn = timeIn, TimeOut = timeOut, TotalHours = totalHours,
            Remarks = remarks, EntryTypeCode = entryTypeCode, ModifiedBy = modifiedBy
        }, cancellationToken);

    public async Task<TimesheetEntryDto?> UpdateTimesheet(
        [Service] IMediator mediator,
        long timeId, DateTime? timeIn, DateTime? timeOut,
        long totalHours, string? remarks, long modifiedBy,
        CancellationToken cancellationToken)
        => await mediator.Send(new UpdateTimesheetCommand
        {
            TimeId = timeId, TimeIn = timeIn, TimeOut = timeOut,
            TotalHours = totalHours, Remarks = remarks, ModifiedBy = modifiedBy
        }, cancellationToken);

    public async Task<bool> DeleteTimesheet(
        [Service] IMediator mediator, long timeId, long modifiedBy,
        CancellationToken cancellationToken)
        => await mediator.Send(new DeleteTimesheetCommand(timeId, modifiedBy), cancellationToken);

    public async Task<TcTimesheetEntryDto> SubmitTcTimesheet(
        [Service] IMediator mediator,
        long timeId, long employeeSysId, DateTime timeDate,
        DateTime? timeIn, DateTime? timeOut, long totalHours,
        string? remarks, string entryTypeCode, long modifiedBy,
        CancellationToken cancellationToken)
        => await mediator.Send(new SubmitTcTimesheetCommand
        {
            TimeId = timeId, EmployeeSysId = employeeSysId, TimeDate = timeDate,
            TimeIn = timeIn, TimeOut = timeOut, TotalHours = totalHours,
            Remarks = remarks, EntryTypeCode = entryTypeCode, ModifiedBy = modifiedBy
        }, cancellationToken);

    public async Task<TcProjectDto> CreateTcProject(
        [Service] IMediator mediator,
        long projectId, string projectName, long categoryId,
        DateTime effectiveDate, long teamId, string listAll,
        long? oldProjectId, long modifiedBy,
        CancellationToken cancellationToken)
        => await mediator.Send(new CreateTcProjectCommand
        {
            ProjectId = projectId, ProjectName = projectName, CategoryId = categoryId,
            EffectiveDate = effectiveDate, TeamId = teamId, ListAll = listAll,
            OldProjectId = oldProjectId, ModifiedBy = modifiedBy
        }, cancellationToken);
}
