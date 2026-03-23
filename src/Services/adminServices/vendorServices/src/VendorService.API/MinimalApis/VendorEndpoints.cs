using MediatR;
using Microsoft.AspNetCore.Authorization;
using VendorService.Application.Commands;
using VendorService.Application.Queries;
using VendorService.Application.DTOs;

namespace VendorService.API.MinimalApis;

public static class VendorEndpoints
{
    public static IEndpointRouteBuilder MapVendorMinimalApis(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/vendors")
            .RequireAuthorization()
            .WithTags("Vendors (Minimal API)");

        group.MapGet("/", async (IMediator mediator, string? status, CancellationToken ct) =>
        {
            var vendors = await mediator.Send(new GetAllVendorsQuery(status), ct);
            return Results.Ok(vendors);
        })
        .WithName("GetVendorsV2")
        .WithSummary("Get all vendors (Minimal API version)");

        group.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var vendor = await mediator.Send(new GetVendorByIdQuery(id), ct);
            return vendor is null ? Results.NotFound() : Results.Ok(vendor);
        })
        .WithName("GetVendorByIdV2");

        group.MapPost("/", async (CreateVendorCommand cmd, IMediator mediator, CancellationToken ct) =>
        {
            var id = await mediator.Send(cmd, ct);
            return Results.Created($"/api/v2/vendors/{id}", id);
        })
        .WithName("CreateVendorV2");

        group.MapPut("/{id:long}", async (long id, UpdateVendorCommand cmd, IMediator mediator, CancellationToken ct) =>
        {
            if (id != cmd.VendorId) return Results.BadRequest("ID mismatch.");
            var updated = await mediator.Send(cmd, ct);
            return updated ? Results.NoContent() : Results.NotFound();
        })
        .WithName("UpdateVendorV2");

        group.MapDelete("/{id:long}", async (long id, long updatedBy, IMediator mediator, CancellationToken ct) =>
        {
            var deactivated = await mediator.Send(new DeactivateVendorCommand(id, updatedBy), ct);
            return deactivated ? Results.NoContent() : Results.NotFound();
        })
        .WithName("DeactivateVendorV2");

        return app;
    }
}
