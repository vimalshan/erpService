using LeaveServices.Domain.Common;

namespace LeaveServices.Domain.Entities;

/// <summary>
/// LEAVE_RULES – Leave policy rules per leave type.
/// </summary>
public class LeaveRules : AggregateRoot
{
    public int   RuleId                { get; private set; }
    public long  RuleLeaveId           { get; private set; }
    public int   RuleMaxDaysInAppl     { get; private set; }
    public int   RuleMinDaysInAppl     { get; private set; }
    public int   RuleMaxYearLimit      { get; private set; }
    public char  RuleClubbing          { get; private set; }
    public long  RuleLastModifiedBy    { get; private set; }
    public DateTime RuleLastModifiedOn { get; private set; }

    public LeaveMaster? LeaveMaster { get; private set; }

    private LeaveRules() { }

    public static LeaveRules Create(
        int ruleId, long leaveId, int maxDays, int minDays, int maxYear, char clubbing, long modifiedBy)
    {
        return new LeaveRules
        {
            RuleId              = ruleId,
            Id                  = ruleId,
            RuleLeaveId         = leaveId,
            RuleMaxDaysInAppl   = maxDays,
            RuleMinDaysInAppl   = minDays,
            RuleMaxYearLimit    = maxYear,
            RuleClubbing        = clubbing,
            RuleLastModifiedBy  = modifiedBy,
            RuleLastModifiedOn  = DateTime.UtcNow
        };
    }
}
