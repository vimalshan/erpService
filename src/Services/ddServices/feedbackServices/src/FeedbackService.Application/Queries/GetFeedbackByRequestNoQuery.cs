namespace FeedbackService.Application.Queries;

using MediatR;
using DTOs;

/// <summary>
/// Query to get feedback by request number
/// </summary>
public class GetFeedbackByRequestNoQuery : IRequest<List<FeedbackDto>>
{
    /// <summary>
    /// Gets or sets the request number
    /// </summary>
    public decimal RequestNo { get; set; }
}
