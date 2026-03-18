namespace Todos.Application.DTOs;

/// <summary>
/// DTO for development category detail response
/// </summary>
public class DevelopmentCategoryDetailDto
{
    public Guid Id { get; set; }
    public decimal RequestNumber { get; set; }
    public decimal QuestionNumber { get; set; }
    public decimal AnswerSerial { get; set; }
    public string? EmployeeId { get; set; }
    public decimal EmployeeNumber { get; set; }
    public string? DevelopmentArea { get; set; }
    public string? Need { get; set; }
    public DateTime? EntryDate { get; set; }
}
