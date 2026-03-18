using MediatR;
using ProxyModule.Application.Commands.CreateProxyRight;
using ProxyModule.Application.Commands.DeactivateProxyRight;
using ProxyModule.Application.Commands.UpdateProxyRight;
using ProxyModule.Application.DTOs;

namespace ProxyModule.API.GraphQL.Mutations;

public class ProxyRightMutation
{
    public async Task<ProxyRightDto> CreateProxyRight(
        [Service] IMediator mediator,
        CreateProxyRightDto input,
        CancellationToken ct)
    {
        var command = new CreateProxyRightCommand(
            input.ProxyUserId, input.DelegatedUserId, input.ProxyStartDate,
            input.ProxyEndDate, input.ProxyType, input.Scope, input.Notes, input.CreatedBy);

        return await mediator.Send(command, ct);
    }

    public async Task<ProxyRightDto> UpdateProxyRight(
        [Service] IMediator mediator,
        long proxyId,
        UpdateProxyRightDto input,
        CancellationToken ct)
    {
        var command = new UpdateProxyRightCommand(
            proxyId, input.ProxyStartDate, input.ProxyEndDate, input.ProxyType,
            input.Scope, input.Notes, input.UpdatedBy);

        return await mediator.Send(command, ct);
    }

    public async Task<bool> DeactivateProxyRight(
        [Service] IMediator mediator,
        long proxyId,
        long updatedBy,
        CancellationToken ct)
    {
        return await mediator.Send(new DeactivateProxyRightCommand(proxyId, updatedBy), ct);
    }
}
