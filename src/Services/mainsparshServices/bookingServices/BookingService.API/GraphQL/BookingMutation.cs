using BookingService.Application.Commands.CreateBooking;
using BookingService.Application.Commands.UpdateBooking;
using BookingService.Application.DTOs;
using MediatR;

namespace BookingService.API.GraphQL;

[ExtendObjectType("Mutation")]
public class BookingMutation
{
    public async Task<BookingDto> CreateBookingAsync(
        string bookingAppNo,
        string bookingTitle,
        string? locationCode,
        DateTime? bookingDate,
        long createdBy,
        ISender sender,
        CancellationToken cancellationToken)
    {
        return await sender.Send(
            new CreateBookingCommand(bookingAppNo, bookingTitle, locationCode, bookingDate, createdBy),
            cancellationToken);
    }

    public async Task<BookingDto> UpdateBookingAsync(
        long bookingId,
        string bookingTitle,
        string? locationCode,
        DateTime? bookingDate,
        long updatedBy,
        ISender sender,
        CancellationToken cancellationToken)
    {
        return await sender.Send(
            new UpdateBookingCommand(bookingId, bookingTitle, locationCode, bookingDate, updatedBy),
            cancellationToken);
    }
}
