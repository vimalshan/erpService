using ObjectiveService.Application.Common;

namespace ObjectiveService.Application.Features.Goals.Commands;

public class CreateGoalCommand : CommandBase<CommandResult<decimal>>
{
    public string UserId { get; set; }
    public decimal PinNumber { get; set; }
    public DateTime PeriodFrom { get; set; }
    public DateTime PeriodTo { get; set; }
    public decimal ReferenceNumber { get; set; }
    public char FormFlag { get; set; }
    public List<CreateGoalSubGoalItem> SubGoals { get; set; } = new();
}

public class CreateGoalSubGoalItem
{
    public string Description { get; set; }
    public string UnitFrom { get; set; }
    public string UnitTo { get; set; }
    public string UnitOfMeasurement { get; set; }
    public string Category { get; set; }
}

public class SubmitGoalForApprovalCommand : CommandBase<CommandResult>
{
    public decimal GoalId { get; set; }
}

public class ApproveGoalCommand : CommandBase<CommandResult>
{
    public decimal GoalId { get; set; }
    public string Remarks { get; set; }
}

public class ReturnGoalCommand : CommandBase<CommandResult>
{
    public decimal GoalId { get; set; }
    public string Remarks { get; set; }
}

public class CloseGoalCommand : CommandBase<CommandResult>
{
    public decimal GoalId { get; set; }
}

public class RecordGoalAchievementCommand : CommandBase<CommandResult>
{
    public decimal GoalSubGoalId { get; set; }
    public string Achievement { get; set; }
    public string Difference { get; set; }
}
