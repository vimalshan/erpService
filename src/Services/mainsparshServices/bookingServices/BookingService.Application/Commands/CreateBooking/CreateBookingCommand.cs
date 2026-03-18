using BookingService.Application.DTOs;
using MediatR;

namespace BookingService.Application.Commands.CreateBooking;

public record CreateBookingCommand(
    string BookingAppNo,
    string BookingTitle,
    string? LocationCode,
    DateTime? BookingDate,
    long CreatedBy) : IRequest<BookingDto>;
