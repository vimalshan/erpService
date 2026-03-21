using BookingService.Application.DTOs;
using BookingService.Application.Queries;
using BookingService.Application.Commands;
using MediatR;

namespace BookingService.API.GraphQL;

public class BookingQuery
{
    public async Task<IReadOnlyList<BookRequestMainDto>> GetBookings(
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllBookingsQuery(), ct);

    public async Task<BookRequestMainDto?> GetBookingById(
        string id, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetBookingByIdQuery(id), ct);

    public async Task<IReadOnlyList<BookRequestMainDto>> GetBookingsByEmployee(
        string employeeSysId, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetBookingsByEmployeeQuery(employeeSysId), ct);

    public async Task<IReadOnlyList<BookConfirmationDto>> GetConfirmations(
        string bookingId, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetBookingConfirmationsQuery(bookingId), ct);
}

public class BookingMutation
{
    public async Task<BookRequestMainDto> CreateBooking(
        CreateBookingCommand input, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<BookRequestMainDto> UpdateBooking(
        UpdateBookingCommand input, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<bool> DeleteBooking(
        string bookMainId, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new DeleteBookingCommand(bookMainId), ct);

    public async Task<bool> ApproveBooking(
        string bookMainId, string approvedBy, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new ApproveBookingCommand(bookMainId, approvedBy), ct);

    public async Task<BookConfirmationDto> ConfirmBooking(
        ConfirmBookingCommand input, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<bool> CancelBooking(
        string bookMainId, string reason, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new CancelBookingCommand(bookMainId, reason), ct);
}
