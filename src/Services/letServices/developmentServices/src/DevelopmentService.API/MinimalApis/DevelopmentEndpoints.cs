using MediatR;
using DevelopmentService.Application.Queries.GetPlans;
using DevelopmentService.Application.Queries.GetCompetencyIndicators;
using DevelopmentService.Application.Commands.CreateLearningPlan;
using DevelopmentService.Application.Commands.ApprovePlan;
using DevelopmentService.Application.Commands.CreateBhrPlan;

namespace DevelopmentService.API.MinimalApis;

public static class DevelopmentEndpoints
{
    public static IEndpointRouteBuilder MapDevelopmentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/development").RequireAuthorization();

        group.MapGet("/plans", async (string? userId, char? status, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetPlansQuery(userId, status), ct)));

        group.MapPost("/plans", async (CreateLearningPlanCommand cmd, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(cmd, ct);
            return Results.Created($"/api/v2/development/plans/{result.ReqNum}", result);
        });

        group.MapPatch("/plans/{reqNum:long}/status",
            async (long reqNum, ApprovePlanCommand body, IMediator mediator, CancellationToken ct) =>
            {
                var cmd     = body with { ReqNum = reqNum };
                var updated = await mediator.Send(cmd, ct);
                return updated ? Results.NoContent() : Results.NotFound();
            });

        group.MapPost("/bhr-plans", async (CreateBhrPlanCommand cmd, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(cmd, ct);
            return Results.Created($"/api/v2/development/bhr-plans/{result.ReqNum}", result);
        });

        group.MapGet("/competency-indicators",
            async (long? compNum, string? band, IMediator mediator, CancellationToken ct) =>
                Results.Ok(await mediator.Send(new GetCompetencyIndicatorsQuery(compNum, band), ct)));

        return app;
    }
}
