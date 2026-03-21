using MediatR;
using Microsoft.AspNetCore.Mvc;
using BookingService.Application.Commands;
using BookingService.Application.DTOs;
using BookingService.Application.Queries;
using BookingService.Infrastructure.Persistence;

namespace BookingService.API.Endpoints;

public static class BookingEndpoints
{
    public static void MapBookingEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/bookings")
            .WithTags("Bookings (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllBookingsQuery(), ct);
            return Results.Ok(result);
        }).WithName("GetAllBookingsV2");

        group.MapGet("/{id}", async (string id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetBookingByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetBookingByIdV2");

        group.MapGet("/employee/{employeeSysId}", async (string employeeSysId, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetBookingsByEmployeeQuery(employeeSysId), ct);
            return Results.Ok(result);
        }).WithName("GetBookingsByEmployeeV2");

        group.MapPost("/", async ([FromBody] CreateBookingCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/bookings/{result.BookMainId}", result);
        }).WithName("CreateBookingV2");

        group.MapPost("/{id}/approve", async (string id, [FromQuery] string approvedBy, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(new ApproveBookingCommand(id, approvedBy), ct);
            return Results.Ok(new { message = "Booking approved" });
        }).WithName("ApproveBookingV2");

        group.MapPost("/{id}/cancel", async (string id, [FromQuery] string reason, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(new CancelBookingCommand(id, reason), ct);
            return Results.Ok(new { message = "Booking cancelled" });
        }).WithName("CancelBookingV2");

        // Dapper-based read endpoint for high-perf summary queries
        group.MapGet("/summary", async (DapperBookingQuery dapperQuery, CancellationToken ct) =>
        {
            var result = await dapperQuery.GetBookingsSummaryAsync(ct);
            return Results.Ok(result);
        }).WithName("GetBookingsSummaryV2");
    }
}
