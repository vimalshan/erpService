using LovService.Application.Commands.LovMaster;
using LovService.Application.DTOs;
using LovService.Application.Queries.LovMaster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LovService.API.Endpoints;

public static class LovMasterEndpoints
{
    public static RouteGroupBuilder MapLovMasterEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllLovMastersQuery(), ct)))
        .WithName("GetAllLovMasters")
        .WithSummary("Get all LOV masters")
        .Produces<IEnumerable<LovMasterDto>>();

        group.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLovMasterByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetLovMasterById")
        .WithSummary("Get LOV master by ID")
        .Produces<LovMasterDto>()
        .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/by-type/{lovTypeId:long}", async (long lovTypeId, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetLovMastersByTypeQuery(lovTypeId), ct)))
        .WithName("GetLovMastersByType")
        .WithSummary("Get LOV masters by type ID")
        .Produces<IEnumerable<LovMasterDto>>();

        group.MapPost("/", async ([FromBody] CreateLovMasterRequest req, IMediator mediator, CancellationToken ct) =>
        {
            var id = await mediator.Send(new CreateLovMasterCommand(req.LovId, req.LovTypeId, req.LovName, req.UpdatedBy), ct);
            return Results.CreatedAtRoute("GetLovMasterById", new { id }, new { lovId = id });
        })
        .WithName("CreateLovMaster")
        .WithSummary("Create a new LOV master entry")
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);

        group.MapPut("/{id:long}", async (long id, [FromBody] UpdateLovMasterRequest req, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(new UpdateLovMasterCommand(id, req.LovName, req.UpdatedBy), ct);
            return Results.NoContent();
        })
        .WithName("UpdateLovMaster")
        .WithSummary("Update a LOV master entry")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(new DeleteLovMasterCommand(id), ct);
            return Results.NoContent();
        })
        .WithName("DeleteLovMaster")
        .WithSummary("Delete a LOV master entry")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        return group;
    }
}
