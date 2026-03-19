using MamAllocationService.Application.Commands;
using MamAllocationService.Application.DTOs;
using MamAllocationService.Application.Queries;
using MediatR;

namespace MamAllocationService.Api.MinimalApis;

public static class AllocationMinimalApiEndpoints
{
    public static void MapAllocationMinimalApis(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/minimal/allocations").WithTags("Allocations (Minimal)").RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllAllocationsQuery(), ct)));

        group.MapGet("/{date:datetime}/{rmCode:int}", async (DateTime date, int rmCode, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllocationByIdQuery(date, rmCode), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        group.MapPost("/", async (AllocationDetailDto dto, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new CreateAllocationDetailCommand(dto), ct);
            return Results.Created($"/api/minimal/allocations/{result.AllDate:yyyy-MM-dd}/{result.AllRm}", result);
        });

        group.MapDelete("/{date:datetime}/{rmCode:int}", async (DateTime date, int rmCode, IMediator mediator, CancellationToken ct) =>
        {
            var deleted = await mediator.Send(new DeleteAllocationDetailCommand(date, rmCode), ct);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }

    public static void MapArrivalMinimalApis(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/minimal/arrivals").WithTags("Arrivals (Minimal)").RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllArrivalsQuery(), ct)));

        group.MapPost("/", async (ArrivalDetailDto dto, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new CreateArrivalDetailCommand(dto), ct);
            return Results.Created(string.Empty, result);
        });
    }

    public static void MapConsumptionMinimalApis(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/minimal/consumptions").WithTags("Consumptions (Minimal)").RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllConsumptionsQuery(), ct)));

        group.MapPost("/", async (ConsumptionDetailDto dto, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new CreateConsumptionDetailCommand(dto), ct);
            return Results.Created(string.Empty, result);
        });
    }

    public static void MapDispatchMinimalApis(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/minimal/dispatches").WithTags("Dispatches (Minimal)").RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllDispatchesQuery(), ct)));

        group.MapPost("/", async (DispatchDetailDto dto, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new CreateDispatchDetailCommand(dto), ct);
            return Results.Created(string.Empty, result);
        });
    }
}
