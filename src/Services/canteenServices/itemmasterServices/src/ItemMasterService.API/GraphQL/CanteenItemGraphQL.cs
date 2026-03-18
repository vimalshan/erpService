using ItemMasterService.Application.DTOs;
using ItemMasterService.Application.CQRS.Queries;
using ItemMasterService.Application.CQRS.Commands;
using MediatR;

namespace ItemMasterService.API.GraphQL;

public class CanteenItemQuery
{
    public async Task<IEnumerable<CanteenItemMasterDto>> GetCanteenItemsAsync(
        long canteenUnitCode,
        [Service] IMediator mediator,
        CancellationToken ct) =>
        await mediator.Send(new GetAllCanteenItemsQuery(canteenUnitCode), ct);

    public async Task<CanteenItemMasterDto?> GetCanteenItemAsync(
        long canteenUnitCode,
        long itemCode,
        [Service] IMediator mediator,
        CancellationToken ct) =>
        await mediator.Send(new GetCanteenItemByIdQuery(canteenUnitCode, itemCode), ct);

    public async Task<CanteenItemPriceMasterDto?> GetActivePriceAsync(
        long canteenUnitCode,
        long itemCode,
        [Service] IMediator mediator,
        CancellationToken ct) =>
        await mediator.Send(new GetItemPriceQuery(canteenUnitCode, itemCode), ct);

    public async Task<IEnumerable<CanteenGradeItemPriceDto>> GetGradeItemPricesAsync(
        [Service] IMediator mediator,
        CancellationToken ct) =>
        await mediator.Send(new GetAllGradeItemPricesQuery(), ct);
}

public class CanteenItemMutation
{
    public async Task<CanteenItemMasterDto> CreateCanteenItemAsync(
        CreateCanteenItemInput input,
        [Service] IMediator mediator,
        CancellationToken ct) =>
        await mediator.Send(new CreateCanteenItemCommand(
            input.CanteenUnitCode, input.ItemCode, input.ItemDescription,
            input.ItemType, input.ItemReference, input.EnteredBy), ct);

    public async Task<CanteenItemMasterDto> UpdateCanteenItemAsync(
        UpdateCanteenItemInput input,
        [Service] IMediator mediator,
        CancellationToken ct) =>
        await mediator.Send(new UpdateCanteenItemCommand(
            input.CanteenUnitCode, input.ItemCode, input.ItemDescription,
            input.ItemType, input.ItemReference), ct);

    public async Task<bool> DeleteCanteenItemAsync(
        long canteenUnitCode,
        long itemCode,
        [Service] IMediator mediator,
        CancellationToken ct) =>
        await mediator.Send(new DeleteCanteenItemCommand(canteenUnitCode, itemCode), ct);
}

public record CreateCanteenItemInput(
    long CanteenUnitCode,
    long ItemCode,
    string? ItemDescription,
    string? ItemType,
    string? ItemReference,
    string EnteredBy);

public record UpdateCanteenItemInput(
    long CanteenUnitCode,
    long ItemCode,
    string? ItemDescription,
    string? ItemType,
    string? ItemReference);
