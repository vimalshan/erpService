namespace FeedbackService.Domain.Events;

/// <summary>
/// Event raised when feedback is submitted
/// </summary>
public class FeedbackSubmittedEvent : Common.DomainEvent
{
    /// <summary>
    /// Initializes a new instance of the FeedbackSubmittedEvent class
    /// </summary>
    public FeedbackSubmittedEvent(decimal feedbackId, decimal requestNo)
    {
        FeedbackId = feedbackId;
        RequestNo = requestNo;
    }

    /// <summary>
    /// Gets the feedback ID
    /// </summary>
    public decimal FeedbackId { get; }

    /// <summary>
    /// Gets the request number
    /// </summary>
    public decimal RequestNo { get; }
}
