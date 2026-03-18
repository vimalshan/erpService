using MediatR;
using Todos.Application.DTOs;

namespace Todos.Application.Commands;

/// <summary>
/// Command to update a learning record
/// </summary>
public class UpdateLearningRecordCommand : IRequest<ApiResponse<LearningRecordDto>>
{
    public Guid Id { get; set; }
    public string? SpecificNeed { get; set; }
    public string? Indicator { get; set; }
    public string? DevelopmentArea { get; set; }
    public string? ExpectedPostTraining { get; set; }
    public string? BhrStatus { get; set; }
    public decimal ModifiedBy { get; set; }
}
