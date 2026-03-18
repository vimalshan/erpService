namespace FeedbackService.Application.DTOs;

/// <summary>
/// Data transfer object for feedback
/// </summary>
public class FeedbackDto
{
    /// <summary>
    /// Gets or sets the feedback ID
    /// </summary>
    public decimal Id { get; set; }

    /// <summary>
    /// Gets or sets the request number
    /// </summary>
    public decimal RequestNo { get; set; }

    /// <summary>
    /// Gets or sets the approver system ID
    /// </summary>
    public decimal ApproverSystemId { get; set; }

    /// <summary>
    /// Gets or sets the status
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Gets or sets the remarks
    /// </summary>
    public string? Remarks { get; set; }

    /// <summary>
    /// Gets or sets the feedback items
    /// </summary>
    public List<FeedbackItemDto> Items { get; set; } = new();

    /// <summary>
    /// Gets or sets the creation timestamp
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Gets or sets the update timestamp
    /// </summary>
    public DateTime? UpdatedOn { get; set; }
}
