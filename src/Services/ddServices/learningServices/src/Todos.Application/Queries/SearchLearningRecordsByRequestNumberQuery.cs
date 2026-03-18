using MediatR;
using Todos.Application.DTOs;

namespace Todos.Application.Queries;

/// <summary>
/// Query to search learning records by request number
/// </summary>
public class SearchLearningRecordsByRequestNumberQuery : IRequest<ApiResponse<IEnumerable<LearningRecordDto>>>
{
    public decimal RequestNumber { get; set; }
}
