using MediatR;
using Microsoft.AspNetCore.Authorization;
using StrategicStock.Application.Commands.CloseStrategicStock;
using StrategicStock.Application.Commands.CreateStrategicStock;
using StrategicStock.Application.Commands.UpdateStrategicStock;
using StrategicStock.Application.DTOs;
using StrategicStock.Application.Queries.GetAllStrategicStocks;
using StrategicStock.Application.Queries.GetStrategicStockById;
using StrategicStock.Application.Queries.GetStrategicStockInfo;

namespace StrategicStock.API.MinimalApis;

public static class StrategicStockEndpoints
{
    public static void MapStrategicStockEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/strategic-stocks")
            .WithTags("StrategicStock-MinimalApi")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllStrategicStocksQuery(), ct);
            return Results.Ok(result);
        }).Produces<IReadOnlyList<StrategicStockDto>>();

        group.MapGet("/{id:int}", async (int id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetStrategicStockByIdQuery(id), ct);
            return result is not null ? Results.Ok(result) : Results.NotFound();
        }).Produces<StrategicStockDto>().Produces(StatusCodes.Status404NotFound);

        group.MapGet("/info", async (int itemId, int companyUnitId, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetStrategicStockInfoQuery(itemId, companyUnitId), ct);
            return Results.Ok(result);
        }).Produces<IReadOnlyList<StrategicStockInfoDto>>();

        group.MapPost("/", async (CreateStrategicStockCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/strategic-stocks/{id}", id);
        }).Produces<int>(StatusCodes.Status201Created);

        group.MapPut("/{id:int}", async (int id, UpdateStrategicStockCommand command, IMediator mediator, CancellationToken ct) =>
        {
            if (id != command.StrategicStockId)
                return Results.BadRequest("ID mismatch.");

            await mediator.Send(command, ct);
            return Results.NoContent();
        });

        group.MapPost("/{id:int}/close", async (int id, int? userId, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(new CloseStrategicStockCommand(id, userId), ct);
            return Results.NoContent();
        });
    }
}
