using MasterService.Application.Features.Trainings.Commands;
using MasterService.Application.Features.Trainings.Queries;
using MediatR;

namespace MasterService.API.MinimalApis;

public static class TrainingEndpoints
{
    public static void MapTrainingEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/trainings")
            .WithTags("Trainings (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct)
            => Results.Ok(await mediator.Send(new GetTrainingsQuery(), ct)));

        group.MapGet("/{trainingCode:long}", async (IMediator mediator, long trainingCode, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetTrainingByCodeQuery(trainingCode), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        group.MapPost("/", async (IMediator mediator, CreateTrainingCommand command, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/trainings/{result.TrainingCode}", result);
        });
    }
}
