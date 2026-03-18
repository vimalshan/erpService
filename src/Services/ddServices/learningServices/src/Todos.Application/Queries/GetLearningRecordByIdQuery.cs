using MediatR;
using Todos.Application.DTOs;

namespace Todos.Application.Queries;

/// <summary>
/// Query to get a learning record by ID
/// </summary>
public class GetLearningRecordByIdQuery : IRequest<ApiResponse<LearningRecordDto>>
{
    public Guid Id { get; set; }
}
