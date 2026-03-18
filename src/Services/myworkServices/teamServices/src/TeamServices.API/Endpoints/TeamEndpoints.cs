using MediatR;
using TeamServices.Application.Commands;
using TeamServices.Application.DTOs;
using TeamServices.Application.Queries;

namespace TeamServices.API.Endpoints;

public static class TeamEndpoints
{
    public static void Map(WebApplication app)
    {
        var group = app.MapGroup("/api/v2/teams").WithTags("Teams (Minimal API)");

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllTeamsQuery(), ct);
            return Results.Ok(result);
        }).WithName("GetAllTeamsMinimal");

        group.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetTeamByIdQuery(id), ct);
            return result is not null ? Results.Ok(result) : Results.NotFound();
        }).WithName("GetTeamByIdMinimal");

        group.MapPost("/", async (CreateTeamCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/teams/{result.TeamId}", result);
        }).RequireAuthorization().WithName("CreateTeamMinimal");

        group.MapPut("/{id:long}", async (long id, UpdateTeamCommand command, IMediator mediator, CancellationToken ct) =>
        {
            if (id != command.TeamId)
                return Results.BadRequest("Route id does not match body TeamId.");
            var result = await mediator.Send(command, ct);
            return Results.Ok(result);
        }).RequireAuthorization().WithName("UpdateTeamMinimal");

        group.MapDelete("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(new DeleteTeamCommand(id), ct);
            return Results.NoContent();
        }).RequireAuthorization().WithName("DeleteTeamMinimal");

        // Employee endpoints
        group.MapGet("/{teamId:long}/employees", async (long teamId, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetTeamEmployeesByTeamIdQuery(teamId), ct);
            return Results.Ok(result);
        }).WithName("GetTeamEmployeesMinimal");

        group.MapPost("/{teamId:long}/employees", async (long teamId, AddTeamEmployeeCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/teams/{teamId}/employees", result);
        }).RequireAuthorization().WithName("AddTeamEmployeeMinimal");

        // Unit map endpoints
        group.MapGet("/{teamId:long}/unitmaps", async (long teamId, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetTeamUnitMapsByTeamIdQuery(teamId), ct);
            return Results.Ok(result);
        }).WithName("GetTeamUnitMapsMinimal");

        group.MapPost("/{teamId:long}/unitmaps", async (long teamId, AddTeamUnitMapCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/teams/{teamId}/unitmaps", result);
        }).RequireAuthorization().WithName("AddTeamUnitMapMinimal");
    }
}
