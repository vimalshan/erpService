using MediatR;
using TimesheetService.Application.DTOs;

namespace TimesheetService.Application.Queries.GetTimesheetById;

public sealed record GetTimesheetByIdQuery(long TimesheetId) : IRequest<TimesheetDto?>;
