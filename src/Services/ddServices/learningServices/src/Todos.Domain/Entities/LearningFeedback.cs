using Todos.Domain.Abstractions;
using Todos.Domain.ValueObjects;

namespace Todos.Domain.Entities;

/// <summary>
/// Represents a learning feedback record (LET_FEEDBACK)
/// </summary>
public class LearningFeedback : AggregateRoot
{
    /// <summary>
    /// Gets the feedback ID (from LET_SRL)
    /// </summary>
    public decimal FeedbackId { get; private set; }

    /// <summary>
    /// Gets the DD request number
    /// </summary>
    public RequestNumber RequestNumber { get; private set; } = null!;

    /// <summary>
    /// Gets the specific learning need from previous year
    /// </summary>
    public string? SpecificNeed { get; private set; }

    /// <summary>
    /// Gets the training program/workshop attended
    /// </summary>
    public string? TrainingProgram { get; private set; }

    /// <summary>
    /// Gets the feedback status
    /// </summary>
    public FeedbackStatus? FeedbackStatus { get; private set; }

    /// <summary>
    /// Gets the appraisee comments
    /// </summary>
    public string? AppraiseeComments { get; private set; }

    /// <summary>
    /// Gets the appraiser comments
    /// </summary>
    public string? AppraiserComments { get; private set; }

    /// <summary>
    /// Gets the reviewer comments
    /// </summary>
    public string? ReviewerComments { get; private set; }

    /// <summary>
    /// Gets the appraiser need status
    /// </summary>
    public FeedbackStatus? AppraiserNeedStatus { get; private set; }

    /// <summary>
    /// Gets post-training feedback
    /// </summary>
    public string? AppraiserPostTraining { get; private set; }

    /// <summary>
    /// Gets the user who modified this record
    /// </summary>
    public decimal ModifiedBy { get; private set; }

    /// <summary>
    /// Initializes a new instance of the LearningFeedback class
    /// </summary>
    protected LearningFeedback() { }

    /// <summary>
    /// Creates a new learning feedback record
    /// </summary>
    public static LearningFeedback Create(
        decimal feedbackId,
        RequestNumber requestNumber,
        string? specificNeed,
        decimal modifiedBy)
    {
        var feedback = new LearningFeedback
        {
            FeedbackId = feedbackId,
            RequestNumber = requestNumber,
            SpecificNeed = specificNeed,
            ModifiedBy = modifiedBy,
            CreatedAt = DateTime.UtcNow,
            Version = 1
        };

        return feedback;
    }

    /// <summary>
    /// Submits feedback
    /// </summary>
    public void SubmitFeedback(
        string? trainingProgram,
        FeedbackStatus? feedbackStatus,
        string? appraiseeComments,
        string? appraiserComments,
        string? reviewerComments,
        decimal modifiedBy)
    {
        TrainingProgram = trainingProgram;
        FeedbackStatus = feedbackStatus;
        AppraiseeComments = appraiseeComments;
        AppraiserComments = appraiserComments;
        ReviewerComments = reviewerComments;
        ModifiedBy = modifiedBy;
        UpdatedAt = DateTime.UtcNow;
        Version++;

        RaiseDomainEvent(new Events.FeedbackSubmittedEvent
        {
            AggregateId = Id,
            RequestNumber = RequestNumber.Value,
            FeedbackStatus = feedbackStatus?.Value ?? 'N',
            SubmittedAt = UpdatedAt.Value
        });
    }
}
