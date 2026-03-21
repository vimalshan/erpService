using MediatR;
using ProductService.Application.Commands.CreateProduct;
using ProductService.Application.Commands.DeleteProduct;
using ProductService.Application.Commands.UpdateProduct;
using ProductService.Application.DTOs;
using ProductService.Application.Queries.GetAllProducts;
using ProductService.Application.Queries.GetProductById;
using ProductService.Application.Commands.CreateCategory;
using ProductService.Application.Commands.DeleteCategory;
using ProductService.Application.Commands.UpdateCategory;
using ProductService.Application.Queries.GetAllCategories;
using ProductService.Application.Queries.GetCategoryById;

namespace ProductService.API.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/minimal/products").WithTags("Products (Minimal)");

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllProductsQuery(), ct)));

        group.MapGet("/{id:int}", async (int id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetProductByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        group.MapPost("/", async (CreateProductDto dto, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new CreateProductCommand(dto), ct);
            return Results.Created($"/api/minimal/products/{result.ProductId}", result);
        }).RequireAuthorization();

        group.MapPut("/{id:int}", async (int id, UpdateProductDto dto, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new UpdateProductCommand(id, dto), ct);
            return Results.Ok(result);
        }).RequireAuthorization();

        group.MapDelete("/{id:int}", async (int id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new DeleteProductCommand(id), ct);
            return result ? Results.NoContent() : Results.NotFound();
        }).RequireAuthorization();
    }
}

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/minimal/categories").WithTags("Categories (Minimal)");

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllCategoriesQuery(), ct)));

        group.MapGet("/{id:int}", async (int id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetCategoryByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        group.MapPost("/", async (CreateCategoryDto dto, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new CreateCategoryCommand(dto), ct);
            return Results.Created($"/api/minimal/categories/{result.CategoryId}", result);
        }).RequireAuthorization();

        group.MapPut("/{id:int}", async (int id, UpdateCategoryDto dto, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new UpdateCategoryCommand(id, dto), ct);
            return Results.Ok(result);
        }).RequireAuthorization();

        group.MapDelete("/{id:int}", async (int id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new DeleteCategoryCommand(id), ct);
            return result ? Results.NoContent() : Results.NotFound();
        }).RequireAuthorization();
    }
}
