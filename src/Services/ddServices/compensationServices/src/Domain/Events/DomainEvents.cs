namespace CompensationService.Domain.Events;

/// <summary>
/// Raised when a budget is updated.
/// </summary>
public class BudgetUpdatedEvent : DomainEvent
{
    /// <summary>Gets the budget ID.</summary>
    public decimal BudgetId { get; }

    /// <summary>Gets the new budget amount.</summary>
    public decimal Amount { get; }

    /// <summary>Gets who updated the budget.</summary>
    public decimal UpdatedBy { get; }

    /// <summary>Gets when the budget was updated.</summary>
    public DateTime UpdatedOn { get; }

    /// <summary>Initializes a new instance.</summary>
    public BudgetUpdatedEvent(decimal budgetId, decimal amount, decimal updatedBy, DateTime updatedOn)
    {
        BudgetId = budgetId;
        Amount = amount;
        UpdatedBy = updatedBy;
        UpdatedOn = updatedOn;
    }
}

/// <summary>
/// Raised when a compensation level is closed.
/// </summary>
public class CompensationLevelClosedEvent : DomainEvent
{
    /// <summary>Gets the level ID.</summary>
    public decimal LevelId { get; }

    /// <summary>Gets when the level was closed.</summary>
    public DateTime ClosedOn { get; }

    /// <summary>Initializes a new instance.</summary>
    public CompensationLevelClosedEvent(decimal levelId, DateTime closedOn)
    {
        LevelId = levelId;
        ClosedOn = closedOn;
    }
}

/// <summary>
/// Raised when a circular is generated for a period.
/// </summary>
public class CircularGeneratedEvent : DomainEvent
{
    /// <summary>Gets the period ID.</summary>
    public decimal PeriodId { get; }

    /// <summary>Gets who generated the circular.</summary>
    public decimal GeneratedBy { get; }

    /// <summary>Gets when the circular was generated.</summary>
    public DateTime GeneratedOn { get; }

    /// <summary>Initializes a new instance.</summary>
    public CircularGeneratedEvent(decimal periodId, decimal generatedBy, DateTime generatedOn)
    {
        PeriodId = periodId;
        GeneratedBy = generatedBy;
        GeneratedOn = generatedOn;
    }
}

/// <summary>
/// Raised when a period is confirmed to payroll.
/// </summary>
public class PeriodConfirmedToPayrollEvent : DomainEvent
{
    /// <summary>Gets the period ID.</summary>
    public decimal PeriodId { get; }

    /// <summary>Gets when the period was confirmed.</summary>
    public DateTime ConfirmedOn { get; }

    /// <summary>Initializes a new instance.</summary>
    public PeriodConfirmedToPayrollEvent(decimal periodId, DateTime confirmedOn)
    {
        PeriodId = periodId;
        ConfirmedOn = confirmedOn;
    }
}

/// <summary>
/// Raised when a recommendation is created.
/// </summary>
public class RecommendationCreatedEvent : DomainEvent
{
    /// <summary>Gets the recommendation ID.</summary>
    public decimal RecommendationId { get; }

    /// <summary>Gets the employee system ID.</summary>
    public decimal EmployeeSystemId { get; }

    /// <summary>Gets when the recommendation was created.</summary>
    public DateTime CreatedOn { get; }

    /// <summary>Initializes a new instance.</summary>
    public RecommendationCreatedEvent(decimal recommendationId, decimal employeeSystemId, DateTime createdOn)
    {
        RecommendationId = recommendationId;
        EmployeeSystemId = employeeSystemId;
        CreatedOn = createdOn;
    }
}

/// <summary>
/// Raised when a recommendation is submitted.
/// </summary>
public class RecommendationSubmittedEvent : DomainEvent
{
    /// <summary>Gets the recommendation ID.</summary>
    public decimal RecommendationId { get; }

    /// <summary>Gets the role that submitted it.</summary>
    public string SubmittedRole { get; }

    /// <summary>Gets who submitted it.</summary>
    public decimal SubmittedBy { get; }

    /// <summary>Gets when it was submitted.</summary>
    public DateTime SubmittedOn { get; }

    /// <summary>Initializes a new instance.</summary>
    public RecommendationSubmittedEvent(decimal recommendationId, string submittedRole, decimal submittedBy, DateTime submittedOn)
    {
        RecommendationId = recommendationId;
        SubmittedRole = submittedRole;
        SubmittedBy = submittedBy;
        SubmittedOn = submittedOn;
    }
}

/// <summary>
/// Raised when a recommendation is approved.
/// </summary>
public class RecommendationApprovedEvent : DomainEvent
{
    /// <summary>Gets the recommendation ID.</summary>
    public decimal RecommendationId { get; }

    /// <summary>Gets the final level.</summary>
    public decimal FinalLevel { get; }

    /// <summary>Gets the final amount.</summary>
    public decimal FinalAmount { get; }

    /// <summary>Gets when it was approved.</summary>
    public DateTime ApprovedOn { get; }

    /// <summary>Initializes a new instance.</summary>
    public RecommendationApprovedEvent(decimal recommendationId, decimal finalLevel, decimal finalAmount, DateTime approvedOn)
    {
        RecommendationId = recommendationId;
        FinalLevel = finalLevel;
        FinalAmount = finalAmount;
        ApprovedOn = approvedOn;
    }
}

/// <summary>
/// Raised when a recommendation is rejected.
/// </summary>
public class RecommendationRejectedEvent : DomainEvent
{
    /// <summary>Gets the recommendation ID.</summary>
    public decimal RecommendationId { get; }

    /// <summary>Gets who rejected it.</summary>
    public decimal RejectedBy { get; }

    /// <summary>Gets when it was rejected.</summary>
    public DateTime RejectedOn { get; }

    /// <summary>Gets the rejection remarks.</summary>
    public string RejectionRemarks { get; }

    /// <summary>Initializes a new instance.</summary>
    public RecommendationRejectedEvent(decimal recommendationId, decimal rejectedBy, DateTime rejectedOn, string rejectionRemarks)
    {
        RecommendationId = recommendationId;
        RejectedBy = rejectedBy;
        RejectedOn = rejectedOn;
        RejectionRemarks = rejectionRemarks;
    }
}
