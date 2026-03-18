using LovService.Application.Commands.ItemData;
using LovService.Application.DTOs;
using LovService.Application.Queries.ItemData;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LovService.API.Endpoints;

public static class ItemDataEndpoints
{
    public static RouteGroupBuilder MapItemDataEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllItemDataQuery(), ct)))
        .WithName("GetAllItemData")
        .WithSummary("Get all item data")
        .Produces<IEnumerable<ItemDataDto>>();

        group.MapGet("/{id:int}", async (int id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetItemDataByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetItemDataById")
        .Produces<ItemDataDto>()
        .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/search", async ([FromQuery] string? catName, [FromQuery] string? itemName, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new SearchItemDataQuery(catName, itemName), ct)))
        .WithName("SearchItemData")
        .WithSummary("Search item data by category or item name")
        .Produces<IEnumerable<ItemDataDto>>();

        group.MapPost("/", async ([FromBody] CreateItemDataRequest req, IMediator mediator, CancellationToken ct) =>
        {
            var id = await mediator.Send(new CreateItemDataCommand(req.CatName, req.ItemName, req.Make, req.Uom, req.Price), ct);
            return Results.CreatedAtRoute("GetItemDataById", new { id }, new { id });
        })
        .WithName("CreateItemData")
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);

        group.MapPut("/{id:int}", async (int id, [FromBody] UpdateItemDataRequest req, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(new UpdateItemDataCommand(id, req.CatName, req.ItemName, req.Make, req.Uom, req.Price), ct);
            return Results.NoContent();
        })
        .WithName("UpdateItemData")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{id:int}", async (int id, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(new DeleteItemDataCommand(id), ct);
            return Results.NoContent();
        })
        .WithName("DeleteItemData")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        return group;
    }
}
