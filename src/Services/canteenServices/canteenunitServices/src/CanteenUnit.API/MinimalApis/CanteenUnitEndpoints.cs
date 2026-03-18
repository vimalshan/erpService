using CanteenUnit.Application.Features.CanteenUnits.Commands.CreateCanteenUnit;
using CanteenUnit.Application.Features.CanteenUnits.Queries.GetAllCanteenUnits;
using CanteenUnit.Application.Features.CanteenUnits.Queries.GetCanteenUnit;
using CanteenUnit.Infrastructure.Dapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CanteenUnit.API.MinimalApis;

public static class CanteenUnitEndpoints
{
    public static IEndpointRouteBuilder MapCanteenUnitEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/canteen-units")
            .WithTags("CanteenUnits (Minimal)")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllCanteenUnitsQuery(), ct)))
            .WithName("GetAllCanteenUnitsV2")
            .WithSummary("Get all canteen units");

        group.MapGet("/{comCode:decimal}", async (decimal comCode, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetCanteenUnitQuery(comCode), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetCanteenUnitV2");

        group.MapPost("/", async (CreateCanteenUnitCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/canteen-units/{result.UnComCod}", result);
        })
        .WithName("CreateCanteenUnitV2");

        // Dapper read-side demonstration
        group.MapGet("/search", async (string? name, CanteenUnitDapperRepository dapper, CancellationToken ct) =>
            Results.Ok(await dapper.SearchUnitsAsync(name)))
            .WithName("SearchCanteenUnitsV2")
            .WithSummary("Search units (Dapper)");

        group.MapGet("/with-access-count", async (CanteenUnitDapperRepository dapper) =>
            Results.Ok(await dapper.GetUnitsWithAccessCountAsync()))
            .WithName("GetUnitsWithAccessCountV2");

        return app;
    }
}
