using CalendarService.Domain.Common;

namespace CalendarService.Domain.Entities;

public class CalendarGraceRange : BaseEntity
{
    public int CalGraceId { get; private set; }
    public int CalGraceCalenId { get; private set; }
    public int CalGraceGraceId { get; private set; }
    public int CalGraceGraceTime { get; private set; }
    public long LastModifiedBy { get; private set; }
    public DateTime LastModifiedOn { get; private set; }

    public CalendarMaster? Calendar { get; private set; }

    private CalendarGraceRange() { }

    public static CalendarGraceRange Create(int id, int calendarId, int graceId, int graceTime, long modifiedBy)
    {
        return new CalendarGraceRange
        {
            CalGraceId = id,
            CalGraceCalenId = calendarId,
            CalGraceGraceId = graceId,
            CalGraceGraceTime = graceTime,
            LastModifiedBy = modifiedBy,
            LastModifiedOn = DateTime.UtcNow
        };
    }
}
