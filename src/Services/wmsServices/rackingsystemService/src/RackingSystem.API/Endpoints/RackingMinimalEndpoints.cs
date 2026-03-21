using MediatR;
using Microsoft.AspNetCore.Authorization;
using RackingSystem.API.Controllers;
using RackingSystem.Application.Features.Bins.Commands;
using RackingSystem.Application.Features.Bins.Queries;
using RackingSystem.Application.Features.Racks.Commands.CreateRack;
using RackingSystem.Application.Features.Racks.Queries.GetRacks;

namespace RackingSystem.API.Endpoints;

/// <summary>Minimal API endpoint registrations as an alternative route surface.</summary>
public static class RackingMinimalEndpoints
{
    public static IEndpointRouteBuilder MapRackingEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/racking")
            .WithTags("Racking Minimal")
            .RequireAuthorization();

        // Racks
        group.MapGet("/racks", async (IMediator mediator, int? zoneId, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetRacksQuery(zoneId), ct)))
            .WithName("MinimalGetRacks");

        group.MapPost("/racks", async (IMediator mediator, CreateRackCommand cmd, CancellationToken ct) =>
        {
            var result = await mediator.Send(cmd, ct);
            return Results.Created($"/api/v2/racking/racks/{result.Id}", result);
        }).WithName("MinimalCreateRack");

        // Bins by status
        group.MapGet("/bins/status/{status}", async (IMediator mediator, string status, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetBinsQuery(Status: status), ct)))
            .WithName("MinimalGetBinsByStatus");

        // Update bin status
        group.MapPatch("/bins/{id:int}/status", async (IMediator mediator, int id, UpdateBinStatusRequest req, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new UpdateBinStatusCommand(id, req.NewStatus), ct)))
            .WithName("MinimalUpdateBinStatus");

        return app;
    }
}
