using MediatR;
using WarehouseStructure.Application.Commands.CreateZone;
using WarehouseStructure.Application.Commands.DeleteZone;
using WarehouseStructure.Application.Commands.UpdateZone;
using WarehouseStructure.Application.DTOs;
using WarehouseStructure.Application.Queries.GetAllZones;
using WarehouseStructure.Application.Queries.GetZoneById;

namespace WarehouseStructure.API.Endpoints;

public static class ZoneEndpoints
{
    public static void MapZoneEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/v2/zones")
            .WithTags("Zones (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/", async (int? warehouseId, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllZonesQuery(warehouseId), ct);
            return Results.Ok(result);
        }).WithName("GetAllZonesMinimal");

        group.MapGet("/{id:int}", async (int id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetZoneByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetZoneByIdMinimal");

        group.MapPost("/", async (CreateZoneDto dto, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new CreateZoneCommand(dto), ct);
            return Results.Created($"/api/v2/zones/{result.ZoneId}", result);
        }).WithName("CreateZoneMinimal");

        group.MapPut("/{id:int}", async (int id, UpdateZoneDto dto, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new UpdateZoneCommand(id, dto), ct);
            return Results.Ok(result);
        }).WithName("UpdateZoneMinimal");

        group.MapDelete("/{id:int}", async (int id, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(new DeleteZoneCommand(id), ct);
            return Results.NoContent();
        }).WithName("DeleteZoneMinimal");
    }
}
