using MediatR;
using TimeSheetService.Application.DTOs;

namespace TimeSheetService.Application.Queries.GetTimesheetById;

public record GetTimesheetByIdQuery(long TimeId) : IRequest<TimesheetEntryDto?>;
