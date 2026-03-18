namespace FeedbackService.Application.Queries;

using MediatR;
using DTOs;

/// <summary>
/// Query to get feedback by ID
/// </summary>
public class GetFeedbackByIdQuery : IRequest<FeedbackDto?>
{
    /// <summary>
    /// Gets or sets the feedback ID
    /// </summary>
    public decimal FeedbackId { get; set; }
}
