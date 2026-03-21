using BookingService.Infrastructure.DapperRepositories;
using BookingService.Application.Commands.CreateBooking;
using BookingService.Application.Commands.CancelBooking;
using BookingService.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.API.MinimalApis;

public static class BookingMinimalApis
{
    public static WebApplication MapBookingMinimalApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/v2/bookings")
            .WithTags("Bookings v2")
            .RequireAuthorization();

        group.MapGet("/{bookingNumber:long}", async (
            long bookingNumber,
            IBookingReadRepository repo,
            CancellationToken ct) =>
        {
            var result = await repo.GetBookingDetailsAsync(bookingNumber, ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetBookingV2")
        .WithSummary("Get booking details (Minimal API v2)");

        group.MapPost("/", async (
            CreateBookingRequestDto dto,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var bookingNumber = await mediator.Send(new CreateBookingCommand(dto), ct);
            return Results.Created($"/api/v2/bookings/{bookingNumber}", new { bookingNumber });
        })
        .WithName("CreateBookingV2")
        .WithSummary("Create booking (Minimal API v2)");

        group.MapDelete("/{bookingNumber:long}", async (
            long bookingNumber,
            [FromBody] CancelBookingRequestDto dto,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new CancelBookingCommand(bookingNumber, dto.CancellationRemarks, dto.CancelledBy), ct);
            return Results.NoContent();
        })
        .WithName("CancelBookingV2")
        .WithSummary("Cancel booking (Minimal API v2)");

        return app;
    }
}
