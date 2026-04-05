using MediatR;

namespace TimeSheetService.Application.Commands.DeleteTimesheet;

public record DeleteTimesheetCommand(long TimeId, long ModifiedBy) : IRequest<bool>;
