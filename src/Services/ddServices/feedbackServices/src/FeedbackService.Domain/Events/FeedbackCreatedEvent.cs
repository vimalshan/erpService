namespace FeedbackService.Domain.Events;

/// <summary>
/// Event raised when feedback is created
/// </summary>
public class FeedbackCreatedEvent : Common.DomainEvent
{
    /// <summary>
    /// Initializes a new instance of the FeedbackCreatedEvent class
    /// </summary>
    public FeedbackCreatedEvent(decimal feedbackId, decimal requestNo, decimal approverSystemId)
    {
        FeedbackId = feedbackId;
        RequestNo = requestNo;
        ApproverSystemId = approverSystemId;
    }

    /// <summary>
    /// Gets the feedback ID
    /// </summary>
    public decimal FeedbackId { get; }

    /// <summary>
    /// Gets the request number
    /// </summary>
    public decimal RequestNo { get; }

    /// <summary>
    /// Gets the approver system ID
    /// </summary>
    public decimal ApproverSystemId { get; }
}
