using MediatR;
using WorkOrderService.Application.DTOs;

namespace WorkOrderService.Application.Commands.CreateWorkOrder;

public record CreateWorkOrderCommand : IRequest<WorkOrderDto>
{
    public string WorkOrderName { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime DueDate { get; init; }
    public long AssignedTo { get; init; }
    public long CreatedBy { get; init; }
}
