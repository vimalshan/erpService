using MediatR;
using InventoryService.Application.Commands.ReceiveStock;
using InventoryService.Application.Commands.TransferInventory;
using InventoryService.Application.Commands.AdjustInventory;
using InventoryService.Application.Queries.GetStockLevel;
using InventoryService.Application.Queries.GetInventoryByWarehouse;
using InventoryService.Application.Queries.GetAvailableStock;
using InventoryService.Application.Queries.GetLowStockItems;

namespace InventoryService.API.Endpoints;

public static class InventoryEndpoints
{
    public static void MapInventoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/minimal/inventory")
            .WithTags("Inventory (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/stock/{stockLevelId:long}", async (long stockLevelId, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetStockLevelQuery(stockLevelId));
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("MinimalGetStockLevel");

        group.MapGet("/warehouse/{warehouseId:int}", async (int warehouseId, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetInventoryByWarehouseQuery(warehouseId));
            return Results.Ok(result);
        })
        .WithName("MinimalGetByWarehouse");

        group.MapGet("/available", async (int productId, int? warehouseId, int? binId, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAvailableStockQuery(productId, warehouseId, binId));
            return Results.Ok(result);
        })
        .WithName("MinimalGetAvailable");

        group.MapGet("/low-stock", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetLowStockItemsQuery());
            return Results.Ok(result);
        })
        .WithName("MinimalGetLowStock");

        group.MapPost("/receive", async (ReceiveStockCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return Results.Created($"/api/minimal/inventory/stock/{result.StockLevelId}", result);
        })
        .WithName("MinimalReceiveStock");

        group.MapPost("/transfer", async (TransferInventoryCommand command, IMediator mediator) =>
        {
            await mediator.Send(command);
            return Results.NoContent();
        })
        .WithName("MinimalTransfer");

        group.MapPost("/adjust", async (AdjustInventoryCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return Results.Ok(result);
        })
        .WithName("MinimalAdjust");
    }
}
