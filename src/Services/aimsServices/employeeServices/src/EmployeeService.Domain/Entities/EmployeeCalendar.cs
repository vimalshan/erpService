using EmployeeService.Domain.Common;
using EmployeeService.Domain.ValueObjects;

namespace EmployeeService.Domain.Entities;

/// <summary>Maps to EMP_CALENDAR — employee-to-calendar mapping.</summary>
public sealed class EmployeeCalendar : BaseAuditableEntity
{
    public long EmpCalId { get; private set; }
    public EmployeeId EmpSysId { get; private set; } = null!;
    public int CalendarId { get; private set; }
    public long? SwipeId { get; private set; }
    public DateTime EffDate { get; private set; }
    public DateTime? ClsDate { get; private set; }
    public char? Status { get; private set; }
    public int? Transfer { get; private set; }
    public long? SettlementNo { get; private set; }

    private EmployeeCalendar() { }

    public static EmployeeCalendar Create(long empCalId, long empSysId, int calendarId, long mappedBy)
    {
        var entity = new EmployeeCalendar
        {
            EmpCalId = empCalId,
            EmpSysId = EmployeeId.Of(empSysId),
            CalendarId = calendarId,
            EffDate = DateTime.UtcNow,
            LastModifiedBy = mappedBy,
            LastModifiedOn = DateTime.UtcNow
        };
        entity.AddDomainEvent(new Events.CalendarMappedEvent(empCalId, empSysId, calendarId));
        return entity;
    }

    public void Close(DateTime closeDate, long modifiedBy)
    {
        ClsDate = closeDate;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }
}
