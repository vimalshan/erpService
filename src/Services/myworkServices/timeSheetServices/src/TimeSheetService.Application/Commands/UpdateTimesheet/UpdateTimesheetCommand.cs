using MediatR;
using TimeSheetService.Application.DTOs;

namespace TimeSheetService.Application.Commands.UpdateTimesheet;

public class UpdateTimesheetCommand : IRequest<TimesheetEntryDto>
{
    public long TimeId { get; init; }
    public DateTime? TimeIn { get; init; }
    public DateTime? TimeOut { get; init; }
    public long TotalHours { get; init; }
    public string? Remarks { get; init; }
    public long ModifiedBy { get; init; }
}
