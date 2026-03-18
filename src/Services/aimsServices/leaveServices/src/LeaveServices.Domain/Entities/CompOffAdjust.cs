using LeaveServices.Domain.Common;

namespace LeaveServices.Domain.Entities;

/// <summary>
/// COMPOFF_ADJUST – Compensatory off adjustment per employee.
/// </summary>
public class CompOffAdjust : AggregateRoot
{
    public long     CompOffId             { get; private set; }
    public long     CompOffEmpSysId       { get; private set; }
    public DateTime CompOffCompOffDate    { get; private set; }
    public DateTime? CompOffUsedDate      { get; private set; }
    public string   CompOffStatus         { get; private set; } = default!;
    public long     CompOffLastModifiedBy { get; private set; }
    public DateTime CompOffLastModifiedOn { get; private set; }

    private CompOffAdjust() { }

    public static CompOffAdjust Create(long compOffId, long empSysId, DateTime compOffDate, long modifiedBy)
    {
        return new CompOffAdjust
        {
            CompOffId             = compOffId,
            Id                    = compOffId,
            CompOffEmpSysId       = empSysId,
            CompOffCompOffDate    = compOffDate,
            CompOffUsedDate       = null,
            CompOffStatus         = "A",    // Available
            CompOffLastModifiedBy = modifiedBy,
            CompOffLastModifiedOn = DateTime.UtcNow
        };
    }

    public void MarkUsed(DateTime usedDate, long modifiedBy)
    {
        CompOffStatus         = "U";
        CompOffUsedDate       = usedDate;
        CompOffLastModifiedBy = modifiedBy;
        CompOffLastModifiedOn = DateTime.UtcNow;
    }
}
