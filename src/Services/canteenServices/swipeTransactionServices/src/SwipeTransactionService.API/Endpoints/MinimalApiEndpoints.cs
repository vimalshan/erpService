using SwipeTransactionService.Application.DTOs;
using SwipeTransactionService.Application.Features.SwipeTransactions.Commands;
using SwipeTransactionService.Application.Features.SwipeTransactions.Queries;
using SwipeTransactionService.Application.Features.CanteenPunch.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace SwipeTransactionService.API.Endpoints;

public static class MinimalApiEndpoints
{
    public static WebApplication MapSwipeEndpoints(this WebApplication app)
    {
        var swipes = app.MapGroup("/api/v2/swipes")
            .WithTags("Swipes v2")
            .RequireAuthorization();

        swipes.MapPost("/", async (RecordSwipeUploadCommand cmd, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(cmd, ct);
            return Results.Created($"/api/v2/swipes/{result.SerialNumber}", result);
        })
        .WithName("CreateSwipe")
        .WithSummary("Record a new swipe upload");

        swipes.MapGet("/{employeeNumber}/range", async (
            string employeeNumber,
            DateTime from,
            DateTime to,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetSwipesByEmployeeQuery(employeeNumber, from, to), ct);
            return Results.Ok(result);
        })
        .WithName("GetSwipesByEmployee")
        .WithSummary("Get swipes by employee and date range");

        swipes.MapGet("/pending", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetPendingSwipesQuery(), ct);
            return Results.Ok(result);
        })
        .WithName("GetPendingSwipes")
        .WithSummary("Get all pending swipe transactions");

        var punches = app.MapGroup("/api/v2/punches")
            .WithTags("Punches v2")
            .RequireAuthorization();

        punches.MapPost("/", async (RecordPunchCommand cmd, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(cmd, ct);
            return Results.Ok(result);
        })
        .WithName("RecordPunch")
        .WithSummary("Record a canteen check-in/check-out");

        return app;
    }
}
