using BookingService.Application.Commands.AddAttendee;
using BookingService.Application.Commands.CreateBooking;
using BookingService.Application.Common;
using BookingService.Application.DTOs;
using BookingService.Application.Queries.GetAllBookings;
using BookingService.Application.Queries.GetBookingById;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace BookingService.API.MinimalApis;

public static class BookingEndpoints
{
    public static IEndpointRouteBuilder MapBookingEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/bookings")
            .WithTags("Bookings (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/", async (ISender sender, int page = 1, int pageSize = 20, string? status = null,
            CancellationToken ct = default) =>
        {
            var result = await sender.Send(new GetAllBookingsQuery(page, pageSize, status), ct);
            return Results.Ok(result);
        })
        .WithName("GetAllBookingsMinimal")
        .Produces<PagedResponse<BookingDto>>();

        group.MapGet("/{id:long}", async (long id, ISender sender, CancellationToken ct) =>
        {
            try
            {
                var result = await sender.Send(new GetBookingByIdQuery(id), ct);
                return Results.Ok(result);
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                return Results.NotFound();
            }
        })
        .WithName("GetBookingByIdMinimal")
        .Produces<BookingDetailDto>()
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", async (CreateBookingCommand cmd, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(cmd, ct);
            return Results.Created($"/api/v2/bookings/{result.BookingId}", result);
        })
        .WithName("CreateBookingMinimal")
        .Produces<BookingDto>(StatusCodes.Status201Created);

        return app;
    }
}
