using LovService.Application.Commands.LovType;
using LovService.Application.DTOs;
using LovService.Application.Queries.LovType;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LovService.API.Endpoints;

public static class LovTypeEndpoints
{
    public static RouteGroupBuilder MapLovTypeEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllLovTypesQuery(), ct);
            return Results.Ok(result);
        })
        .WithName("GetAllLovTypes")
        .WithSummary("Get all LOV types")
        .Produces<IEnumerable<LovTypeDto>>();

        group.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLovTypeByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetLovTypeById")
        .WithSummary("Get LOV type by ID")
        .Produces<LovTypeDto>()
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", async ([FromBody] CreateLovTypeRequest req, IMediator mediator, CancellationToken ct) =>
        {
            var id = await mediator.Send(new CreateLovTypeCommand(req.LovTypeId, req.LovTypeName), ct);
            return Results.CreatedAtRoute("GetLovTypeById", new { id }, new { lovTypeId = id });
        })
        .WithName("CreateLovType")
        .WithSummary("Create a new LOV type")
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);

        group.MapPut("/{id:long}", async (long id, [FromBody] UpdateLovTypeRequest req, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(new UpdateLovTypeCommand(id, req.LovTypeName), ct);
            return Results.NoContent();
        })
        .WithName("UpdateLovType")
        .WithSummary("Update an existing LOV type")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(new DeleteLovTypeCommand(id), ct);
            return Results.NoContent();
        })
        .WithName("DeleteLovType")
        .WithSummary("Delete a LOV type")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        return group;
    }
}
