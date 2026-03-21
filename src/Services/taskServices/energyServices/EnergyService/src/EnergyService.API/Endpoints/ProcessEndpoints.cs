using EnergyService.Application.Features.Processes.Commands.CreateProcess;
using EnergyService.Application.Features.Processes.Queries.GetAllProcesses;
using EnergyService.Application.Features.Processes.Queries.GetProcessById;
using MediatR;

namespace EnergyService.API.Endpoints;

public static class ProcessEndpoints
{
    public static void MapProcessEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/minimal/processes")
            .WithTags("Processes (Minimal)")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllProcessesQuery(), ct)));

        group.MapGet("/{id:int}", async (int id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetProcessByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        group.MapPost("/", async (CreateProcessCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/minimal/processes/{result.EcProcessId}", result);
        });
    }
}
