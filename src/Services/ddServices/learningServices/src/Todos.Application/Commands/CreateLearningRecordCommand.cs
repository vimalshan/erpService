using MediatR;
using Todos.Application.DTOs;

namespace Todos.Application.Commands;

/// <summary>
/// Command to create a learning record
/// </summary>
public class CreateLearningRecordCommand : IRequest<ApiResponse<LearningRecordDto>>
{
    public decimal LetId { get; set; }
    public decimal RequestNumber { get; set; }
    public string? EmployeeId { get; set; }
    public string? SpecificNeed { get; set; }
    public decimal ModifiedBy { get; set; }
}
