namespace Todos.Application.DTOs;

/// <summary>
/// DTO for creating a learning record
/// </summary>
public class CreateLearningRecordDto
{
    public decimal LetId { get; set; }
    public decimal RequestNumber { get; set; }
    public string? EmployeeId { get; set; }
    public string? SpecificNeed { get; set; }
    public decimal ModifiedBy { get; set; }
}
