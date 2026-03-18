namespace CompensationService.Domain.Entities;

using CompensationService.Domain.ValueObjects;
using CompensationService.Domain.Events;

/// <summary>
/// Represents a compensation budget entity.
/// </summary>
public class Budget : BaseEntity
{
    /// <summary>Gets or sets the business ID.</summary>
    public decimal BusinessId { get; set; }

    /// <summary>Gets or sets the year ID.</summary>
    public decimal YearId { get; set; }

    /// <summary>Gets or sets the budget amount.</summary>
    public MoneyAmount BudgetAmount { get; set; } = null!;

    /// <summary>Gets or sets the user who last updated the budget.</summary>
    public decimal UpdatedBy { get; set; }

    /// <summary>Gets or sets when the budget was last updated.</summary>
    public DateTime UpdatedOn { get; set; }

    /// <summary>Private constructor for EF Core.</summary>
    private Budget() { }

    /// <summary>Creates a new budget.</summary>
    public static Budget Create(decimal id, decimal businessId, decimal yearId, decimal amount, decimal updatedBy, DateTime updatedOn)
    {
        var budget = new Budget
        {
            Id = id,
            BusinessId = businessId,
            YearId = yearId,
            BudgetAmount = MoneyAmount.Create(amount),
            UpdatedBy = updatedBy,
            UpdatedOn = updatedOn
        };

        return budget;
    }

    /// <summary>Updates the budget amount.</summary>
    public void UpdateAmount(MoneyAmount newAmount, decimal updatedBy, DateTime updatedOn)
    {
        BudgetAmount = newAmount;
        UpdatedBy = updatedBy;
        UpdatedOn = updatedOn;
        AddDomainEvent(new BudgetUpdatedEvent(Id, newAmount.Amount, updatedBy, updatedOn));
    }
}

/// <summary>
/// Represents a compensation level entity.
/// </summary>
public class CompensationLevel : BaseEntity
{
    /// <summary>Gets or sets the level description.</summary>
    public string LevelDesc { get; set; } = null!;

    /// <summary>Gets or sets the level amount.</summary>
    public string LevelAmount { get; set; } = null!;

    /// <summary>Gets or sets the level reason.</summary>
    public string LevelReason { get; set; } = null!;

    /// <summary>Gets or sets the level range.</summary>
    public LevelRange LevelRange { get; set; } = null!;

    /// <summary>Gets or sets the effective date.</summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>Gets or sets the close date (nullable).</summary>
    public DateTime? CloseDate { get; set; }

    /// <summary>Gets or sets the user who last updated the level.</summary>
    public decimal UpdatedBy { get; set; }

    /// <summary>Gets or sets when the level was last updated.</summary>
    public DateTime UpdatedOn { get; set; }

    /// <summary>Private constructor for EF Core.</summary>
    private CompensationLevel() { }

    /// <summary>Creates a new compensation level.</summary>
    public static CompensationLevel Create(decimal id, string levelDesc, string levelAmount, string levelReason,
        decimal minAmount, decimal maxAmount, DateTime effectiveDate, decimal updatedBy, DateTime updatedOn)
    {
        var level = new CompensationLevel
        {
            Id = id,
            LevelDesc = levelDesc,
            LevelAmount = levelAmount,
            LevelReason = levelReason,
            LevelRange = LevelRange.Create(minAmount, maxAmount),
            EffectiveDate = effectiveDate,
            UpdatedBy = updatedBy,
            UpdatedOn = updatedOn
        };

        return level;
    }

    /// <summary>Closes the compensation level.</summary>
    public void Close(DateTime closeDate)
    {
        CloseDate = closeDate;
        AddDomainEvent(new CompensationLevelClosedEvent(Id, closeDate));
    }
}

/// <summary>
/// Represents a compensation period (quarter).
/// </summary>
public class CompensationPeriod : BaseEntity
{
    /// <summary>Gets or sets the year ID.</summary>
    public decimal YearId { get; set; }

