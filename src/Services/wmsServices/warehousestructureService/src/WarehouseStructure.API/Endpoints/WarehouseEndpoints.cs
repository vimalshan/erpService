using MediatR;
using WarehouseStructure.Application.Commands.CreateWarehouse;
using WarehouseStructure.Application.Commands.DeleteWarehouse;
using WarehouseStructure.Application.Commands.UpdateWarehouse;
using WarehouseStructure.Application.DTOs;
using WarehouseStructure.Application.Queries.GetAllWarehouses;
using WarehouseStructure.Application.Queries.GetWarehouseById;

namespace WarehouseStructure.API.Endpoints;

public static class WarehouseEndpoints
{
    public static void MapWarehouseEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/v2/warehouses")
            .WithTags("Warehouses (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllWarehousesQuery(), ct);
            return Results.Ok(result);
        }).WithName("GetAllWarehousesMinimal");

        group.MapGet("/{id:int}", async (int id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetWarehouseByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetWarehouseByIdMinimal");

        group.MapPost("/", async (CreateWarehouseDto dto, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new CreateWarehouseCommand(dto), ct);
            return Results.Created($"/api/v2/warehouses/{result.WarehouseId}", result);
        }).WithName("CreateWarehouseMinimal");

        group.MapPut("/{id:int}", async (int id, UpdateWarehouseDto dto, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new UpdateWarehouseCommand(id, dto), ct);
            return Results.Ok(result);
        }).WithName("UpdateWarehouseMinimal");

        group.MapDelete("/{id:int}", async (int id, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(new DeleteWarehouseCommand(id), ct);
            return Results.NoContent();
        }).WithName("DeleteWarehouseMinimal");
    }
}
