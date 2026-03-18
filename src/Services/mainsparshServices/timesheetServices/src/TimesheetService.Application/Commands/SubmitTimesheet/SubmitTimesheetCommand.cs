using MediatR;
using TimesheetService.Application.DTOs;

namespace TimesheetService.Application.Commands.SubmitTimesheet;

public sealed record SubmitTimesheetCommand(long TimesheetId, long UpdatedBy) : IRequest<TimesheetDto>;
