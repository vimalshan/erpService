using BookingService.Application.Commands.CreateBooking;
using BookingService.Application.Commands.ConfirmBooking;
using BookingService.Application.Commands.CancelBooking;
using BookingService.Application.DTOs;
using MediatR;
using HotChocolate;
using Microsoft.AspNetCore.Authorization;

namespace BookingService.API.GraphQL.Mutations;

public class BookingMutation
{
    [Authorize]
    public async Task<long> CreateBooking(
        [Service] IMediator mediator,
        CreateBookingRequestDto input,
        CancellationToken ct)
        => await mediator.Send(new CreateBookingCommand(input), ct);

    [Authorize(Roles = "Admin,TravelDesk")]
    public async Task<long> ConfirmBooking(
        [Service] IMediator mediator,
        long bookingNumber,
        long modeOfTravel,
        long? vendorCode,
        string? ticketNumber,
        string? adminRemarks,
        CancellationToken ct)
        => await mediator.Send(new ConfirmBookingCommand(bookingNumber, modeOfTravel, vendorCode, ticketNumber, adminRemarks), ct);

    [Authorize]
    public async Task<bool> CancelBooking(
        [Service] IMediator mediator,
        long bookingNumber,
        string remarks,
        string cancelledBy,
        CancellationToken ct)
    {
        await mediator.Send(new CancelBookingCommand(bookingNumber, remarks, cancelledBy), ct);
        return true;
    }
}
