using MediatR;
using TimesheetService.Application.DTOs;

namespace TimesheetService.Application.Commands.ApproveTimesheet;

public sealed record ApproveTimesheetCommand(long TimesheetId, long ApproverId) : IRequest<TimesheetDto>;
