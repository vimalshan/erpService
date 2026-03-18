using InventoryManagement.Application.Queries.Items;
using InventoryManagement.Application.Queries.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.API.MinimalApis;

public static class InventoryEndpoints
{
    public static IEndpointRouteBuilder MapInventoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/inventory")
            .WithTags("Inventory (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/products", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllProductsQuery(), ct)))
            .WithName("GetAllProductsV2")
            .WithSummary("Get all products (Minimal API)");

        group.MapGet("/products/{id:int}", async (int id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetProductByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetProductByIdV2");

        group.MapGet("/items", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllItemsQuery(), ct)))
            .WithName("GetAllItemsV2")
            .WithSummary("Get all items (Minimal API)");

        group.MapGet("/items/{id:int}", async (int id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetItemByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetItemByIdV2");

        return app;
    }
}
