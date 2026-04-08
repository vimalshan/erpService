using HotChocolate.Authorization;
using MediatR;
using SciTransactional.Application.Commands.CloseNorm;
using SciTransactional.Application.Commands.CreateAdvanceLicense;
using SciTransactional.Application.Commands.CreateAutoMail;
using SciTransactional.Application.Commands.CreateDirectEntry;
using SciTransactional.Application.Commands.CreateNavigation;
using SciTransactional.Application.Commands.CreateNorm;
using SciTransactional.Application.Commands.CreateOrderMap;
using SciTransactional.Application.Commands.UpdateAdvanceLicense;
using SciTransactional.Application.Commands.UpdateNavigation;

namespace SciTransactional.API.GraphQL.Mutations;

[Authorize]
public sealed class TransactionalMutationType
{
    public async Task<long> CreateNavigation(
        CreateNavigationCommand input, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<bool> UpdateNavigationStatus(
        UpdateNavigationCommand input, [Service] IMediator mediator, CancellationToken ct)
    {
        await mediator.Send(input, ct);
        return true;
    }

    public async Task<long> CreateNorm(
        CreateNormCommand input, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<bool> CloseNorm(
        long normNo, [Service] IMediator mediator, CancellationToken ct)
    {
        await mediator.Send(new CloseNormCommand(normNo), ct);
        return true;
    }

    public async Task<long> CreateAdvanceLicense(
        CreateAdvanceLicenseCommand input, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<bool> UpdateAdvanceLicense(
        UpdateAdvanceLicenseCommand input, [Service] IMediator mediator, CancellationToken ct)
    {
        await mediator.Send(input, ct);
        return true;
    }

    public async Task<int> CreateAutoMailStatus(
        CreateAutoMailStatusCommand input, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<int> CreateOrderMap(
        CreateOrderMapCommand input, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<long> CreateDirectEntry(
        CreateDirectEntryCommand input, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(input, ct);
}
