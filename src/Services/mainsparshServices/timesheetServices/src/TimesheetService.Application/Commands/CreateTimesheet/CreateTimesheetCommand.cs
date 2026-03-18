using MediatR;
using TimesheetService.Application.DTOs;

namespace TimesheetService.Application.Commands.CreateTimesheet;

public sealed record CreateTimesheetCommand(
    long EmployeeId,
    DateOnly TimesheetDate,
    DateOnly WorkDate,
    TimeOnly? StartTime,
    TimeOnly? EndTime,
    decimal? TotalHours,
    long? ProjectId,
    long? TaskId,
    string? WorkDescription,
    long CreatedBy
) : IRequest<TimesheetDto>;
