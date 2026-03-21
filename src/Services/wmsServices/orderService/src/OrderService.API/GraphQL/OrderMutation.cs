using MediatR;
using OrderService.Application.Commands;
using OrderService.Application.DTOs;

namespace OrderService.API.GraphQL;

public class OrderMutation
{
    public async Task<OrderDto> CreateOrder([Service] IMediator mediator, CreateOrderRequest input, CancellationToken ct)
        => await mediator.Send(new CreateOrderCommand(input), ct);

    public async Task<bool> UpdateOrderStatus([Service] IMediator mediator, int orderId, string status, CancellationToken ct)
    {
        await mediator.Send(new UpdateOrderStatusCommand(orderId, status), ct);
        return true;
    }

    public async Task<bool> DeleteOrder([Service] IMediator mediator, int orderId, CancellationToken ct)
    {
        await mediator.Send(new DeleteOrderCommand(orderId), ct);
        return true;
    }
}
