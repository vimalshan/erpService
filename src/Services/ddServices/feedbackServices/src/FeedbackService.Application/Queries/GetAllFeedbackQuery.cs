namespace FeedbackService.Application.Queries;

using MediatR;
using DTOs;

/// <summary>
/// Query to get all feedback
/// </summary>
public class GetAllFeedbackQuery : IRequest<List<FeedbackDto>>
{
    /// <summary>
    /// Gets or sets the page number (1-based)
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Gets or sets the page size
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Gets or sets the status filter
    /// </summary>
    public string? StatusFilter { get; set; }
}
