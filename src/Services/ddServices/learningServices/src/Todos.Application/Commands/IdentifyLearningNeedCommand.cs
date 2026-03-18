using MediatR;
using Todos.Application.DTOs;

namespace Todos.Application.Commands;

/// <summary>
/// Command to identify a learning need
/// </summary>
public class IdentifyLearningNeedCommand : IRequest<ApiResponse<LearningRecordDto>>
{
    public Guid LearningRecordId { get; set; }
    public string DevelopmentArea { get; set; } = string.Empty;
    public string Indicator { get; set; } = string.Empty;
}
