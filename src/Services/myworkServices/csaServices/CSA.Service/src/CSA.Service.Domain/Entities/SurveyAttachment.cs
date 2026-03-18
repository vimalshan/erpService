using CSA.Service.Domain.Common;

namespace CSA.Service.Domain.Entities;

public class SurveyAttachment : BaseEntity
{
    public long AttachmentId { get; set; }
    public long FeedbackId { get; set; }
    public long ControlEvidenceId { get; set; }
    public string? Attachment { get; set; }

    // Navigation
    public SurveyFeedback? Feedback { get; set; }
    public Evidence? Evidence { get; set; }
}
