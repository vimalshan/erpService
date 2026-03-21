using MediatR;
using TaskServices.Application.DTOs;
using TaskServices.Application.Features.TaskMails.Commands;
using TaskServices.Application.Features.TaskMails.Queries;

namespace TaskServices.API.Endpoints;

public static class TaskMailEndpoints
{
    public static void MapTaskMailEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/minimal/taskmails")
            .WithTags("TaskMails-Minimal")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllTaskMailsQuery(), ct);
            return Results.Ok(result);
        })
        .WithName("GetAllTaskMailsMinimal")
        .Produces<IReadOnlyList<TaskMailDto>>();

        group.MapGet("/{mid}", async (decimal mid, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetTaskMailByIdQuery(mid), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetTaskMailByIdMinimal")
        .Produces<TaskMailDto>()
        .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapPost("/", async (CreateTaskMailCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return Results.Created($"/api/minimal/taskmails/{id}", id);
        })
        .WithName("CreateTaskMailMinimal")
        .Produces<decimal>(StatusCodes.Status201Created);

        group.MapPut("/{mid}", async (decimal mid, UpdateTaskMailCommand command, IMediator mediator, CancellationToken ct) =>
        {
            if (mid != command.MID) return Results.BadRequest("MID mismatch.");
            await mediator.Send(command, ct);
            return Results.NoContent();
        })
        .WithName("UpdateTaskMailMinimal");

        group.MapDelete("/{mid}", async (decimal mid, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(new DeleteTaskMailCommand(mid), ct);
            return Results.NoContent();
        })
        .WithName("DeleteTaskMailMinimal");
    }
}
