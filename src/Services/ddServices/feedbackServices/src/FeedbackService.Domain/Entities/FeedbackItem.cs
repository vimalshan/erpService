namespace FeedbackService.Domain.Entities;

/// <summary>
/// Represents a feedback sub-item/detail (question and answer)
/// </summary>
public class FeedbackItem
{
    /// <summary>
    /// Initializes a new instance of the FeedbackItem class
    /// </summary>
    public FeedbackItem(decimal feedbackId, decimal questionNo, decimal? answerNo)
    {
        FeedbackId = feedbackId;
        QuestionNo = questionNo;
        AnswerNo = answerNo;
    }

    /// <summary>
    /// Gets the feedback ID
    /// </summary>
    public decimal FeedbackId { get; private set; }

    /// <summary>
    /// Gets the question number
    /// </summary>
    public decimal QuestionNo { get; private set; }

    /// <summary>
    /// Gets the answer number
    /// </summary>
    public decimal? AnswerNo { get; private set; }

    /// <summary>
    /// Gets the update timestamp
    /// </summary>
    public DateTime? UpdatedOn { get; private set; }

    /// <summary>
    /// Updates the answer for this feedback item
    /// </summary>
    public void UpdateAnswer(decimal? answerNo)
    {
        AnswerNo = answerNo;
        UpdatedOn = DateTime.UtcNow;
    }
}
