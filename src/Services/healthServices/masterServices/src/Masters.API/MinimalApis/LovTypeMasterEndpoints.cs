using MediatR;
using Masters.Application.Commands;
using Masters.Application.Queries;
using Masters.Application.DTOs;

namespace Masters.API.MinimalApis;

public static class LovTypeMasterEndpoints
{
    public static IEndpointRouteBuilder MapLovTypeMasterEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/api/v2/lov-type-masters")
            .WithTags("LOV Type Masters")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
        {
            var query = new GetAllLovTypeMastersQuery();
            var result = await mediator.Send(query, ct);
            return Results.Ok(result);
        })
        .WithName("GetAllLovTypeMasters");

        group.MapGet("/{lovTypeCode}", async (string lovTypeCode, IMediator mediator, CancellationToken ct) =>
        {
            var query = new GetLovTypeMasterByIdQuery(lovTypeCode);
            var result = await mediator.Send(query, ct);
            return result != null ? Results.Ok(result) : Results.NotFound();
        })
        .WithName("GetLovTypeMasterById");

        group.MapPost("/", async (CreateLovTypeMasterDto dto, IMediator mediator, CancellationToken ct) =>
        {
            var command = new CreateLovTypeMasterCommand(dto.LovTypeCode, dto.LovTypeName);
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/lov-type-masters/{result.LovTypeCode}", result);
        })
        .WithName("CreateLovTypeMaster");

        group.MapPut("/{lovTypeCode}", async (string lovTypeCode, UpdateLovTypeMasterDto dto, IMediator mediator, CancellationToken ct) =>
        {
            var command = new UpdateLovTypeMasterCommand(lovTypeCode, dto.LovTypeName);
            var result = await mediator.Send(command, ct);
            return Results.Ok(result);
        })
        .WithName("UpdateLovTypeMaster");

        group.MapDelete("/{lovTypeCode}", async (string lovTypeCode, IMediator mediator, CancellationToken ct) =>
        {
            var command = new DeleteLovTypeMasterCommand(lovTypeCode);
            var result = await mediator.Send(command, ct);
            return result ? Results.NoContent() : Results.NotFound();
        })
        .WithName("DeleteLovTypeMaster");

        return builder;
    }
}
