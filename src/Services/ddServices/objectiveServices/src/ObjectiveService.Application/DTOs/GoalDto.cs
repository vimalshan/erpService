namespace ObjectiveService.Application.DTOs;

public class GoalDto
{
    public decimal Id { get; set; }
    public string UserId { get; set; }
    public decimal PinNumber { get; set; }
    public DateTime PeriodFrom { get; set; }
    public DateTime PeriodTo { get; set; }
    public decimal ReferenceNumber { get; set; }
    public char FormFlag { get; set; }
    public DateTime? NextReviewDate { get; set; }
    public DateTime? ClosureDate { get; set; }
    public string Status { get; set; }
    public string AppraiserRemarks { get; set; }
    public DateTime CreatedDate { get; set; }
    public List<GoalSubGoalDto> SubGoals { get; set; } = new();
}

public class GoalSubGoalDto
{
    public decimal Id { get; set; }
    public string Description { get; set; }
    public string UnitFrom { get; set; }
    public string UnitTo { get; set; }
    public string UnitOfMeasurement { get; set; }
    public string Category { get; set; }
    public string Achievement { get; set; }
    public string Remarks { get; set; }
}

public class CreateGoalDto
{
    public string UserId { get; set; }
    public decimal PinNumber { get; set; }
    public DateTime PeriodFrom { get; set; }
    public DateTime PeriodTo { get; set; }
    public decimal ReferenceNumber { get; set; }
    public char FormFlag { get; set; }
    public List<CreateGoalSubGoalDto> SubGoals { get; set; } = new();
}

public class CreateGoalSubGoalDto
{
    public string Description { get; set; }
    public string UnitFrom { get; set; }
    public string UnitTo { get; set; }
    public string UnitOfMeasurement { get; set; }
    public string Category { get; set; }
}

public class SubmitGoalForApprovalDto
{
    public decimal GoalId { get; set; }
}

public class ApproveGoalDto
{
    public decimal GoalId { get; set; }
    public string Remarks { get; set; }
}

public class ReturnGoalDto
{
    public decimal GoalId { get; set; }
    public string Remarks { get; set; }
}
