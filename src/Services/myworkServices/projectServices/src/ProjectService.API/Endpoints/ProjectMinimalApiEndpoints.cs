using MediatR;
using ProjectService.Application.Commands;
using ProjectService.Application.DTOs;
using ProjectService.Application.Queries;

namespace ProjectService.API.Endpoints;

public static class ProjectMinimalApiEndpoints
{
    public static void MapProjectEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/projects")
            .WithTags("Projects (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllProjectsQuery(), ct);
            return Results.Ok(result);
        }).WithName("GetAllProjectsV2");

        group.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetProjectByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetProjectByIdV2");

        group.MapGet("/{id:long}/details", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetProjectWithDetailsQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetProjectWithDetailsV2");

        group.MapPost("/", async (CreateProjectCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/projects/{result.ProjId}", result);
        }).WithName("CreateProjectV2");

        group.MapPut("/{id:long}", async (long id, UpdateProjectCommand command, IMediator mediator, CancellationToken ct) =>
        {
            if (id != command.ProjId) return Results.BadRequest("ID mismatch.");
            var result = await mediator.Send(command, ct);
            return Results.Ok(result);
        }).WithName("UpdateProjectV2");

        group.MapDelete("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(new DeleteProjectCommand(id), ct);
            return Results.NoContent();
        }).WithName("DeleteProjectV2");

        group.MapPost("/{id:long}/close", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new CloseProjectCommand(id), ct);
            return Results.Ok(result);
        }).WithName("CloseProjectV2");

        // Lookup endpoints
        var lookups = app.MapGroup("/api/v2/lookups")
            .WithTags("Lookups (Minimal API)")
            .RequireAuthorization();

        lookups.MapGet("/locations", async (IMediator mediator, CancellationToken ct)
            => Results.Ok(await mediator.Send(new GetAllLocationsQuery(), ct))).WithName("GetLocationsV2");

        lookups.MapGet("/processes", async (IMediator mediator, CancellationToken ct)
            => Results.Ok(await mediator.Send(new GetAllProcessesQuery(), ct))).WithName("GetProcessesV2");

        lookups.MapGet("/departments", async (IMediator mediator, CancellationToken ct)
            => Results.Ok(await mediator.Send(new GetAllDepartmentsQuery(), ct))).WithName("GetDepartmentsV2");

        lookups.MapGet("/functions", async (IMediator mediator, CancellationToken ct)
            => Results.Ok(await mediator.Send(new GetAllFunctionsQuery(), ct))).WithName("GetFunctionsV2");

        lookups.MapGet("/categories", async (IMediator mediator, CancellationToken ct)
            => Results.Ok(await mediator.Send(new GetAllCategoriesQuery(), ct))).WithName("GetCategoriesV2");
    }
}
