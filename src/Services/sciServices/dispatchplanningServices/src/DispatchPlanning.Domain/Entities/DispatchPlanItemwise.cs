using DispatchPlanning.Domain.Common;
using DispatchPlanning.Domain.ValueObjects;

namespace DispatchPlanning.Domain.Entities;

public class DispatchPlanItemwise : Entity
{
    public int DispatchPlanHeaderId { get; private set; }
    public int BreakupItemId { get; private set; }
    public long? TargetWeek1 { get; private set; }
    public long? TargetWeek2 { get; private set; }
    public long? TargetWeek3 { get; private set; }
    public long? TargetWeek4 { get; private set; }
    public long? TargetWeek5 { get; private set; }
    public long? TargetMPlus1 { get; private set; }
    public long? TargetMPlus2 { get; private set; }
    public long? TargetMPlus3 { get; private set; }
    public long? TargetMPlus4 { get; private set; }
    public int SciUserIdModified { get; private set; }
    public DateTime ModifiedDate { get; private set; }

    private DispatchPlanItemwise() { }

    public static DispatchPlanItemwise Create(int headerId, int breakupItemId,
        TargetWeeks targets, int modifiedBy)
    {
        return new DispatchPlanItemwise
        {
            DispatchPlanHeaderId = headerId,
            BreakupItemId = breakupItemId,
            TargetWeek1 = targets.Week1,
            TargetWeek2 = targets.Week2,
            TargetWeek3 = targets.Week3,
            TargetWeek4 = targets.Week4,
            TargetWeek5 = targets.Week5,
            TargetMPlus1 = targets.MPlus1,
            TargetMPlus2 = targets.MPlus2,
            TargetMPlus3 = targets.MPlus3,
            TargetMPlus4 = targets.MPlus4,
            SciUserIdModified = modifiedBy,
            ModifiedDate = DateTime.UtcNow
        };
    }

    public void UpdateTargets(TargetWeeks targets, int modifiedBy)
    {
        TargetWeek1 = targets.Week1;
        TargetWeek2 = targets.Week2;
        TargetWeek3 = targets.Week3;
        TargetWeek4 = targets.Week4;
        TargetWeek5 = targets.Week5;
        TargetMPlus1 = targets.MPlus1;
        TargetMPlus2 = targets.MPlus2;
        TargetMPlus3 = targets.MPlus3;
        TargetMPlus4 = targets.MPlus4;
        SciUserIdModified = modifiedBy;
        ModifiedDate = DateTime.UtcNow;
    }
}
