using MediatR;
using TimeSheetService.Application.DTOs;
using TimeSheetService.Application.Queries.GetAllTimesheets;
using TimeSheetService.Application.Queries.GetTimesheetById;
using TimeSheetService.Application.Queries.GetTimesheetsByEmployee;
using TimeSheetService.Application.Queries.GetTcProjects;
using TimeSheetService.Application.Queries.GetTsProjects;

namespace TimeSheetService.API.GraphQL.Queries;

public class TimesheetQueries
{
    public async Task<IEnumerable<TimesheetEntryDto>> GetTimesheets(
        [Service] IMediator mediator, CancellationToken cancellationToken)
        => await mediator.Send(new GetAllTimesheetsQuery(), cancellationToken);

    public async Task<TimesheetEntryDto?> GetTimesheet(
        [Service] IMediator mediator, long timeId, CancellationToken cancellationToken)
        => await mediator.Send(new GetTimesheetByIdQuery(timeId), cancellationToken);

    public async Task<IEnumerable<TimesheetEntryDto>> GetTimesheetsByEmployee(
        [Service] IMediator mediator, long employeeSysId,
        DateTime? from, DateTime? to, CancellationToken cancellationToken)
        => await mediator.Send(new GetTimesheetsByEmployeeQuery(employeeSysId, from, to), cancellationToken);

    public async Task<IEnumerable<TcProjectDto>> GetTcProjects(
        [Service] IMediator mediator, CancellationToken cancellationToken)
        => await mediator.Send(new GetTcProjectsQuery(), cancellationToken);

    public async Task<IEnumerable<TsProjectDto>> GetTsProjects(
        [Service] IMediator mediator, CancellationToken cancellationToken)
        => await mediator.Send(new GetTsProjectsQuery(), cancellationToken);
}
