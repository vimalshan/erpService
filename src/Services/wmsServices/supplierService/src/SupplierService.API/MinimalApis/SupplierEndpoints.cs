using MediatR;
using SupplierService.Application.DTOs;
using SupplierService.Application.Features.Suppliers.Commands;
using SupplierService.Application.Features.Suppliers.Queries;

namespace SupplierService.API.MinimalApis;

public static class SupplierEndpoints
{
    public static void MapSupplierEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/minimal/suppliers")
            .WithTags("Suppliers (Minimal API)");

        group.MapGet("/", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllSuppliersQuery());
            return Results.Ok(result);
        }).WithName("MinimalGetAllSuppliers");

        group.MapGet("/{id:int}", async (int id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetSupplierByIdQuery(id));
            return result is not null ? Results.Ok(result) : Results.NotFound();
        }).WithName("MinimalGetSupplierById");

        group.MapGet("/paged", async (int page, int pageSize, string? search, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetSuppliersPagedQuery(page, pageSize, search));
            return Results.Ok(result);
        }).WithName("MinimalGetSuppliersPaged");

        group.MapPost("/", async (CreateSupplierDto dto, IMediator mediator) =>
        {
            var result = await mediator.Send(new CreateSupplierCommand(dto));
            return Results.Created($"/api/minimal/suppliers/{result.SupplierId}", result);
        }).RequireAuthorization().WithName("MinimalCreateSupplier");

        group.MapPut("/{id:int}", async (int id, UpdateSupplierDto dto, IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateSupplierCommand(id, dto));
            return Results.Ok(result);
        }).RequireAuthorization().WithName("MinimalUpdateSupplier");

        group.MapDelete("/{id:int}", async (int id, IMediator mediator) =>
        {
            await mediator.Send(new DeleteSupplierCommand(id));
            return Results.NoContent();
        }).RequireAuthorization().WithName("MinimalDeleteSupplier");
    }
}
