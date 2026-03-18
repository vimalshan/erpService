namespace Todos.Application.DTOs;

/// <summary>
/// DTO for learning record response
/// </summary>
public class LearningRecordDto
{
    public Guid Id { get; set; }
    public decimal LetId { get; set; }
    public decimal RequestNumber { get; set; }
    public string? EmployeeId { get; set; }
    public decimal? DevelopmentSourceId { get; set; }
    public string? SpecificNeed { get; set; }
    public string? Indicator { get; set; }
    public string? DevelopmentArea { get; set; }
    public string? ExpectedPostTraining { get; set; }
    public string? BhrStatus { get; set; }
    public string? ReviewerComments { get; set; }
    public string? AppraiseeOpinion { get; set; }
    public string? AppraiserComments { get; set; }
    public decimal ModifiedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int Version { get; set; }
    public List<LearningSubRecordDto> SubRecords { get; set; } = [];
}
