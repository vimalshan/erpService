namespace FeedbackService.Application.Commands;

using MediatR;
using DTOs;

/// <summary>
/// Command to submit feedback
/// </summary>
public class SubmitFeedbackCommand : IRequest<FeedbackDto>
{
    /// <summary>
    /// Gets or sets the feedback ID
    /// </summary>
    public decimal FeedbackId { get; set; }
}
