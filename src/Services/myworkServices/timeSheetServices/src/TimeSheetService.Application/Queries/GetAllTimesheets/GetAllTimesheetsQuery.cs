using MediatR;
using TimeSheetService.Application.DTOs;

namespace TimeSheetService.Application.Queries.GetAllTimesheets;

public record GetAllTimesheetsQuery : IRequest<IEnumerable<TimesheetEntryDto>>;
