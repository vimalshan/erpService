using MediatR;
using WorkOrderService.Application.DTOs;

namespace WorkOrderService.Application.Queries.GetWorkOrder;

public record GetWorkOrderQuery(long WorkOrderId) : IRequest<WorkOrderDto?>;
