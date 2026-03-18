namespace Todos.Application.DTOs;

/// <summary>
/// DTO for updating a learning record
/// </summary>
public class UpdateLearningRecordDto
{
    public Guid Id { get; set; }
    public string? SpecificNeed { get; set; }
    public string? Indicator { get; set; }
    public string? DevelopmentArea { get; set; }
    public string? ExpectedPostTraining { get; set; }
    public string? BhrStatus { get; set; }
    public decimal ModifiedBy { get; set; }
}
