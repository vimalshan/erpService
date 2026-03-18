using BookingService.Application.Commands.ApproveBooking;
using BookingService.Application.Commands.CancelBooking;
using BookingService.Application.Commands.RejectBooking;
using BookingService.Application.Commands.SubmitBooking;
using BookingService.Domain.Interfaces;
using MediatR;

namespace BookingService.Application.Commands;

public class SubmitBookingCommandHandler(IUnitOfWork uow) : IRequestHandler<SubmitBookingCommand>
{
    public async Task Handle(SubmitBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await uow.Bookings.GetByIdAsync(request.BookingId, cancellationToken)
            ?? throw new KeyNotFoundException($"Booking {request.BookingId} not found.");
        booking.Submit(request.UpdatedBy);
        await uow.SaveChangesAsync(cancellationToken);
    }
}

public class ApproveBookingCommandHandler(IUnitOfWork uow) : IRequestHandler<ApproveBookingCommand>
{
    public async Task Handle(ApproveBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await uow.Bookings.GetByIdAsync(request.BookingId, cancellationToken)
            ?? throw new KeyNotFoundException($"Booking {request.BookingId} not found.");
        booking.Approve(request.UpdatedBy);
        await uow.SaveChangesAsync(cancellationToken);
    }
}

public class RejectBookingCommandHandler(IUnitOfWork uow) : IRequestHandler<RejectBookingCommand>
{
    public async Task Handle(RejectBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await uow.Bookings.GetByIdAsync(request.BookingId, cancellationToken)
            ?? throw new KeyNotFoundException($"Booking {request.BookingId} not found.");
        booking.Reject(request.UpdatedBy);
        await uow.SaveChangesAsync(cancellationToken);
    }
}

public class CancelBookingCommandHandler(IUnitOfWork uow) : IRequestHandler<CancelBookingCommand>
{
    public async Task Handle(CancelBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await uow.Bookings.GetByIdAsync(request.BookingId, cancellationToken)
            ?? throw new KeyNotFoundException($"Booking {request.BookingId} not found.");
        booking.Cancel(request.UpdatedBy);
        await uow.SaveChangesAsync(cancellationToken);
    }
}
