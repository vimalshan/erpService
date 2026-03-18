using MediatR;
using Masters.Application.Commands;
using Masters.Application.Queries;
using Masters.Application.DTOs;

namespace Masters.API.MinimalApis;

public static class LovMasterEndpoints
{
    public static IEndpointRouteBuilder MapLovMasterEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/api/v2/lov-masters")
            .WithTags("LOV Masters")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
        {
            var query = new GetAllLovMastersQuery();
            var result = await mediator.Send(query, ct);
            return Results.Ok(result);
        })
        .WithName("GetAllLovMasters");

        group.MapGet("/type/{lovType}", async (string lovType, IMediator mediator, CancellationToken ct) =>
        {
            var query = new GetLovMastersByTypeQuery(lovType);
            var result = await mediator.Send(query, ct);
            return Results.Ok(result);
        })
        .WithName("GetLovMastersByType");

        group.MapGet("/{lovId:long}", async (long lovId, IMediator mediator, CancellationToken ct) =>
        {
            var query = new GetLovMasterByIdQuery(lovId);
            var result = await mediator.Send(query, ct);
            return result != null ? Results.Ok(result) : Results.NotFound();
        })
        .WithName("GetLovMasterById");

        group.MapPost("/", async (CreateLovMasterDto dto, IMediator mediator, CancellationToken ct) =>
        {
            var command = new CreateLovMasterCommand(dto.LovId, dto.LovType, dto.LovName);
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/lov-masters/{result.LovId}", result);
        })
        .WithName("CreateLovMaster");

        group.MapPut("/{lovId:long}", async (long lovId, UpdateLovMasterDto dto, IMediator mediator, CancellationToken ct) =>
        {
            var command = new UpdateLovMasterCommand(lovId, dto.LovName);
            var result = await mediator.Send(command, ct);
            return Results.Ok(result);
        })
        .WithName("UpdateLovMaster");

        group.MapDelete("/{lovId:long}", async (long lovId, IMediator mediator, CancellationToken ct) =>
        {
            var command = new DeleteLovMasterCommand(lovId);
            var result = await mediator.Send(command, ct);
            return result ? Results.NoContent() : Results.NotFound();
        })
        .WithName("DeleteLovMaster");

        return builder;
    }
}
