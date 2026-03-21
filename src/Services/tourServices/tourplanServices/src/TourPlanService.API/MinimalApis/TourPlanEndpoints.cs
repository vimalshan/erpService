using MediatR;
using TourPlanService.Application.Commands.CreateTourPlan;
using TourPlanService.Application.Queries.GetTourPlanById;
using TourPlanService.Application.Queries.GetTourPlanList;

namespace TourPlanService.API.MinimalApis;

public static class TourPlanEndpoints
{
    public static void MapTourPlanEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/minimal/tourplans")
            .WithTags("TourPlan Minimal API")
            .RequireAuthorization();

        group.MapGet("/", async (
            IMediator mediator,
            int page = 1, int pageSize = 20,
            string? employeeId = null, string? status = null,
            CancellationToken cancellationToken = default) =>
        {
            var result = await mediator.Send(
                new GetTourPlanListQuery(page, pageSize, employeeId, status), cancellationToken);
            return Results.Ok(result);
        })
        .WithName("MinimalGetTourPlans")
        .WithSummary("Get paginated list of tour plans")
        .Produces(200);

        group.MapGet("/{tpId}", async (
            string tpId, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new GetTourPlanByIdQuery(tpId), cancellationToken);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("MinimalGetTourPlanById")
        .WithSummary("Get tour plan by ID")
        .Produces(200)
        .Produces(404);

        group.MapPost("/", async (
            CreateTourPlanCommand command, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(command, cancellationToken);
            return result.IsSuccess
                ? Results.Created($"/api/minimal/tourplans/{result.Value}", result.Value)
                : Results.BadRequest(result.Error);
        })
        .WithName("MinimalCreateTourPlan")
        .WithSummary("Create a new tour plan")
        .Produces(201)
        .Produces(400);
    }
}
