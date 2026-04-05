using MediatR;
using TimeSheetService.Application.DTOs;

namespace TimeSheetService.Application.Commands.SubmitTcTimesheet;

public class SubmitTcTimesheetCommand : IRequest<TcTimesheetEntryDto>
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
    public List<TcTimesheetDetailInput> Details { get; init; } = [];
}

public record TcTimesheetDetailInput(
    long DetailId, long Hours, long ProjectId, long SubCategoryId, string? Remarks, long? CallNo);
