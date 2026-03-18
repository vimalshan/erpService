using MediatR;
using Todos.Application.DTOs;

namespace Todos.Application.Commands;

/// <summary>
/// Command to delete a learning record
/// </summary>
public class DeleteLearningRecordCommand : IRequest<ApiResponse<bool>>
{
    public Guid Id { get; set; }
}
