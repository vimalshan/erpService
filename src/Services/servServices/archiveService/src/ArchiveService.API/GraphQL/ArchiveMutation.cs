using ArchiveService.Application.Features.ServiceOrders.Commands;
using ArchiveService.Application.Features.ToolKits.Commands;
using MediatR;

namespace ArchiveService.API.GraphQL;

public class ArchiveMutation
{
    public async Task<string> CreateServiceOrder(CreateServiceOrderCommand input, [Service] IMediator mediator)
        => await mediator.Send(input);

    public async Task<bool> UpdateServiceOrderStatus(UpdateServiceOrderStatusCommand input, [Service] IMediator mediator)
        => await mediator.Send(input);

    public async Task<bool> DeleteServiceOrder(string sernoDell, [Service] IMediator mediator)
        => await mediator.Send(new DeleteServiceOrderCommand(sernoDell));

    public async Task<long> CreateToolKit(CreateToolKitCommand input, [Service] IMediator mediator)
        => await mediator.Send(input);

    public async Task<bool> UpdateToolKitFlag(UpdateToolKitFlagCommand input, [Service] IMediator mediator)
        => await mediator.Send(input);
}
