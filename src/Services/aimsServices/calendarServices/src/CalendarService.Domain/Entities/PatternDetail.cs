using CalendarService.Domain.Common;

namespace CalendarService.Domain.Entities;

public class PatternDetail : BaseEntity
{
    public int PatDetId { get; private set; }
    public int PatDetPatternId { get; private set; }
    public int PatDetDayNo { get; private set; }
    public int PatDetShiftId { get; private set; }
    public long LastModifiedBy { get; private set; }
    public DateTime LastModifiedOn { get; private set; }

    public PatternMaster? Pattern { get; private set; }
    public ShiftMaster? Shift { get; private set; }

    private PatternDetail() { }

    public static PatternDetail Create(int id, int patternId, int dayNo, int shiftId, long modifiedBy)
    {
        return new PatternDetail
        {
            PatDetId = id,
            PatDetPatternId = patternId,
            PatDetDayNo = dayNo,
            PatDetShiftId = shiftId,
            LastModifiedBy = modifiedBy,
            LastModifiedOn = DateTime.UtcNow
        };
    }
}
