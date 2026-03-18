namespace Todos.Application.DTOs;

/// <summary>
/// DTO for submitting learning feedback
/// </summary>
public class SubmitLearningFeedbackDto
{
    public Guid FeedbackId { get; set; }
    public string? TrainingProgram { get; set; }
    public string? FeedbackStatus { get; set; }
    public string? AppraiseeComments { get; set; }
    public string? AppraiserComments { get; set; }
    public string? ReviewerComments { get; set; }
    public decimal ModifiedBy { get; set; }
}
