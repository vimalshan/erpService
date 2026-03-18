using TimesheetService.Application.DTOs;
using TimesheetService.Application.Queries.GetTimesheetById;
using TimesheetService.Application.Queries.GetTimesheetsByEmployee;
using TimesheetService.Application.Queries.GetPendingTimesheets;
using MediatR;

namespace TimesheetService.API.GraphQL.Queries;

public sealed class TimesheetQuery
{
    public async Task<TimesheetDto?> GetTimesheetById(
        [Service] IMediator mediator,
        long id,
        CancellationToken cancellationToken)
        => await mediator.Send(new GetTimesheetByIdQuery(id), cancellationToken);

    public async Task<IEnumerable<TimesheetSummaryDto>> GetTimesheetsByEmployee(
        [Service] IMediator mediator,
        long employeeId,
        DateOnly? from,
        DateOnly? to,
        CancellationToken cancellationToken)
        => await mediator.Send(new GetTimesheetsByEmployeeQuery(employeeId, from, to), cancellationToken);

    public async Task<IEnumerable<TimesheetSummaryDto>> GetPendingTimesheets(
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
        => await mediator.Send(new GetPendingTimesheetsQuery(), cancellationToken);
}
