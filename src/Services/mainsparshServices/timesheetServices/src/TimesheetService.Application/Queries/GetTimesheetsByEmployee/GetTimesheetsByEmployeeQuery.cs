using MediatR;
using TimesheetService.Application.DTOs;

namespace TimesheetService.Application.Queries.GetTimesheetsByEmployee;

public sealed record GetTimesheetsByEmployeeQuery(
    long EmployeeId,
    DateOnly? From = null,
    DateOnly? To   = null
) : IRequest<IEnumerable<TimesheetSummaryDto>>;
