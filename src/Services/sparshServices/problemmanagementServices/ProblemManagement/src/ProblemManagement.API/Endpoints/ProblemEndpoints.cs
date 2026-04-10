using MediatR;
using ProblemManagement.Application.Commands;
using ProblemManagement.Application.Queries;

namespace ProblemManagement.API.Endpoints;

public static class ProblemEndpoints
{
    public static void MapProblemEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/minimal/problems")
            .WithTags("Problems (Minimal)")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllProblemsQuery(), ct)));

        group.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetProblemByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        group.MapGet("/status/{status}", async (string status, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetProblemsByStatusQuery(status), ct)));

        group.MapPost("/", async (CreateProblemCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/minimal/problems/{result.PrId}", result);
        });

        group.MapGet("/{id:long}/solutions", async (long id, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetSolutionsByProblemQuery(id), ct)));

        group.MapPost("/{id:long}/solutions", async (long id, RecordSolutionCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command with { ProblemId = id }, ct);
            return Results.Created($"/api/minimal/problems/{id}/solutions", result);
        });

        group.MapGet("/functions", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetProblemFunctionsQuery(), ct)));

        group.MapGet("/impacts", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetProblemImpactsQuery(), ct)));
    }
}
