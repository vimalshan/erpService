using MediatR;
using WorkOrderService.Application.DTOs;
using WorkOrderService.Application.Queries.GetAllWorkOrders;
using WorkOrderService.Application.Queries.GetTasksByWorkOrder;
using WorkOrderService.Application.Queries.GetWorkOrder;

namespace WorkOrderService.API.GraphQL.Queries;

public class WorkOrderQueries
{
    public async Task<IEnumerable<WorkOrderDto>> GetWorkOrders([Service] IMediator mediator, CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetAllWorkOrdersQuery(), cancellationToken);
    }

    public async Task<WorkOrderDto?> GetWorkOrder([Service] IMediator mediator, long workOrderId, CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetWorkOrderQuery(workOrderId), cancellationToken);
    }

    public async Task<IEnumerable<WorkTaskDto>> GetTasksByWorkOrder([Service] IMediator mediator, long workOrderId, CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetTasksByWorkOrderQuery(workOrderId), cancellationToken);
    }
}