    /// <summary>Gets or sets the quarter number.</summary>
    public decimal QuarterNo { get; set; }

    /// <summary>Gets or sets the period status.</summary>
    public PeriodStatus Status { get; set; } = null!;

    /// <summary>Gets or sets the period open date.</summary>
    public DateTime PeriodOpenDate { get; set; }

    /// <summary>Gets or sets the period close date.</summary>
    public DateTime PeriodCloseDate { get; set; }

    /// <summary>Gets or sets when the circular was generated.</summary>
    public DateTime? CircularGeneratedOn { get; set; }

    /// <summary>Gets or sets who generated the circular.</summary>
    public decimal? CircularGeneratedBy { get; set; }

    /// <summary>Gets or sets when the reminder letter was sent.</summary>
    public DateTime? ReminderLetterOn { get; set; }

    /// <summary>Gets or sets the form open date.</summary>
    public DateTime FormOpenDate { get; set; }

    /// <summary>Gets or sets the last date for appraiser entry.</summary>
    public DateTime? AppraiserLastDate { get; set; }

    /// <summary>Gets or sets the last date for reviewer entry.</summary>
    public DateTime? ReviewerLastDate { get; set; }

    /// <summary>Gets or sets the last date for BHR entry.</summary>
    public DateTime? BhrLastDate { get; set; }

    /// <summary>Gets or sets the last date for UHR entry.</summary>
    public DateTime? UhrLastDate { get; set; }

    /// <summary>Private constructor for EF Core.</summary>
    private CompensationPeriod() { }

    /// <summary>Creates a new compensation period.</summary>
    public static CompensationPeriod Create(decimal id, decimal yearId, decimal quarterNo, string statusCode,
        DateTime periodOpenDate, DateTime periodCloseDate, DateTime formOpenDate)
    {
        var period = new CompensationPeriod
        {
            Id = id,
            YearId = yearId,
            QuarterNo = quarterNo,
            Status = PeriodStatus.FromCode(statusCode),
            PeriodOpenDate = periodOpenDate,
            PeriodCloseDate = periodCloseDate,
            FormOpenDate = formOpenDate
        };

        return period;
    }

    /// <summary>Generates the circular.</summary>
    public void GenerateCircular(decimal generatedBy, DateTime generatedOn)
    {
        CircularGeneratedOn = generatedOn;
        CircularGeneratedBy = generatedBy;
        Status = PeriodStatus.CircularGenerated();
        AddDomainEvent(new CircularGeneratedEvent(Id, generatedBy, generatedOn));
    }

    /// <summary>Confirms the period to payroll.</summary>
    public void ConfirmToPayroll()
    {
        Status = PeriodStatus.ConfirmedToPayroll();
        AddDomainEvent(new PeriodConfirmedToPayrollEvent(Id, DateTime.UtcNow));
    }
}

/// <summary>
/// Represents a compensation recommendation (aggregate root).
/// </summary>
public class CompensationRecommendation : BaseEntity
{
    /// <summary>Gets or sets the year ID.</summary>
    public decimal YearId { get; set; }

    /// <summary>Gets or sets the period ID.</summary>
    public decimal PeriodId { get; set; }

    /// <summary>Gets or sets the employee system ID.</summary>
    public decimal EmployeeSystemId { get; set; }

    /// <summary>Gets or sets the level ID.</summary>
    public decimal LevelId { get; set; }

    /// <summary>Gets or sets the CTC amount.</summary>
    public MoneyAmount CtcAmount { get; set; } = null!;

    /// <summary>Gets or sets the maximum cap.</summary>
    public MoneyAmount MaximumCap { get; set; } = null!;

    /// <summary>Gets or sets the eligibility amount.</summary>
    public MoneyAmount EligibilityAmount { get; set; } = null!;

    /// <summary>Gets or sets the recommended amount.</summary>
    public MoneyAmount? RecommendedAmount { get; set; }

