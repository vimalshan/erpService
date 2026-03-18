namespace ObjectiveService.Domain.Entities;

/// <summary>
/// Goal entity representing performance goals for employees
/// </summary>
public class Goal : BaseEntity
{
    public string UserId { get; set; }
    public decimal PinNumber { get; set; }
    public DateTime PeriodFrom { get; set; }
    public DateTime PeriodTo { get; set; }
    public decimal ReferenceNumber { get; set; }
    public char FormFlag { get; set; } // D-For DD, C-Confirmation
    public DateTime? NextReviewDate { get; set; }
    public DateTime? ClosureDate { get; set; }
    public string Status { get; set; } // N-with appraisee, Y-completed, C-completed with appraisers, A-feedback accepted
    public string? AppraiserRemarks { get; set; }
    public bool HasAttachment { get; set; }
    public string? AttachmentUrl { get; set; }
    public DateTime CreatedDate { get; set; }

    private readonly List<GoalSubGoal> _subGoals = new();
    public IReadOnlyList<GoalSubGoal> SubGoals => _subGoals.AsReadOnly();

    private Goal() { }

    public Goal(
        string userId,
        decimal pinNumber,
        DateTime periodFrom,
        DateTime periodTo,
        decimal referenceNumber,
        char formFlag)
    {
        UserId = userId;
        PinNumber = pinNumber;
        PeriodFrom = periodFrom;
        PeriodTo = periodTo;
        ReferenceNumber = referenceNumber;
        FormFlag = formFlag;
        Status = "N"; // Initially with appraisee
        CreatedDate = DateTime.UtcNow;

        RaiseDomainEvent(new GoalCreatedDomainEvent(Id, userId, periodFrom, periodTo));
    }

    public void AddSubGoal(GoalSubGoal subGoal)
    {
        if (subGoal == null)
            throw new ArgumentNullException(nameof(subGoal));

        _subGoals.Add(subGoal);
    }

    public void SubmitForApproval(DateTime submittedDate)
    {
        Status = "Y";
        ModifiedDate = DateTime.UtcNow;

        RaiseDomainEvent(new GoalSubmittedForApprovalDomainEvent(Id, UserId));
    }

    public void ApproveGoal()
    {
        Status = "A";
        ModifiedDate = DateTime.UtcNow;

        RaiseDomainEvent(new GoalApprovedDomainEvent(Id, UserId));
    }

    public void ReturnGoal(string remarks)
    {
        Status = "R";
        AppraiserRemarks = remarks;
        ModifiedDate = DateTime.UtcNow;

        RaiseDomainEvent(new GoalReturnedDomainEvent(Id, UserId, remarks));
    }

    public void CloseGoal()
    {
        ClosureDate = DateTime.UtcNow;
        Status = "C";
        ModifiedDate = DateTime.UtcNow;

        RaiseDomainEvent(new GoalClosedDomainEvent(Id, UserId));
    }

    public DateTime? ModifiedDate { get; private set; }
}

/// <summary>
/// Represents a sub-goal or control point within a goal
/// </summary>
public class GoalSubGoal : BaseEntity
{
    public decimal GoalId { get; set; }
    public string Description { get; set; }
    public string UnitFrom { get; set; }
    public string UnitTo { get; set; }
    public string? Achievement { get; set; }
    public string? Difference { get; set; }
    public string? ExpectationCode { get; set; }
    public string? GoalFlag { get; set; }
    public decimal? ModificationSerialNumber { get; set; }
    public string UnitOfMeasurement { get; set; }
    public string Category { get; set; }
    public string? Remarks { get; set; }

    private GoalSubGoal() { }

    public GoalSubGoal(
        decimal goalId,
        string description,
        string unitFrom,
        string unitTo,
        string unitOfMeasurement,
        string category)
    {
        GoalId = goalId;
        Description = description;
        UnitFrom = unitFrom;
        UnitTo = unitTo;
        UnitOfMeasurement = unitOfMeasurement;
        Category = category;
    }

    public void RecordAchievement(string achievement, string difference)
    {
        Achievement = achievement;
        Difference = difference;

        RaiseDomainEvent(new GoalAchievementRecordedDomainEvent(GoalId, Id, achievement));
    }
}

// Domain Events
public class GoalCreatedDomainEvent : DomainEventBase
{
    public decimal GoalId { get; }
    public string UserId { get; }
    public DateTime PeriodFrom { get; }
    public DateTime PeriodTo { get; }

    public GoalCreatedDomainEvent(decimal goalId, string userId, DateTime periodFrom, DateTime periodTo)
    {
        GoalId = goalId;
        UserId = userId;
        PeriodFrom = periodFrom;
        PeriodTo = periodTo;
    }
}

public class GoalSubmittedForApprovalDomainEvent : DomainEventBase
{
    public decimal GoalId { get; }
    public string UserId { get; }

    public GoalSubmittedForApprovalDomainEvent(decimal goalId, string userId)
    {
        GoalId = goalId;
        UserId = userId;
    }
}

public class GoalApprovedDomainEvent : DomainEventBase
{
    public decimal GoalId { get; }
    public string UserId { get; }

    public GoalApprovedDomainEvent(decimal goalId, string userId)
    {
        GoalId = goalId;
        UserId = userId;
    }
}

public class GoalReturnedDomainEvent : DomainEventBase
{
    public decimal GoalId { get; }
    public string UserId { get; }
    public string Remarks { get; }

    public GoalReturnedDomainEvent(decimal goalId, string userId, string remarks)
    {
        GoalId = goalId;
        UserId = userId;
        Remarks = remarks;
    }
}

public class GoalClosedDomainEvent : DomainEventBase
{
    public decimal GoalId { get; }
    public string UserId { get; }

    public GoalClosedDomainEvent(decimal goalId, string userId)
    {
        GoalId = goalId;
        UserId = userId;
    }
}

public class GoalAchievementRecordedDomainEvent : DomainEventBase
{
    public decimal GoalId { get; }
    public decimal SubGoalId { get; }
    public string Achievement { get; }

    public GoalAchievementRecordedDomainEvent(decimal goalId, decimal subGoalId, string achievement)
    {
        GoalId = goalId;
        SubGoalId = subGoalId;
        Achievement = achievement;
    }
}
