using MediatR;
using Microsoft.AspNetCore.Authorization;
using ShipmentService.Application.DTOs;
using ShipmentService.Application.Features.Shipments.Commands.CreateShipment;
using ShipmentService.Application.Features.Shipments.Queries.GetAllShipments;
using ShipmentService.Application.Features.Shipments.Queries.GetShipmentById;

namespace ShipmentService.API.MinimalApis;

public static class ShipmentEndpoints
{
    public static void MapShipmentEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v2/shipments")
            .WithTags("Shipments (Minimal API)")
            .WithOpenApi()
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, int page = 1, int pageSize = 20, CancellationToken ct = default) =>
        {
            var result = await mediator.Send(new GetAllShipmentsQuery(page, pageSize), ct);
            return Results.Ok(result);
        })
        .WithName("GetAllShipmentsV2")
        .WithSummary("Get all shipments (paginated) via Minimal API");

        group.MapGet("/{id:int}", async (int id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetShipmentByIdQuery(id), ct);
            return Results.Ok(result);
        })
        .WithName("GetShipmentByIdV2")
        .WithSummary("Get shipment by ID via Minimal API");

        group.MapPost("/", async (CreateShipmentCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/shipments/{result.ShipmentId}", result);
        })
        .WithName("CreateShipmentV2")
        .WithSummary("Create a new shipment via Minimal API");
    }
}
