using CalendarService.Domain.Common;

namespace CalendarService.Domain.Entities;

public class ShiftTimeMaster : BaseEntity
{
    public int ShiftTimeId { get; private set; }
    public int ShiftTimeShiftId { get; private set; }
    public TimeOnly ShiftTimeInTime { get; private set; }
    public TimeOnly ShiftTimeOutTime { get; private set; }
    public long LastModifiedBy { get; private set; }
    public DateTime LastModifiedOn { get; private set; }

    public ShiftMaster? Shift { get; private set; }

    private ShiftTimeMaster() { }

    public static ShiftTimeMaster Create(int id, int shiftId, TimeOnly inTime, TimeOnly outTime, long modifiedBy)
    {
        return new ShiftTimeMaster
        {
            ShiftTimeId = id,
            ShiftTimeShiftId = shiftId,
            ShiftTimeInTime = inTime,
            ShiftTimeOutTime = outTime,
            LastModifiedBy = modifiedBy,
            LastModifiedOn = DateTime.UtcNow
        };
    }
}
