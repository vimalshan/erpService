namespace ReportingService.Domain.Entities;

/// <summary>
/// Appraisal Goal Entity
/// </summary>
public class AppraisalGoal : BaseEntity
{
    public long RequestNumber { get; set; }
    public long? SerialNumber { get; set; }
    public string? Description { get; set; }
    public string? FromUnit { get; set; }
    public string? ToUnit { get; set; }
    public string? AppraiserRemarks { get; set; }
    public string? CandidateRemarks { get; set; }
    public string? UserId { get; set; }
    public decimal? Weightage { get; set; }
    public DateTime? FinancialStart { get; set; }
    public DateTime? FinancialEnd { get; set; }
    public long? PinNumber { get; set; }
    public char? AppraisalStatus { get; set; }
    public string? Achievement { get; set; }
    public string? Difference { get; set; }
    public string? Category { get; set; }
    public string? UnitOfMeasure { get; set; }
    public long? ModificationSerialNo { get; set; }
    public string? ExpenseCode { get; set; }
    public string? GoalFlag { get; set; }
    public long? AccountabilityId { get; set; }

    public AppraisalGoal() { }

    public AppraisalGoal(long requestNumber, string? description)
    {
        RequestNumber = requestNumber;
        Description = description;
    }
}
