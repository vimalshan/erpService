using MediatR;
using TimesheetService.Application.DTOs;

namespace TimesheetService.Application.Commands.RejectTimesheet;

public sealed record RejectTimesheetCommand(long TimesheetId, long ApproverId, string RejectionReason) : IRequest<TimesheetDto>;
