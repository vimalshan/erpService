using CalendarService.Domain.Common;

namespace CalendarService.Domain.Entities;

public class ShiftException : BaseEntity
{
    public int ShiftExcId { get; private set; }
    public int ShiftExcShiftId { get; private set; }
    public DateTime ShiftExcEffDate { get; private set; }
    public DateTime? ShiftExcClsDate { get; private set; }
    public int ShiftExcNewShiftId { get; private set; }
    public long LastModifiedBy { get; private set; }
    public DateTime LastModifiedOn { get; private set; }

    public ShiftMaster? Shift { get; private set; }
    public ShiftMaster? NewShift { get; private set; }

    private ShiftException() { }

    public static ShiftException Create(int id, int shiftId, DateTime effDate, int newShiftId, long modifiedBy, DateTime? clsDate = null)
    {
        return new ShiftException
        {
            ShiftExcId = id,
            ShiftExcShiftId = shiftId,
            ShiftExcEffDate = effDate,
            ShiftExcClsDate = clsDate,
            ShiftExcNewShiftId = newShiftId,
            LastModifiedBy = modifiedBy,
            LastModifiedOn = DateTime.UtcNow
        };
    }
}
