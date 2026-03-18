using MasterService.Domain.Common;

namespace MasterService.Domain.Entities;

/// <summary>Reference: GOAL_MAST</summary>
public sealed class Goal : AggregateRoot
{
    public string GoalCode { get; private set; } = string.Empty;
    public string GoalName { get; private set; } = string.Empty;

    private Goal() { }

    public static Goal Create(string goalCode, string goalName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(goalCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(goalName);
        return new Goal { GoalCode = goalCode.Trim().ToUpper(), GoalName = goalName.Trim() };
    }
}
