using EnergyService.Application.Features.Readings.Commands.InsertReading;
using EnergyService.Application.Features.Readings.Queries.GetReadingsByProcess;
using MediatR;

namespace EnergyService.API.Endpoints;

public static class ReadingEndpoints
{
    public static void MapReadingEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/minimal/readings")
            .WithTags("Readings (Minimal)")
            .RequireAuthorization();

        group.MapGet("/process/{processId:int}", async (int processId, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetReadingsByProcessQuery(processId), ct)));

        group.MapPost("/", async (InsertReadingCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/minimal/readings/{result.EbId}", result);
        });
    }
}
