using MediatR;
using TdsService.Application.DTOs;
using TdsService.Application.Vendors.Queries.GetAllTdsVendors;
using TdsService.Application.Vendors.Queries.GetTdsVendorByPan;
using TdsService.Application.Vendors.Commands.CreateTdsVendor;
using TdsService.Application.Vendors.Commands.UpdateTdsVendor;
using TdsService.Application.Vendors.Commands.DeleteTdsVendor;
using Microsoft.AspNetCore.Mvc;

namespace TdsService.API.MinimalApis;

public static class VendorEndpoints
{
    public static IEndpointRouteBuilder MapVendorEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/vendors")
            .WithTags("Vendors (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/", async (
            [FromServices] IMediator mediator,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken ct = default) =>
        {
            var result = await mediator.Send(new GetAllTdsVendorsQuery(page, pageSize), ct);
            return Results.Ok(result);
        }).Produces<PagedResult<TdsVendorDto>>();

        group.MapGet("/{panNo}", async (
            string panNo,
            [FromServices] IMediator mediator,
            CancellationToken ct = default) =>
        {
            var result = await mediator.Send(new GetTdsVendorByPanQuery(panNo), ct);
            return result is not null ? Results.Ok(result) : Results.NotFound();
        }).Produces<TdsVendorDto>();

        group.MapPost("/", async (
            CreateTdsVendorCommand command,
            [FromServices] IMediator mediator,
            CancellationToken ct = default) =>
        {
            var id = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/vendors/{command.PanNo}", id);
        }).Produces<long>(StatusCodes.Status201Created);

        group.MapPut("/{vendorId:long}", async (
            long vendorId,
            UpdateTdsVendorCommand command,
            [FromServices] IMediator mediator,
            CancellationToken ct = default) =>
        {
            if (vendorId != command.VendorId) return Results.BadRequest("ID mismatch.");
            await mediator.Send(command, ct);
            return Results.NoContent();
        });

        group.MapDelete("/{vendorId:long}", async (
            long vendorId,
            [FromServices] IMediator mediator,
            CancellationToken ct = default) =>
        {
            await mediator.Send(new DeleteTdsVendorCommand(vendorId), ct);
            return Results.NoContent();
        });

        return app;
    }
}
