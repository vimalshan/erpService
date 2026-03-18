using CalendarService.Domain.Common;

namespace CalendarService.Domain.Entities;

public class CalendarRoundRange : BaseEntity
{
    public int CalRoundId { get; private set; }
    public int CalRoundCalenId { get; private set; }
    public int CalRoundRoundNo { get; private set; }
    public int CalRoundRoundFrom { get; private set; }
    public int CalRoundRoundTo { get; private set; }
    public int CalRoundWorking { get; private set; }
    public long LastModifiedBy { get; private set; }
    public DateTime LastModifiedOn { get; private set; }

    public CalendarMaster? Calendar { get; private set; }

    private CalendarRoundRange() { }

    public static CalendarRoundRange Create(int id, int calendarId, int roundNo, int roundFrom, int roundTo, int working, long modifiedBy)
    {
        return new CalendarRoundRange
        {
            CalRoundId = id,
            CalRoundCalenId = calendarId,
            CalRoundRoundNo = roundNo,
            CalRoundRoundFrom = roundFrom,
            CalRoundRoundTo = roundTo,
            CalRoundWorking = working,
            LastModifiedBy = modifiedBy,
            LastModifiedOn = DateTime.UtcNow
        };
    }
}
