using MediatR;
using RiskService.Application.Commands.Risk;
using RiskService.Application.DTOs;
using RiskService.Application.Queries.Risk;
using RiskService.Application.Queries.RiskType;

namespace RiskService.API.MinimalApis;

public static class RiskEndpoints
{
    public static void MapRiskMinimalApis(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/risks").RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllRisksQuery(), ct);
            return Results.Ok(result);
        }).WithName("GetAllRisksV2").WithOpenApi();

        group.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetRiskByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetRiskByIdV2").WithOpenApi();

        group.MapPost("/", async (CreateRiskCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/risks/{id}", id);
        }).WithName("CreateRiskV2").WithOpenApi();

        // Lookups
        var lookups = app.MapGroup("/api/v2/lookups").RequireAuthorization();

        lookups.MapGet("/risk-types", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllRiskTypesQuery(), ct)))
            .WithName("GetRiskTypesV2").WithOpenApi();

        lookups.MapGet("/impacts", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllRiskImpactsQuery(), ct)))
            .WithName("GetImpactsV2").WithOpenApi();

        lookups.MapGet("/probabilities", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllRiskProbabilitiesQuery(), ct)))
            .WithName("GetProbabilitiesV2").WithOpenApi();

        lookups.MapGet("/ratings", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllRiskRatingsQuery(), ct)))
            .WithName("GetRatingsV2").WithOpenApi();

        lookups.MapGet("/responses", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllRiskResponsesQuery(), ct)))
            .WithName("GetResponsesV2").WithOpenApi();
    }
}