    /// <summary>Gets or sets the initiative taken.</summary>
    public string InitiativeTaken { get; set; } = null!;

    /// <summary>Gets or sets the results.</summary>
    public string Results { get; set; } = null!;

    /// <summary>Gets or sets additional remarks.</summary>
    public string? AdditionalRemarks { get; set; }

    /// <summary>Gets or sets the recommendation status.</summary>
    public RecommendationStatus Status { get; set; } = null!;

    /// <summary>Gets or sets who rejected the recommendation.</summary>
    public decimal? RejectionBy { get; set; }

    /// <summary>Gets or sets when the recommendation was rejected.</summary>
    public DateTime? RejectionOn { get; set; }

    /// <summary>Gets or sets the rejection remarks.</summary>
    public string? RejectionRemarks { get; set; }

    /// <summary>Gets or sets who recommended it (APR/REV/BHR/CHR).</summary>
    public string RecommendedBy { get; set; } = null!;

    /// <summary>Gets or sets who submitted the recommendation.</summary>
    public decimal? RecommendSubmittedBy { get; set; }

    /// <summary>Gets or sets when the recommendation was submitted.</summary>
    public DateTime? RecommendSubmittedOn { get; set; }

    /// <summary>Gets or sets who reviewed it.</summary>
    public decimal? ReviewerSubmittedBy { get; set; }

    /// <summary>Gets or sets when the reviewer submitted.</summary>
    public DateTime? ReviewerSubmittedOn { get; set; }

    /// <summary>Gets or sets who from BHR submitted.</summary>
    public decimal? BhrSubmittedBy { get; set; }

    /// <summary>Gets or sets when BHR submitted.</summary>
    public DateTime? BhrSubmittedOn { get; set; }

    /// <summary>Gets or sets who from CHR submitted.</summary>
    public decimal? ChrSubmittedBy { get; set; }

    /// <summary>Gets or sets when CHR submitted.</summary>
    public DateTime? ChrSubmittedOn { get; set; }

    /// <summary>Gets or sets who from UHR submitted.</summary>
    public decimal? UhrSubmittedBy { get; set; }

    /// <summary>Gets or sets when UHR submitted.</summary>
    public DateTime? UhrSubmittedOn { get; set; }

    /// <summary>Gets or sets the final level after approval.</summary>
    public decimal? FinalLevel { get; set; }

    /// <summary>Gets or sets the final amount after approval.</summary>
    public decimal? FinalAmount { get; set; }

    /// <summary>Gets or sets the initiative letter.</summary>
    public string? InitiativeLetter { get; set; }

    /// <summary>Gets or sets the results letter.</summary>
    public string? ResultsLetter { get; set; }

    /// <summary>Private constructor for EF Core.</summary>
    private CompensationRecommendation() { }

    /// <summary>Creates a new compensation recommendation.</summary>
    public static CompensationRecommendation Create(decimal id, decimal yearId, decimal periodId, decimal employeeSystemId,
        decimal levelId, decimal ctcAmount, decimal maximumCap, decimal eligibilityAmount, string initiativeTaken,
        string results, string recommendedBy)
    {
        var recommendation = new CompensationRecommendation
        {
            Id = id,
            YearId = yearId,
            PeriodId = periodId,
            EmployeeSystemId = employeeSystemId,
            LevelId = levelId,
            CtcAmount = MoneyAmount.Create(ctcAmount),
            MaximumCap = MoneyAmount.Create(maximumCap),
            EligibilityAmount = MoneyAmount.Create(eligibilityAmount),
            InitiativeTaken = initiativeTaken,
            Results = results,
            RecommendedBy = recommendedBy,
            Status = RecommendationStatus.Pending()
        };

        recommendation.AddDomainEvent(new RecommendationCreatedEvent(id, employeeSystemId, DateTime.UtcNow));

        return recommendation;
    }

