using CalendarService.Domain.Common;

namespace CalendarService.Domain.Entities;

public class CalendarUnitMap : BaseEntity
{
    public int CalUnitId { get; private set; }
    public int CalUnitCalenId { get; private set; }
    public int CalUnitUnitId { get; private set; }
    public DateTime CalUnitEffDate { get; private set; }
    public DateTime? CalUnitClsDate { get; private set; }
    public long LastModifiedBy { get; private set; }
    public DateTime LastModifiedOn { get; private set; }

    public CalendarMaster? Calendar { get; private set; }

    private CalendarUnitMap() { }

    public static CalendarUnitMap Create(int id, int calendarId, int unitId, DateTime effDate, long modifiedBy)
    {
        return new CalendarUnitMap
        {
            CalUnitId = id,
            CalUnitCalenId = calendarId,
            CalUnitUnitId = unitId,
            CalUnitEffDate = effDate,
            LastModifiedBy = modifiedBy,
            LastModifiedOn = DateTime.UtcNow
        };
    }
}
