using HotChocolate.Authorization;
using MediatR;
using SciTransactional.Application.DTOs;
using SciTransactional.Application.Queries.GetAdvanceLicenseById;
using SciTransactional.Application.Queries.GetAllAdvanceLicenses;
using SciTransactional.Application.Queries.GetAllNavigations;
using SciTransactional.Application.Queries.GetAllNorms;
using SciTransactional.Application.Queries.GetAutoMailStatus;
using SciTransactional.Application.Queries.GetDirectEntries;
using SciTransactional.Application.Queries.GetNavigationById;
using SciTransactional.Application.Queries.GetNormById;
using SciTransactional.Application.Queries.GetOrderMaps;

namespace SciTransactional.API.GraphQL.Queries;

[Authorize]
public sealed class TransactionalQueryType
{
    public async Task<IReadOnlyList<NavigationDto>> GetNavigations(
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllNavigationsQuery(), ct);

    public async Task<NavigationDto?> GetNavigationById(
        long requestNum, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetNavigationByIdQuery(requestNum), ct);

    public async Task<IReadOnlyList<NormsMainDto>> GetNorms(
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllNormsQuery(), ct);

    public async Task<NormsMainDto?> GetNormById(
        long normNo, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetNormByIdQuery(normNo), ct);

    public async Task<IReadOnlyList<AdvanceLicenseDto>> GetAdvanceLicenses(
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllAdvanceLicensesQuery(), ct);

    public async Task<AdvanceLicenseDto?> GetAdvanceLicenseById(
        long licenseId, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAdvanceLicenseByIdQuery(licenseId), ct);

    public async Task<IReadOnlyList<AutoMailStatusDto>> GetAutoMailStatus(
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAutoMailStatusQuery(), ct);

    public async Task<IReadOnlyList<OrderMapDto>> GetOrderMaps(
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetOrderMapsQuery(), ct);

    public async Task<IReadOnlyList<DirectEntryDto>> GetDirectEntries(
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetDirectEntriesQuery(), ct);
}
