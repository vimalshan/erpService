using CategoryAndVendorService.Application.DTOs;
using CategoryAndVendorService.Application.MainCategories.Commands;
using CategoryAndVendorService.Application.MainCategories.Queries;
using CategoryAndVendorService.Application.SubCategories.Commands;
using CategoryAndVendorService.Application.SubCategories.Queries;
using MediatR;

namespace CategoryAndVendorService.API.MinimalApis;

public static class CategoryEndpoints
{
    public static IEndpointRouteBuilder MapCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/categories").RequireAuthorization();

        group.MapGet("/main", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllMainCategoriesQuery(), ct)));

        group.MapGet("/main/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetMainCategoryByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        group.MapPost("/main", async (CreateMainCategoryCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/categories/main/{result.MainCatId}", result);
        });

        group.MapGet("/sub", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllSubCategoriesQuery(), ct)));

        group.MapGet("/sub/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetSubCategoryByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        group.MapGet("/sub/by-main/{mainCatId:long}", async (long mainCatId, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetSubCategoriesByMainCategoryQuery(mainCatId), ct)));

        group.MapPost("/sub", async (CreateSubCategoryCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/categories/sub/{result.SubCatId}", result);
        });

        return app;
    }
}
