using MediatR;
using LocationService.Application.Commands.Locations;
using LocationService.Application.Commands.Rooms;
using LocationService.Application.Commands.RoomResources;
using LocationService.Application.Queries.Locations;
using LocationService.Application.Queries.Rooms;
using LocationService.Application.Queries.RoomResources;
using LocationService.Application.DTOs;

namespace LocationService.API.Endpoints;

public static class LocationEndpoints
{
    public static void MapLocationEndpoints(this WebApplication app)
    {
        var locations = app.MapGroup("/api/minimal/locations")
            .WithTags("Minimal-Locations");

        locations.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllLocationsQuery(), ct);
            return Results.Ok(result);
        }).WithName("MinimalGetAllLocations").AllowAnonymous();

        locations.MapGet("/active", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetActiveLocationsQuery(), ct);
            return Results.Ok(result);
        }).WithName("MinimalGetActiveLocations").AllowAnonymous();

        locations.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLocationByIdQuery(id), ct);
            return result is null ? Results.NotFound(new { message = $"Location {id} not found" }) : Results.Ok(result);
        }).WithName("MinimalGetLocationById").AllowAnonymous();

        locations.MapGet("/code/{code}", async (string code, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLocationByCodeQuery(code), ct);
            return result is null ? Results.NotFound(new { message = $"Location '{code}' not found" }) : Results.Ok(result);
        }).WithName("MinimalGetLocationByCode").AllowAnonymous();

        locations.MapGet("/search", async (string searchText, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new SearchLocationsByNameQuery(searchText), ct);
            return Results.Ok(result);
        }).WithName("MinimalSearchLocations").AllowAnonymous();

        locations.MapPost("/", async (CreateLocationDto dto, IMediator mediator, CancellationToken ct) =>
        {
            var command = new CreateLocationCommand
            {
                LocationCode = dto.LocationCode,
                LocationName = dto.LocationName,
                StreetAddress = dto.StreetAddress,
                City = dto.City,
                State = dto.State,
                PostalCode = dto.PostalCode,
                Country = dto.Country,
                Phone = dto.Phone,
                Email = dto.Email,
                ContactPerson = dto.ContactPerson,
                UserId = 1
            };
            var result = await mediator.Send(command, ct);
            return Results.CreatedAtRoute("MinimalGetLocationById", new { id = result.LocationId }, result);
        }).WithName("MinimalCreateLocation").AllowAnonymous();

        locations.MapPut("/{id:long}", async (long id, UpdateLocationDto dto, IMediator mediator, CancellationToken ct) =>
        {
            var command = new UpdateLocationCommand
            {
                LocationId = id,
                LocationName = dto.LocationName,
                StreetAddress = dto.StreetAddress,
                City = dto.City,
                State = dto.State,
                PostalCode = dto.PostalCode,
                Country = dto.Country,
                Phone = dto.Phone,
                Email = dto.Email,
                ContactPerson = dto.ContactPerson,
                UserId = 1
            };
            var result = await mediator.Send(command, ct);
            return Results.Ok(result);
        }).WithName("MinimalUpdateLocation").AllowAnonymous();

        locations.MapDelete("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(new DeleteLocationCommand { LocationId = id, UserId = 1 }, ct);
            return Results.NoContent();
        }).WithName("MinimalDeleteLocation").AllowAnonymous();

        // ── Rooms ─────────────────────────────────────────────────────────────

        var rooms = app.MapGroup("/api/minimal/rooms")
            .WithTags("Minimal-Rooms");

        rooms.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetRoomByIdQuery(id), ct);
            return result is null ? Results.NotFound(new { message = $"Room {id} not found" }) : Results.Ok(result);
        }).WithName("MinimalGetRoomById").AllowAnonymous();

        rooms.MapGet("/location/{locationId:long}", async (long locationId, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetRoomsByLocationQuery(locationId), ct);
            return Results.Ok(result);
        }).WithName("MinimalGetRoomsByLocation").AllowAnonymous();

        rooms.MapPost("/", async (CreateRoomDto dto, IMediator mediator, CancellationToken ct) =>
        {
            var command = new CreateRoomCommand
            {
                LocationId = dto.LocationId,
                RoomCode = dto.RoomCode,
                RoomName = dto.RoomName,
                RoomCapacity = dto.RoomCapacity,
                RoomType = dto.RoomType,
                FloorNumber = dto.FloorNumber,
                UserId = 1
            };
            var result = await mediator.Send(command, ct);
            return Results.CreatedAtRoute("MinimalGetRoomById", new { id = result.RoomId }, result);
        }).WithName("MinimalCreateRoom").AllowAnonymous();

        rooms.MapPut("/{id:long}", async (long id, UpdateRoomDto dto, IMediator mediator, CancellationToken ct) =>
        {
            var command = new UpdateRoomCommand
            {
                RoomId = id,
                RoomName = dto.RoomName,
                RoomCapacity = dto.RoomCapacity,
                RoomType = dto.RoomType,
                FloorNumber = dto.FloorNumber,
                UserId = 1
            };
            var result = await mediator.Send(command, ct);
            return Results.Ok(result);
        }).WithName("MinimalUpdateRoom").AllowAnonymous();

        rooms.MapDelete("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(new DeleteRoomCommand { RoomId = id, UserId = 1 }, ct);
            return Results.NoContent();
        }).WithName("MinimalDeleteRoom").AllowAnonymous();

        // ── Room Resources ─────────────────────────────────────────────────────

        var resources = app.MapGroup("/api/minimal/roomresources")
            .WithTags("Minimal-RoomResources");

        resources.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetRoomResourceByIdQuery(id), ct);
            return result is null ? Results.NotFound(new { message = $"Resource {id} not found" }) : Results.Ok(result);
        }).WithName("MinimalGetResourceById").AllowAnonymous();

        resources.MapGet("/room/{roomId:long}", async (long roomId, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetRoomResourcesByRoomQuery(roomId), ct);
            return Results.Ok(result);
        }).WithName("MinimalGetResourcesByRoom").AllowAnonymous();

        resources.MapPost("/", async (CreateRoomResourceDto dto, IMediator mediator, CancellationToken ct) =>
        {
            var command = new CreateRoomResourceCommand
            {
                RoomId = dto.RoomId,
                LocationId = dto.LocationId,
                ResourceCode = dto.ResourceCode,
                ResourceName = dto.ResourceName,
                ResourceType = dto.ResourceType,
                ResourceQuantity = dto.ResourceQuantity,
                UserId = 1
            };
            var result = await mediator.Send(command, ct);
            return Results.CreatedAtRoute("MinimalGetResourceById", new { id = result.ResourceId }, result);
        }).WithName("MinimalCreateResource").AllowAnonymous();
    }
}
