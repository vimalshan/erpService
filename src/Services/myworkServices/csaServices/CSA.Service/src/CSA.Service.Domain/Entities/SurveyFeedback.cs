using CSA.Service.Domain.Common;

namespace CSA.Service.Domain.Entities;

public class SurveyFeedback : BaseEntity
{
    public long FeedbackId { get; set; }
    public long SurveyQuestionId { get; set; }
    public long EmployeeSysId { get; set; }
    public char Status { get; set; }
    public char Type { get; set; }
    public char RemedialFlag { get; set; }
    public DateTime? RemedialDate { get; set; }
    public string? Remarks { get; set; }
    public DateTime EnteredOn { get; set; }
    public char EvidenceFlag { get; set; }
    public char ApprovalFlag { get; set; }
    public string ApproverRemarks { get; set; } = string.Empty;
    public DateTime? ApprovalDate { get; set; }
    public long? ApprovedBy { get; set; }
    public DateTime? EntryDate { get; set; }

    // Navigation
    public SurveyQuestion? SurveyQuestion { get; set; }
    public ICollection<SurveyAttachment> Attachments { get; set; } = [];
}
