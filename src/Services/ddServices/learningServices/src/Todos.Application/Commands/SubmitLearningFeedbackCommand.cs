using MediatR;
using Todos.Application.DTOs;

namespace Todos.Application.Commands;

/// <summary>
/// Command to submit learning feedback
/// </summary>
public class SubmitLearningFeedbackCommand : IRequest<ApiResponse<LearningFeedbackDto>>
{
    public Guid FeedbackId { get; set; }
    public string? TrainingProgram { get; set; }
    public string? FeedbackStatus { get; set; }
    public string? AppraiseeComments { get; set; }
    public string? AppraiserComments { get; set; }
    public string? ReviewerComments { get; set; }
    public decimal ModifiedBy { get; set; }
}
