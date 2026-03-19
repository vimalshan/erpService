namespace OrderScheduleService.API.GraphQL;

using MediatR;
using OrderScheduleService.Application.DTOs;
using OrderScheduleService.Application.Queries;

public class Query
{
    public async Task<TiedOrderDto?> GetOrder(
        long id,
        [Service] IMediator mediator)
    {
        return await mediator.Send(new GetTiedOrderByIdQuery(id));
    }

    public async Task<IEnumerable<TiedOrderDto>> GetOrders(
        [Service] IMediator mediator)
    {
        return await mediator.Send(new GetAllOrdersQuery());
    }

    public async Task<IEnumerable<TiedOrderDto>> GetOrdersByCustomer(
        string customerCode,
        [Service] IMediator mediator)
    {
        return await mediator.Send(new GetOrdersByCustomerQuery(customerCode));
    }

    public async Task<ScheduleDto?> GetSchedule(
        long id,
        [Service] IMediator mediator)
    {
        return await mediator.Send(new GetScheduleByIdQuery(id));
    }

    public async Task<IEnumerable<ScheduleDto>> GetSchedulesByItem(
        decimal itemId,
        [Service] IMediator mediator)
    {
        return await mediator.Send(new GetSchedulesByItemQuery(itemId));
    }

    public async Task<IEnumerable<ShiftDto>> GetShifts(
        [Service] IMediator mediator)
    {
        return await mediator.Send(new GetAllShiftsQuery());
    }
}

public class Mutation
{
    public async Task<long> CreateOrder(
        CreateTiedOrderDto input,
        [Service] IMediator mediator)
    {
        return await mediator.Send(new OrderScheduleService.Application.Commands.CreateTiedOrderCommand(input));
    }

    public async Task<bool> ScheduleOrder(
        long orderId,
        long detailId,
        DateTime scheduledDate,
        long allocatedQuantity,
        int userId,
        [Service] IMediator mediator)
    {
        return await mediator.Send(new OrderScheduleService.Application.Commands.ScheduleOrderDetailCommand(
            orderId, detailId, scheduledDate, allocatedQuantity, userId));
    }

    public async Task<bool> CancelOrder(
        long orderId,
        long detailId,
        int userId,
        [Service] IMediator mediator)
    {
        return await mediator.Send(new OrderScheduleService.Application.Commands.CancelOrderDetailCommand(orderId, detailId, userId));
    }

    public async Task<long> CreateSchedule(
        CreateScheduleDto input,
        [Service] IMediator mediator)
    {
        return await mediator.Send(new OrderScheduleService.Application.Commands.CreateScheduleCommand(input));
    }
}

public class Subscription
{
    [Subscribe]
    public async IAsyncEnumerable<long> OnOrderCreated(
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        // Placeholder for subscription logic
        await Task.Delay(100, cancellationToken);
        yield return 1;
    }
}
