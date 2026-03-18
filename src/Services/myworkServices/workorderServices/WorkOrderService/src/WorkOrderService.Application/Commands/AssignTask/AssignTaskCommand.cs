using MediatR;
using WorkOrderService.Application.DTOs;

namespace WorkOrderService.Application.Commands.AssignTask;

public record AssignTaskCommand : IRequest<WorkTaskDto>
{
    public long WorkOrderId { get; init; }
    public string TaskName { get; init; } = string.Empty;
    public long AssignedTo { get; init; }
    public int EstimatedHours { get; init; }
    public long CreatedBy { get; init; }
}
