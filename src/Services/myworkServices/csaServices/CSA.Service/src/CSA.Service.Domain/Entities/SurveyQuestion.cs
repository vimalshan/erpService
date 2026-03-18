using CSA.Service.Domain.Common;

namespace CSA.Service.Domain.Entities;

public class SurveyQuestion : AuditableEntity
{
    public long SurveyQuestionId { get; set; }
    public long SurveyId { get; set; }
    public long ControlId { get; set; }
    public long UnitId { get; set; }
    public long OwnerId { get; set; }
    public long ApproverId { get; set; }
    public DateTime OriginalDueDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? CancelDate { get; set; }
    public char? AssessmentFlag { get; set; }
    public char? ApprovalFlag { get; set; }
    public char? RemedialFlag { get; set; }
    public DateTime? RemedialDate { get; set; }
    public DateTime? AssessmentDate { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public long? DelayDays { get; set; }
    public long? RemedialCount { get; set; }
    public string? UnitName { get; set; }
    public char? EntryFlag { get; set; }

    // Navigation
    public Survey? Survey { get; set; }
    public Control? Control { get; set; }
    public Unit? Unit { get; set; }
    public ICollection<SurveyFeedback> Feedbacks { get; set; } = [];
}
