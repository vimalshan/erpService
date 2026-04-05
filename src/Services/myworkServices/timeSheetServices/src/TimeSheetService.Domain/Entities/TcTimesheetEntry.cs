using TimeSheetService.Domain.Common;
using TimeSheetService.Domain.Events;
using TimeSheetService.Domain.ValueObjects;

namespace TimeSheetService.Domain.Entities;

/// <summary>Maps to TCTIMESHEET_MAIN - Time Collection module timesheet</summary>
public class TcTimesheetEntry : AggregateRoot
{
    private readonly List<TcTimesheetDetail> _details = new();

    public long TimeId => Id;
    public long EmployeeSysId { get; private set; }
    public DateTime TimeDate { get; private set; }
    public DateTime? TimeIn { get; private set; }
    public DateTime? TimeOut { get; private set; }
    public long TotalHours { get; private set; }
    public string? Remarks { get; private set; }
    public EntryType EntryType { get; private set; } = EntryType.Self;
    public IReadOnlyCollection<TcTimesheetDetail> Details => _details.AsReadOnly();

    private TcTimesheetEntry() { } // EF

    public TcTimesheetEntry(long timeId, long employeeSysId, DateTime timeDate,
        DateTime? timeIn, DateTime? timeOut, long totalHours,
        string? remarks, EntryType entryType, long modifiedBy)
    {
        Id = timeId;
        EmployeeSysId = employeeSysId;
        TimeDate = timeDate;
        TimeIn = timeIn;
        TimeOut = timeOut;
        TotalHours = totalHours;
        Remarks = remarks;
        EntryType = entryType;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;

        AddDomainEvent(new TcTimesheetSubmittedEvent(timeId, employeeSysId, timeDate, totalHours));
    }

    public TcTimesheetDetail AddDetail(long detailId, long hours, long projectId,
        long subCategoryId, string? remarks, long? callNo, long modifiedBy)
    {
        var detail = new TcTimesheetDetail(detailId, Id, hours, projectId, subCategoryId, remarks, callNo, modifiedBy);
        _details.Add(detail);
        return detail;
    }
}
