namespace Todos.Application.DTOs;

/// <summary>
/// DTO for learning sub-record response
/// </summary>
public class LearningSubRecordDto
{
    public Guid Id { get; set; }
    public decimal SubId { get; set; }
    public Guid LearningRecordId { get; set; }
    public decimal RequestNumber { get; set; }
    public decimal DevelopmentModeId { get; set; }
    public decimal TrainingId { get; set; }
    public string? TrainingDetail { get; set; }
    public string? Remarks { get; set; }
    public decimal DevelopmentId { get; set; }
    public string? FinalReview { get; set; }
}
