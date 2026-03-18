using MediatR;
using Todos.Application.DTOs;

namespace Todos.Application.Queries;

/// <summary>
/// Query to get all learning feedback records
/// </summary>
public class GetAllLearningFeedbackQuery : IRequest<ApiResponse<IEnumerable<LearningFeedbackDto>>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
