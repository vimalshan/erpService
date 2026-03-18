namespace FeedbackService.Application.Commands;

using MediatR;
using DTOs;

/// <summary>
/// Command to create a new feedback
/// </summary>
public class CreateFeedbackCommand : IRequest<FeedbackDto>
{
    /// <summary>
    /// Gets or sets the feedback ID
    /// </summary>
    public decimal FeedbackId { get; set; }

    /// <summary>
    /// Gets or sets the request number
    /// </summary>
    public decimal RequestNo { get; set; }

    /// <summary>
    /// Gets or sets the approver system ID
    /// </summary>
    public decimal ApproverSystemId { get; set; }

    /// <summary>
    /// Gets or sets the remarks
    /// </summary>
    public string? Remarks { get; set; }
}
