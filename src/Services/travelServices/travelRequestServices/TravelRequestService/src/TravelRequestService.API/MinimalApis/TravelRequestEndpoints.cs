using MediatR;
using TravelRequestService.Application.Commands;
using TravelRequestService.Application.DTOs;
using TravelRequestService.Application.Queries;

namespace TravelRequestService.API.MinimalApis;

public static class TravelRequestEndpoints
{
    public static IEndpointRouteBuilder MapTravelRequestEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/travel-requests")
            .WithTags("TravelRequests-MinimalApi")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllTravelRequestsQuery(), ct);
            return Results.Ok(result);
        })
        .WithName("GetAllTravelRequestsV2")
        .Produces<IReadOnlyList<TravelRequestDto>>();

        group.MapGet("/{planNumber}/{companyCode}", async (long planNumber, string companyCode, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetTravelRequestByIdQuery(planNumber, companyCode), ct);
            return result is not null ? Results.Ok(result) : Results.NotFound();
        })
        .WithName("GetTravelRequestByIdV2")
        .Produces<TravelRequestDto>()
        .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/user/{userNumber}", async (long userNumber, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetTravelRequestsByUserQuery(userNumber), ct);
            return Results.Ok(result);
        })
        .WithName("GetTravelRequestsByUserV2");

        group.MapPost("/", async (CreateTravelRequestCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/travel-requests/{result.PlanNumber}/{result.CompanyCode}", result);
        })
        .WithName("CreateTravelRequestV2")
        .Produces<TravelRequestDto>(StatusCodes.Status201Created);

        group.MapPut("/{planNumber}/approve", async (long planNumber, ApproveTravelRequestCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var updatedCommand = command with { PlanNumber = planNumber };
            var result = await mediator.Send(updatedCommand, ct);
            return Results.Ok(new { Success = result });
        })
        .WithName("ApproveTravelRequestV2");

        group.MapPut("/{planNumber}/reject", async (long planNumber, RejectTravelRequestCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var updatedCommand = command with { PlanNumber = planNumber };
            var result = await mediator.Send(updatedCommand, ct);
            return Results.Ok(new { Success = result });
        })
        .WithName("RejectTravelRequestV2");

        group.MapGet("/dashboard", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetDashTourPlanQuery(), ct);
            return Results.Ok(result);
        })
        .WithName("GetDashTourPlanV2");

        return app;
    }
}
