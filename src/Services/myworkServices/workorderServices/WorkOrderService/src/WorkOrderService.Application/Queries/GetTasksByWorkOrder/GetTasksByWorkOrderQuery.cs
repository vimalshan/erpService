using MediatR;
using WorkOrderService.Application.DTOs;

namespace WorkOrderService.Application.Queries.GetTasksByWorkOrder;

public record GetTasksByWorkOrderQuery(long WorkOrderId) : IRequest<IEnumerable<WorkTaskDto>>;
