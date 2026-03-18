using MediatR;
using Todos.Application.DTOs;

namespace Todos.Application.Queries;

/// <summary>
/// Query to get learning feedback by ID
/// </summary>
public class GetLearningFeedbackByIdQuery : IRequest<ApiResponse<LearningFeedbackDto>>
{
    public Guid Id { get; set; }
}
