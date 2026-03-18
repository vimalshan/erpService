namespace FeedbackService.Application.Commands;

using MediatR;
using DTOs;

/// <summary>
/// Command to add a feedback item
/// </summary>
public class AddFeedbackItemCommand : IRequest<FeedbackDto>
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
}
