using MediatR;
using BookingService.Application.DTOs;
using BookingService.Domain.Interfaces;
using FluentValidation;

namespace BookingService.Application.Queries.GetBookingDetails;

// ── Query ─────────────────────────────────────────────────────────────────────
public record GetBookingDetailsQuery(long BookingNumber) : IRequest<BookingRequestDto?>;

// ── Validator ─────────────────────────────────────────────────────────────────
public class GetBookingDetailsQueryValidator : AbstractValidator<GetBookingDetailsQuery>
{
    public GetBookingDetailsQueryValidator()
    {
        RuleFor(x => x.BookingNumber).GreaterThan(0);
    }
}

// ── Handler ──────────────────────────────────────────────────────────────────
public class GetBookingDetailsQueryHandler : IRequestHandler<GetBookingDetailsQuery, BookingRequestDto?>
{
    private readonly IBookingRepository _bookingRepository;

    public GetBookingDetailsQueryHandler(IBookingRepository bookingRepository)
        => _bookingRepository = bookingRepository;

    public async Task<BookingRequestDto?> Handle(GetBookingDetailsQuery request, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(request.BookingNumber, cancellationToken);
        if (booking is null) return null;

        return new BookingRequestDto(
            booking.Id,
            booking.UserCode,
            booking.UserNum,
            booking.BookingType.ToString()[0].ToString(),
            booking.TravelDates.From,
            booking.TravelDates.To,
            booking.FromCity,
            booking.ToCity,
            booking.FromLocation,
            booking.ToLocation,
            booking.PersonName.FullName,
            booking.BudgetAmount.Amount,
            booking.Status.ToString()[0].ToString(),
            booking.ConfirmationNumber,
            booking.CancelledOn,
            booking.CancellationRemarks);
    }
}
