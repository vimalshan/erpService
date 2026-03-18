using MediatR;
using TrustService.Application.Features.Trusts.Commands;

namespace TrustService.API.GraphQL;

public class Mutation
{
    public async Task<string> CreateTrust(
        CreateTrustCommand input,
        [Service] ISender mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(input, cancellationToken);
    }

    public async Task<bool> UpdateTrust(
        UpdateTrustCommand input,
        [Service] ISender mediator,
        CancellationToken cancellationToken)
    {
        await mediator.Send(input, cancellationToken);
        return true;
    }

    public async Task<bool> CloseTrust(
        string trustCode,
        DateTime closureDate,
        [Service] ISender mediator,
        CancellationToken cancellationToken)
    {
        await mediator.Send(new CloseTrustCommand(trustCode, closureDate), cancellationToken);
        return true;
    }

    public async Task<bool> ActivateTrust(
        string trustCode,
        [Service] ISender mediator,
        CancellationToken cancellationToken)
    {
        await mediator.Send(new ActivateTrustCommand(trustCode), cancellationToken);
        return true;
    }

    public async Task<bool> AddFundType(
        AddTrustFundTypeCommand input,
        [Service] ISender mediator,
        CancellationToken cancellationToken)
    {
        await mediator.Send(input, cancellationToken);
        return true;
    }

    public async Task<bool> AddRole(
        AddTrustRoleCommand input,
        [Service] ISender mediator,
        CancellationToken cancellationToken)
    {
        await mediator.Send(input, cancellationToken);
        return true;
    }

    public async Task<bool> AddUnit(
        AddTrustUnitCommand input,
        [Service] ISender mediator,
        CancellationToken cancellationToken)
    {
        await mediator.Send(input, cancellationToken);
        return true;
    }
}
