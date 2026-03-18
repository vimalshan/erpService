using LeaveServices.Domain.Common;

namespace LeaveServices.Domain.Entities;

/// <summary>
/// LEAVE_MASTER – Leave type master data.
/// </summary>
public class LeaveMaster : AggregateRoot
{
    public long   LeaveId                { get; private set; }
    public string LeaveDescription       { get; private set; } = default!;
    public char   LeaveGenderSpecific    { get; private set; }
    public char   LeaveApplicableForAll  { get; private set; }
    public int    LeaveMaxDaysPL         { get; private set; }
    public char   LeaveEncashable        { get; private set; }
    public char   LeaveCarryForward      { get; private set; }
    public long   LeaveLastModifiedBy    { get; private set; }
    public DateTime LeaveLastModifiedOn  { get; private set; }

    // Navigation
    public ICollection<LeaveDetails>  LeaveDetailsList  { get; private set; } = new List<LeaveDetails>();
    public ICollection<LeaveCredit>   LeaveCreditList   { get; private set; } = new List<LeaveCredit>();
    public ICollection<LeaveRules>    LeaveRulesList    { get; private set; } = new List<LeaveRules>();

    private LeaveMaster() { }

    public static LeaveMaster Create(
        long leaveId, string description, char genderSpecific, char applicableForAll,
        int maxDaysPL, char encashable, char carryForward, long modifiedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        return new LeaveMaster
        {
            LeaveId               = leaveId,
            Id                    = leaveId,
            LeaveDescription      = description.Trim(),
            LeaveGenderSpecific   = genderSpecific,
            LeaveApplicableForAll = applicableForAll,
            LeaveMaxDaysPL        = maxDaysPL,
            LeaveEncashable       = encashable,
            LeaveCarryForward     = carryForward,
            LeaveLastModifiedBy   = modifiedBy,
            LeaveLastModifiedOn   = DateTime.UtcNow
        };
    }

    public void Update(string description, char genderSpecific, char applicableForAll,
        int maxDaysPL, char encashable, char carryForward, long modifiedBy)
    {
        LeaveDescription      = description.Trim();
        LeaveGenderSpecific   = genderSpecific;
        LeaveApplicableForAll = applicableForAll;
        LeaveMaxDaysPL        = maxDaysPL;
        LeaveEncashable       = encashable;
        LeaveCarryForward     = carryForward;
        LeaveLastModifiedBy   = modifiedBy;
        LeaveLastModifiedOn   = DateTime.UtcNow;
    }
}
