using CalendarService.Domain.Common;
using CalendarService.Domain.Events;
using CalendarService.Domain.ValueObjects;

namespace CalendarService.Domain.Entities;

public class CalendarMaster : BaseEntity
{
    public int CalendarId { get; private set; }
    public string CalendarName { get; private set; } = string.Empty;
    public int CalendarUnitId { get; private set; }
    public DateTime CalendarEffDate { get; private set; }
    public DateTime? CalendarClsDate { get; private set; }
    public CalendarStatus Status { get; private set; }
    public long LastModifiedBy { get; private set; }
    public DateTime LastModifiedOn { get; private set; }

    // Navigation
    public ICollection<CalendarUnitMap> UnitMaps { get; private set; } = [];
    public ICollection<CalendarRoundRange> RoundRanges { get; private set; } = [];
    public ICollection<CalendarGraceRange> GraceRanges { get; private set; } = [];

    private CalendarMaster() { }

    public static CalendarMaster Create(int id, string name, int unitId, DateTime effDate, long modifiedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        var entity = new CalendarMaster
        {
            CalendarId = id,
            CalendarName = name,
            CalendarUnitId = unitId,
            CalendarEffDate = effDate,
            Status = CalendarStatus.Active,
            LastModifiedBy = modifiedBy,
            LastModifiedOn = DateTime.UtcNow
        };

        entity.RaiseDomainEvent(new CalendarCreatedEvent(entity.CalendarId, entity.CalendarName));
        return entity;
    }

    public void Update(string name, int unitId, long modifiedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        CalendarName = name;
        CalendarUnitId = unitId;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }

    public void Close(DateTime closeDate, long modifiedBy)
    {
        CalendarClsDate = closeDate;
        Status = CalendarStatus.Closed;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }
}
