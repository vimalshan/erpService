using BookingService.Domain.Interfaces;
using MediatR;

namespace BookingService.Application.Commands.RemoveAttendee;

public record RemoveAttendeeCommand(long BookingId, long AttendeeSysId, long UpdatedBy) : IRequest;

public class RemoveAttendeeCommandHandler(IUnitOfWork uow) : IRequestHandler<RemoveAttendeeCommand>
{
    public async Task Handle(RemoveAttendeeCommand request, CancellationToken cancellationToken)
    {
        var booking = await uow.Bookings.GetByIdAsync(request.BookingId, cancellationToken)
            ?? throw new KeyNotFoundException($"Booking {request.BookingId} not found.");

        booking.RemoveAttendee(request.AttendeeSysId, request.UpdatedBy);
        await uow.SaveChangesAsync(cancellationToken);
    }
}
