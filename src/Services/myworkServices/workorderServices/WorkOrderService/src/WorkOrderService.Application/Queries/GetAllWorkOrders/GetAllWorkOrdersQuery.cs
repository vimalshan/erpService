using MediatR;
using WorkOrderService.Application.DTOs;

namespace WorkOrderService.Application.Queries.GetAllWorkOrders;

public record GetAllWorkOrdersQuery : IRequest<IEnumerable<WorkOrderDto>>;
