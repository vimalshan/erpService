using MediatR;
using OrderService.Application.DTOs;
using OrderService.Application.Queries;

namespace OrderService.API.GraphQL;

public class OrderQuery
{
    public async Task<IReadOnlyList<OrderDto>> GetOrders([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllOrdersQuery(), ct);

    public async Task<OrderDto?> GetOrderById([Service] IMediator mediator, int orderId, CancellationToken ct)
        => await mediator.Send(new GetOrderByIdQuery(orderId), ct);

    public async Task<OrderDto?> GetOrderByNumber([Service] IMediator mediator, string orderNumber, CancellationToken ct)
        => await mediator.Send(new GetOrderByNumberQuery(orderNumber), ct);

    public async Task<IReadOnlyList<OrderDto>> GetOrdersByCustomer([Service] IMediator mediator, int customerId, CancellationToken ct)
        => await mediator.Send(new GetOrdersByCustomerQuery(customerId), ct);
}
