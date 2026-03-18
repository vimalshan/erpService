namespace ReportingService.Domain.Entities;

/// <summary>
/// Appraisal Entity - Main entity representing an appraisal process
/// </summary>
public class Appraisal : BaseEntity
{
    public long RequestNumber { get; set; }
    public string? UserName { get; set; }
    public string? StatusDescription { get; set; }
    public string? NumericDate { get; set; }
    public string? FinancialPeriod { get; set; }
    public string? UserId { get; set; }
    public DateTime? FinancialStartYear { get; set; }
    public DateTime? FinancialEndYear { get; set; }
    public long? EmployeeNumber { get; set; }
    public string? UnitCode { get; set; }
    public string? GradeCode { get; set; }
    public string? AcademicYear { get; set; }
    public string? DDType { get; set; }
    public char? CompletionFlag { get; set; }
    public char? StatusCode { get; set; }
    public long? PinNumber { get; set; }

    // Navigation properties
    public virtual ICollection<AppraisalGoal> Goals { get; set; } = new List<AppraisalGoal>();
    public virtual ICollection<AppraiseePerformance> Performances { get; set; } = new List<AppraiseePerformance>();

    public Appraisal() { }

    public Appraisal(long requestNumber, string? userName, string? userId)
    {
        RequestNumber = requestNumber;
        UserName = userName;
        UserId = userId;
    }

    public void MarkAsCompleted()
    {
        CompletionFlag = '1';
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsCompleted => CompletionFlag == '1';
}
