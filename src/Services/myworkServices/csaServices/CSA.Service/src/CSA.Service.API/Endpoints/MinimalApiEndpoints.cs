using CSA.Service.Application.Commands.Controls;
using CSA.Service.Application.DTOs;
using CSA.Service.Application.Queries.Controls;
using CSA.Service.Application.Queries.Processes;
using CSA.Service.Application.Queries.Surveys;
using MediatR;

namespace CSA.Service.API.Endpoints;

public static class MinimalApiEndpoints
{
    public static WebApplication MapCsaEndpoints(this WebApplication app)
    {
        var csa = app.MapGroup("/api/v2").RequireAuthorization();

        // Controls
        csa.MapGet("/controls", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllControlsQuery(), ct)));

        csa.MapGet("/controls/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetControlByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        csa.MapPost("/controls", async (CreateControlDto dto, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new CreateControlCommand(dto, 0), ct);
            return Results.Created($"/api/v2/controls/{result.ControlId}", result);
        });

        // Surveys
        csa.MapGet("/surveys", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllSurveysQuery(), ct)));

        csa.MapGet("/surveys/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetSurveyByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        // Processes
        csa.MapGet("/processes", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllProcessesQuery(), ct)));

        csa.MapGet("/processes/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetProcessByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        csa.MapGet("/processes/{id:long}/subprocesses", async (long id, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetSubProcessesByProcessQuery(id), ct)));

        return app;
    }
}
