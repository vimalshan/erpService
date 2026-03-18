namespace FeedbackService.Domain.Aggregates;

using Entities;
using Events;
using Exceptions;
using ValueObjects;
using Common;

/// <summary>
/// Represents the Feedback aggregate root
/// </summary>
public class Feedback : AggregateRoot
{
    /// <summary>
    /// Initializes a new instance of the Feedback class
    /// </summary>
    private Feedback() { }

    /// <summary>
    /// Gets the request number
    /// </summary>
    public decimal RequestNo { get; private set; }

    /// <summary>
    /// Gets the approver system ID
    /// </summary>
    public decimal ApproverSystemId { get; private set; }

    /// <summary>
    /// Gets the feedback status
    /// </summary>
    public FeedbackStatus? Status { get; private set; }

    /// <summary>
    /// Gets the remarks/comments on the feedback
    /// </summary>
    public string? Remarks { get; private set; }

    /// <summary>
    /// Gets the feedback items (details)
    /// </summary>
    public IReadOnlyList<FeedbackItem> Items => _items.AsReadOnly();

    private readonly List<FeedbackItem> _items = new();

    /// <summary>
    /// Creates a new feedback instance
    /// </summary>
    public static Feedback Create(decimal feedbackId, decimal requestNo, decimal approverSystemId)
    {
        if (requestNo <= 0)
            throw new FeedbackDomainException("Request number must be greater than zero");

        if (approverSystemId <= 0)
            throw new FeedbackDomainException("Approver system ID must be greater than zero");

        var feedback = new Feedback
        {
            Id = feedbackId,
            RequestNo = requestNo,
            ApproverSystemId = approverSystemId,
            Status = FeedbackStatus.Active(),
            CreatedOn = DateTime.UtcNow
        };

        feedback.RaiseDomainEvent(new FeedbackCreatedEvent(feedbackId, requestNo, approverSystemId));

        return feedback;
    }

    /// <summary>
    /// Adds a feedback item to the feedback
    /// </summary>
    public void AddItem(decimal questionNo, decimal? answerNo)
    {
        if (questionNo <= 0)
            throw new FeedbackDomainException("Question number must be greater than zero");

        var existingItem = _items.FirstOrDefault(x => x.QuestionNo == questionNo);
        
        if (existingItem != null)
        {
            existingItem.UpdateAnswer(answerNo);
        }
        else
        {
            _items.Add(new FeedbackItem(Id, questionNo, answerNo));
        }

        UpdatedOn = DateTime.UtcNow;
    }

    /// <summary>
    /// Removes a feedback item
    /// </summary>
    public void RemoveItem(decimal questionNo)
    {
        var item = _items.FirstOrDefault(x => x.QuestionNo == questionNo);
        if (item != null)
        {
            _items.Remove(item);
            UpdatedOn = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Submits the feedback
    /// </summary>
    public void Submit()
    {
        if (!_items.Any())
            throw new FeedbackDomainException("Cannot submit feedback without items");

        Status = FeedbackStatus.Inactive();
        UpdatedOn = DateTime.UtcNow;

        RaiseDomainEvent(new FeedbackSubmittedEvent(Id, RequestNo));
    }

    /// <summary>
    /// Updates the remarks
    /// </summary>
    public void UpdateRemarks(string? remarks)
    {
        Remarks = remarks;
        UpdatedOn = DateTime.UtcNow;
    }

    /// <summary>
    /// Activates the feedback
    /// </summary>
    public void Activate()
    {
        Status = FeedbackStatus.Active();
        UpdatedOn = DateTime.UtcNow;
    }

    /// <summary>
    /// Deactivates the feedback
    /// </summary>
    public void Deactivate()
    {
        Status = FeedbackStatus.Inactive();
        UpdatedOn = DateTime.UtcNow;
    }
}
