namespace FeedbackService.Application.DTOs;

/// <summary>
/// Data transfer object for feedback item
/// </summary>
public class FeedbackItemDto
{
    /// <summary>
    /// Gets or sets the feedback ID
    /// </summary>
    public decimal FeedbackId { get; set; }

    /// <summary>
    /// Gets or sets the question number
    /// </summary>
    public decimal QuestionNo { get; set; }

    /// <summary>
    /// Gets or sets the answer number
    /// </summary>
    public decimal? AnswerNo { get; set; }

    /// <summary>
    /// Gets or sets the update timestamp
    /// </summary>
    public DateTime? UpdatedOn { get; set; }
}
