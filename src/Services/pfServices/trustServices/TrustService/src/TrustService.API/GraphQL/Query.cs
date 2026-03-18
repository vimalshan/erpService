using MediatR;
using TrustService.Application.DTOs;
using TrustService.Application.Features.Trusts.Queries;

namespace TrustService.API.GraphQL;

public class Query
{
    public async Task<IReadOnlyList<TrustMasterDto>> GetAllTrusts(
        [Service] ISender mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetAllTrustsQuery(), cancellationToken);
    }

    public async Task<IReadOnlyList<TrustMasterDto>> GetActiveTrusts(
        [Service] ISender mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetActiveTrustsQuery(), cancellationToken);
    }

    public async Task<TrustMasterDto?> GetTrustByCode(
        string trustCode,
        [Service] ISender mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetTrustByCodeQuery(trustCode), cancellationToken);
    }
}
