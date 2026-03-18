using LovService.Application.DTOs;
using LovService.Application.Queries.LovType;
using LovService.Application.Queries.LovMaster;
using LovService.Application.Queries.ItemData;
using MediatR;

namespace LovService.API.GraphQL.Queries;

[QueryType]
public class LovQuery
{
    public async Task<IEnumerable<LovTypeDto>> GetLovTypesAsync(
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllLovTypesQuery(), ct);

    public async Task<LovTypeDto?> GetLovTypeAsync(long id,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetLovTypeByIdQuery(id), ct);

    public async Task<IEnumerable<LovMasterDto>> GetLovMastersAsync(
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllLovMastersQuery(), ct);

    public async Task<IEnumerable<LovMasterDto>> GetLovMastersByTypeAsync(long lovTypeId,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetLovMastersByTypeQuery(lovTypeId), ct);

    public async Task<IEnumerable<ItemDataDto>> GetItemDataAsync(
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllItemDataQuery(), ct);

    public async Task<IEnumerable<ItemDataDto>> SearchItemDataAsync(string? catName, string? itemName,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new SearchItemDataQuery(catName, itemName), ct);
}
