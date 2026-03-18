using MediatR;
using Stationery.Application.Features.Requests.Commands;
using Stationery.Application.Features.Orders.Commands;

namespace Stationery.Api.GraphQL;

public class Mutation
{
    public async Task<long> CreateRequest(CreateRequestCommand command, [Service] IMediator mediator)
        => await mediator.Send(command);

    public async Task<bool> ApproveRequest(ApproveRequestCommand command, [Service] IMediator mediator)
    {
        await mediator.Send(command);
        return true;
    }

    public async Task<long> CreateOrder(CreateOrderCommand command, [Service] IMediator mediator)
        => await mediator.Send(command);

    public async Task<bool> ReceiveOrder(ReceiveOrderCommand command, [Service] IMediator mediator)
    {
        await mediator.Send(command);
        return true;
    }
}
