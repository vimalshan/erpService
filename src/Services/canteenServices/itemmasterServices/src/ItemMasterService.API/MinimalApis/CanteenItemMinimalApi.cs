using MediatR;
using ItemMasterService.Application.CQRS.Commands;
using ItemMasterService.Application.CQRS.Queries;

namespace ItemMasterService.API.MinimalApis;

public static class CanteenItemMinimalApi
{
    public static IEndpointRouteBuilder MapCanteenItemMinimalApis(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/canteen-items")
            .WithTags("CanteenItems-Minimal")
            .RequireAuthorization();

        group.MapGet("/{canteenUnitCode}", async (long canteenUnitCode, IMediator mediator, CancellationToken ct) =>
        {
            var items = await mediator.Send(new GetAllCanteenItemsQuery(canteenUnitCode), ct);
            return Results.Ok(items);
        })
        .WithName("GetAllCanteenItemsMinimal")
        .WithSummary("Get all items for a canteen unit (minimal API)");

        group.MapGet("/{canteenUnitCode}/{itemCode}", async (long canteenUnitCode, long itemCode, IMediator mediator, CancellationToken ct) =>
        {
            var item = await mediator.Send(new GetCanteenItemByIdQuery(canteenUnitCode, itemCode), ct);
            return item is null ? Results.NotFound() : Results.Ok(item);
        })
        .WithName("GetCanteenItemMinimal");

        group.MapPost("/", async (CreateCanteenItemCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/canteen-items/{result.CanteenUnitCode}/{result.ItemCode}", result);
        })
        .WithName("CreateCanteenItemMinimal");

        group.MapDelete("/{canteenUnitCode}/{itemCode}", async (long canteenUnitCode, long itemCode, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(new DeleteCanteenItemCommand(canteenUnitCode, itemCode), ct);
            return Results.NoContent();
        })
        .WithName("DeleteCanteenItemMinimal");

        return app;
    }
}