    /// <summary>Submits the recommendation.</summary>
    public void Submit(decimal submittedBy, DateTime submittedOn, string submittedRole)
    {
        switch (submittedRole.ToUpper())
        {
            case "APR":
                RecommendSubmittedBy = submittedBy;
                RecommendSubmittedOn = submittedOn;
                Status = RecommendationStatus.AppraisalSubmitted();
                break;
            case "REV":
                ReviewerSubmittedBy = submittedBy;
                ReviewerSubmittedOn = submittedOn;
                Status = RecommendationStatus.ReviewerSubmitted();
                break;
            case "BHR":
                BhrSubmittedBy = submittedBy;
                BhrSubmittedOn = submittedOn;
                Status = RecommendationStatus.BhrSubmitted();
                break;
            case "CHR":
                ChrSubmittedBy = submittedBy;
                ChrSubmittedOn = submittedOn;
                Status = RecommendationStatus.ChrSubmitted();
                break;
            case "UHR":
                UhrSubmittedBy = submittedBy;
                UhrSubmittedOn = submittedOn;
                break;
        }

        AddDomainEvent(new RecommendationSubmittedEvent(Id, submittedRole, submittedBy, submittedOn));
    }

    /// <summary>Approves the recommendation.</summary>
    public void Approve(decimal? finalLevel = null, decimal? finalAmount = null)
    {
        Status = RecommendationStatus.Approved();
        FinalLevel = finalLevel ?? LevelId;
        FinalAmount = finalAmount ?? RecommendedAmount?.Amount;
        AddDomainEvent(new RecommendationApprovedEvent(Id, FinalLevel.Value, FinalAmount.Value, DateTime.UtcNow));
    }

    /// <summary>Rejects the recommendation.</summary>
    public void Reject(decimal rejectionBy, DateTime rejectionOn, string rejectionRemarks)
    {
        Status = RecommendationStatus.Rejected();
        RejectionBy = rejectionBy;
        RejectionOn = rejectionOn;
        RejectionRemarks = rejectionRemarks;
        AddDomainEvent(new RecommendationRejectedEvent(Id, rejectionBy, rejectionOn, rejectionRemarks));
    }

    /// <summary>Sets the recommended amount.</summary>
    public void SetRecommendedAmount(MoneyAmount recommendedAmount)
    {
        if (!MaximumCap.Amount.Equals(0) && recommendedAmount.Amount > MaximumCap.Amount)
            throw new InvalidOperationException("Recommended amount cannot exceed the maximum cap.");

        RecommendedAmount = recommendedAmount;
    }
}

/// <summary>
/// Represents a budget log entry.
/// </summary>
public class BudgetLog : BaseEntity
{
    /// <summary>Gets or sets the budget ID.</summary>
    public decimal BudgetId { get; set; }

    /// <summary>Gets or sets the budget amount.</summary>
    public MoneyAmount BudgetAmount { get; set; } = null!;

    /// <summary>Gets or sets who originally updated the budget.</summary>
    public decimal UpdatedBy { get; set; }

    /// <summary>Gets or sets when the budget was originally updated.</summary>
    public DateTime UpdatedOn { get; set; }

    /// <summary>Gets or sets who modified the budget.</summary>
    public decimal ModifiedBy { get; set; }

    /// <summary>Gets or sets when the budget was modified.</summary>
    public DateTime ModifiedOn { get; set; }

    /// <summary>Private constructor for EF Core.</summary>
    private BudgetLog() { }

    /// <summary>Creates a new budget log entry.</summary>
    public static BudgetLog Create(decimal id, decimal budgetId, decimal budgetAmount, decimal updatedBy,
        DateTime updatedOn, decimal modifiedBy, DateTime modifiedOn)
    {
        return new BudgetLog
        {
            Id = id,
            BudgetId = budgetId,
            BudgetAmount = MoneyAmount.Create(budgetAmount),
            UpdatedBy = updatedBy,
            UpdatedOn = updatedOn,
            ModifiedBy = modifiedBy,
            ModifiedOn = modifiedOn
        };
    }
}
