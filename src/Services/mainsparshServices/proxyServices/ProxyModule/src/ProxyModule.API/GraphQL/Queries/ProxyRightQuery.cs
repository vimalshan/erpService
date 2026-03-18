using MediatR;
using ProxyModule.Application.DTOs;
using ProxyModule.Application.Queries.GetActiveProxyRights;
using ProxyModule.Application.Queries.GetProxyRightById;
using ProxyModule.Application.Queries.GetProxyRightsByUser;

namespace ProxyModule.API.GraphQL.Queries;

public class ProxyRightQuery
{
    public async Task<ProxyRightDto?> GetProxyRightById(
        [Service] IMediator mediator,
        long proxyId,
        CancellationToken ct)
    {
        return await mediator.Send(new GetProxyRightByIdQuery(proxyId), ct);
    }

    public async Task<IEnumerable<ProxyRightDto>> GetProxyRightsByUser(
        [Service] IMediator mediator,
        long proxyUserId,
        CancellationToken ct)
    {
        return await mediator.Send(new GetProxyRightsByUserQuery(proxyUserId), ct);
    }

    public async Task<IEnumerable<ProxyRightDto>> GetActiveProxyRights(
        [Service] IMediator mediator,
        CancellationToken ct)
    {
        return await mediator.Send(new GetActiveProxyRightsQuery(), ct);
    }
}
