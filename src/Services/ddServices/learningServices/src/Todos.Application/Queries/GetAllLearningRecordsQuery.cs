using MediatR;
using Todos.Application.DTOs;

namespace Todos.Application.Queries;

/// <summary>
/// Query to get all learning records
/// </summary>
public class GetAllLearningRecordsQuery : IRequest<ApiResponse<IEnumerable<LearningRecordDto>>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
