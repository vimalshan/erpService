using MediatR;
using BookingService.Domain.Interfaces;
using FluentValidation;

namespace BookingService.Application.Commands.ConfirmBooking;

// ── Command ──────────────────────────────────────────────────────────────────
public record ConfirmBookingCommand(
    long BookingNumber,
    long ModeOfTravel,
    long? VendorCode,
    string? TicketNumber,
    string? AdminRemarks) : IRequest<long>;

// ── Validator ────────────────────────────────────────────────────────────────
public class ConfirmBookingCommandValidator : AbstractValidator<ConfirmBookingCommand>
{
    public ConfirmBookingCommandValidator()
    {
        RuleFor(x => x.BookingNumber).GreaterThan(0);
        RuleFor(x => x.ModeOfTravel).GreaterThan(0);
        RuleFor(x => x.AdminRemarks).MaximumLength(2000).When(x => x.AdminRemarks != null);
    }
}

// ── Handler ──────────────────────────────────────────────────────────────────
public class ConfirmBookingCommandHandler : IRequestHandler<ConfirmBookingCommand, long>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IBookingConfirmationRepository _confirmationRepository;
    private readonly IUnitOfWork _uow;

    public ConfirmBookingCommandHandler(
        IBookingRepository bookingRepository,
        IBookingConfirmationRepository confirmationRepository,
        IUnitOfWork uow)
    {
        _bookingRepository = bookingRepository;
        _confirmationRepository = confirmationRepository;
        _uow = uow;
    }

    public async Task<long> Handle(ConfirmBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(request.BookingNumber, cancellationToken)
            ?? throw new KeyNotFoundException($"Booking {request.BookingNumber} not found.");

        if (!booking.CanBeConfirmed)
            throw new InvalidOperationException("Booking cannot be confirmed in its current state.");

        // BkCnfNum is an IDENTITY column – let SQL Server generate it
        var confirmation = new Domain.Entities.BookingConfirmation
        {
            BkCnfSrl = 1,
            BkBokNum = booking.Id,
            BkSrlNum = 1,
            BkModCod = request.ModeOfTravel,
            BkFroCit = booking.FromCity,
            BkToCit = booking.ToCity,
            BkFroDat = booking.TravelDates.From,
            BkToDat = booking.TravelDates.To,
            BkVndCod = request.VendorCode,
            BkTckNum = request.TicketNumber,
            BkAdmRmk = request.AdminRemarks,
            BkStsCod = "Y",
            BkReqDat = DateTime.UtcNow
        };

        // First save: insert confirmation and get the SQL Server-generated identity value
        await _confirmationRepository.AddAsync(confirmation, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        // Now confirmation.BkCnfNum contains the generated identity value
        booking.Confirm(confirmation.BkCnfNum);
        await _bookingRepository.UpdateAsync(booking, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return confirmation.BkCnfNum;
    }
}
