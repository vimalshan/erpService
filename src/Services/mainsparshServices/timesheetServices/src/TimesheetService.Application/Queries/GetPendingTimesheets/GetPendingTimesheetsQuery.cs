using MediatR;
using TimesheetService.Application.DTOs;

namespace TimesheetService.Application.Queries.GetPendingTimesheets;

public sealed record GetPendingTimesheetsQuery : IRequest<IEnumerable<TimesheetSummaryDto>>;
