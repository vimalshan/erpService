using MediatR;
using TimeSheetService.Application.DTOs;
using TimeSheetService.Domain.ValueObjects;

namespace TimeSheetService.Application.Commands.SubmitTimesheet;

public class SubmitTimesheetCommand : IRequest<TimesheetEntryDto>
{
    public long TimeId { get; init; }
    public long EmployeeSysId { get; init; }
    public DateTime TimeDate { get; init; }
    public DateTime? TimeIn { get; init; }
    public DateTime? TimeOut { get; init; }
    public long TotalHours { get; init; }
    public string? Remarks { get; init; }
    public string EntryTypeCode { get; init; } = "S";
    public long ModifiedBy { get; init; }
    public List<TimesheetDetailInput> Details { get; init; } = [];
}

public record TimesheetDetailInput(
    long DetailId, long Hours, long ProjectId, long SubCategoryId, string? Remarks, long CallNo);
