using BookingService.Application.DTOs;
using MediatR;

namespace BookingService.Application.Commands.UpdateBooking;

public record UpdateBookingCommand(
    long BookingId,
    string BookingTitle,
    string? LocationCode,
    DateTime? BookingDate,
    long UpdatedBy) : IRequest<BookingDto>;
