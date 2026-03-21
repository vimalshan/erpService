using MasterDataService.Application.Commands.GuestHouse;
using MasterDataService.Application.DTOs;
using MasterDataService.Application.Queries.GuestHouse;
using MasterDataService.Application.Queries.Area;
using MasterDataService.Application.Queries.Route;
using MediatR;

namespace MasterDataService.API.Endpoints;

public static class MinimalApiEndpoints
{
    public static WebApplication MapMinimalApis(this WebApplication app)
    {
        var masterData = app.MapGroup("/api/v2/masterdata").WithTags("MasterData-MinimalAPI");

        // Guest Houses
        masterData.MapGet("/guesthouses", async (IMediator mediator) =>
            Results.Ok(await mediator.Send(new GetAllGuestHousesQuery())))
            .WithName("GetAllGuestHousesMinimal");

        masterData.MapGet("/guesthouses/{id:long}", async (long id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetGuestHouseByIdQuery(id));
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetGuestHouseByIdMinimal");

        masterData.MapPost("/guesthouses", async (CreateGuestHouseCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return Results.Created($"/api/v2/masterdata/guesthouses/{result.Id}", result);
        }).RequireAuthorization().WithName("CreateGuestHouseMinimal");

        // Areas
        masterData.MapGet("/areas", async (IMediator mediator) =>
            Results.Ok(await mediator.Send(new GetAllAreasQuery())))
            .WithName("GetAllAreasMinimal");

        masterData.MapGet("/areas/{id:long}", async (long id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAreaByIdQuery(id));
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetAreaByIdMinimal");

        // Routes
        masterData.MapGet("/routes", async (IMediator mediator) =>
            Results.Ok(await mediator.Send(new GetAllRoutesQuery())))
            .WithName("GetAllRoutesMinimal");

        masterData.MapGet("/routes/{id:long}", async (long id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetRouteByIdQuery(id));
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetRouteByIdMinimal");

        return app;
    }
}
