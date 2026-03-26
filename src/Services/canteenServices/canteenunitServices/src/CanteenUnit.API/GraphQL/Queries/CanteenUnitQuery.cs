using CanteenUnit.Application.DTOs;
using CanteenUnit.Application.Features.CanteenUnits.Queries.GetAllCanteenUnits;
using CanteenUnit.Application.Features.CanteenUnits.Queries.GetCanteenUnit;
using CanteenUnit.Application.Features.CanteenMasters.Queries;
using HotChocolate;
using MediatR;

namespace CanteenUnit.API.GraphQL.Queries;

public class CanteenUnitQuery
{
    public async Task<IEnumerable<CanteenUnitMasterDto>> GetCanteenUnitsAsync(
        [Service] IMediator mediator,
        CancellationToken ct)
        => await mediator.Send(new GetAllCanteenUnitsQuery(), ct);

    public async Task<CanteenUnitMasterDto?> GetCanteenUnitAsync(
        decimal comCode,
        [Service] IMediator mediator,
        CancellationToken ct)
        => await mediator.Send(new GetCanteenUnitQuery(comCode), ct);

    public async Task<IEnumerable<CanteenMasterDto>> GetCanteenMastersAsync(
        [Service] IMediator mediator,
        CancellationToken ct)
        => await mediator.Send(new GetAllCanteenMastersQuery(), ct);
}
