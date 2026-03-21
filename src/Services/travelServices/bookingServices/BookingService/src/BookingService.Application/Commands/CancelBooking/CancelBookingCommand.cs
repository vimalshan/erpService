using MediatR;
using BookingService.Domain.Interfaces;
using FluentValidation;

namespace BookingService.Application.Commands.CancelBooking;

// ── Command ──────────────────────────────────────────────────────────────────
public record CancelBookingCommand(
    long BookingNumber,
    string CancellationRemarks,
    string CancelledBy) : IRequest;

// ── Validator ────────────────────────────────────────────────────────────────
public class CancelBookingCommandValidator : AbstractValidator<CancelBookingCommand>
{
    public CancelBookingCommandValidator()
    {
        RuleFor(x => x.BookingNumber).GreaterThan(0);
        RuleFor(x => x.CancellationRemarks).NotEmpty().MaximumLength(200);
        RuleFor(x => x.CancelledBy).NotEmpty().MaximumLength(25);
    }
}

// ── Handler ──────────────────────────────────────────────────────────────────
public class CancelBookingCommandHandler : IRequestHandler<CancelBookingCommand>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _uow;

    public CancelBookingCommandHandler(IBookingRepository bookingRepository, IUnitOfWork uow)
    {
        _bookingRepository = bookingRepository;
        _uow = uow;
    }

    public async Task Handle(CancelBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(request.BookingNumber, cancellationToken)
            ?? throw new KeyNotFoundException($"Booking {request.BookingNumber} not found.");

        if (!booking.CanBeCancelled)
            throw new InvalidOperationException("Booking cannot be cancelled in its current state.");

        booking.Cancel(request.CancellationRemarks, request.CancelledBy);
        await _bookingRepository.UpdateAsync(booking, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
    }
}
