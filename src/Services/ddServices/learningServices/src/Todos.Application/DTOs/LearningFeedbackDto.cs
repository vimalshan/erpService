namespace Todos.Application.DTOs;

/// <summary>
/// DTO for learning feedback response
/// </summary>
public class LearningFeedbackDto
{
    public Guid Id { get; set; }
    public decimal FeedbackId { get; set; }
    public decimal RequestNumber { get; set; }
    public string? SpecificNeed { get; set; }
    public string? TrainingProgram { get; set; }
    public string? FeedbackStatus { get; set; }
    public string? AppraiseeComments { get; set; }
    public string? AppraiserComments { get; set; }
    public string? ReviewerComments { get; set; }
    public string? AppraiserNeedStatus { get; set; }
    public string? AppraiserPostTraining { get; set; }
    public decimal ModifiedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int Version { get; set; }
}
