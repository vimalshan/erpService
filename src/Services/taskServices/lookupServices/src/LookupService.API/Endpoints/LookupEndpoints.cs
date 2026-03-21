using LookupService.Application.Commands;
using LookupService.Application.Queries;
using MediatR;

namespace LookupService.API.Endpoints;

public static class LookupEndpoints
{
    public static void MapLookupEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/v2/lookup").RequireAuthorization();

        // LOV Types
        group.MapGet("/lov-types", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllLovTypesQuery(), ct)));

        group.MapGet("/lov-types/{typeCode}", async (string typeCode, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLovTypeByCodeQuery(typeCode), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        // LOVs
        group.MapGet("/lovs", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllLovsQuery(), ct)));

        group.MapGet("/lovs/{lovId:long}", async (long lovId, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLovByIdQuery(lovId), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        group.MapGet("/lovs/type/{lovType}", async (string lovType, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetLovsByTypeQuery(lovType), ct)));

        group.MapPost("/lovs", async (CreateLovCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/lookup/lovs/{id}", id);
        });

        // Processes
        group.MapGet("/processes", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllProcessesQuery(), ct)));

        group.MapGet("/processes/{processId}", async (decimal processId, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetProcessByIdQuery(processId), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        group.MapPost("/processes", async (CreateProcessCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/lookup/processes/{id}", id);
        });

        // Panels
        group.MapGet("/panels", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllPanelsQuery(), ct)));

        group.MapGet("/panels/{panelId}", async (decimal panelId, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetPanelByIdQuery(panelId), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });
    }
}
