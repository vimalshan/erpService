using BookingService.Domain.Common;
using BookingService.Domain.Enums;
using BookingService.Domain.Events;
using BookingService.Domain.ValueObjects;

namespace BookingService.Domain.Aggregates;

/// <summary>
/// Booking Aggregate Root – owns BookingRequest and its associated Confirmation.
/// </summary>
public sealed class BookingAggregate : AggregateRoot<long>
{
    // ── Core request fields ──────────────────────────────────────────────────
    public string UserCode { get; private set; } = default!;
    public long UserNum { get; private set; }
    public BookingType BookingType { get; private set; }
    public DateRange TravelDates { get; private set; } = default!;
    public long FromCity { get; private set; }
    public long ToCity { get; private set; }
    public string FromLocation { get; private set; } = string.Empty;
    public string ToLocation { get; private set; } = string.Empty;
    public PersonName PersonName { get; private set; } = default!;
    public Money BudgetAmount { get; private set; } = Money.Zero;
    public BookingStatus Status { get; private set; }
    public TravelArrangement Arrangement { get; private set; }
    public PersonStatus PersonStatus { get; private set; }
    public DateTime AppliedDate { get; private set; }

    // ── Cancellation ─────────────────────────────────────────────────────────
    public DateTime? CancelledOn { get; private set; }
    public string? CancellationRemarks { get; private set; }
    public string? CancelledBy { get; private set; }

    // ── Confirmation link ─────────────────────────────────────────────────────
    public long? ConfirmationNumber { get; private set; }

    // ── Airline / travel extras ───────────────────────────────────────────────
    public string? AirlineCode { get; private set; }
    public long? TravelClass { get; private set; }

    private BookingAggregate() { }

    public static BookingAggregate Create(
        long bookingNumber,
        string userCode,
        long userNum,
        BookingType bookingType,
        DateRange travelDates,
        long fromCity,
        long toCity,
        string fromLocation,
        string toLocation,
        PersonName personName,
        Money? budgetAmount = null,
        string? airlineCode = null,
        long? travelClass = null)
    {
        var booking = new BookingAggregate
        {
            Id = bookingNumber,
            UserCode = userCode,
            UserNum = userNum,
            BookingType = bookingType,
            TravelDates = travelDates,
            FromCity = fromCity,
            ToCity = toCity,
            FromLocation = fromLocation,
            ToLocation = toLocation,
            PersonName = personName,
            BudgetAmount = budgetAmount ?? Money.Zero,
            Status = BookingStatus.New,
            Arrangement = TravelArrangement.Admin,
            PersonStatus = PersonStatus.Self,
            AppliedDate = DateTime.UtcNow,
            AirlineCode = airlineCode,
            TravelClass = travelClass
        };
        booking.RaiseDomainEvent(new BookingCreatedEvent(bookingNumber, userCode, bookingType, travelDates));
        return booking;
    }

    public void Confirm(long confirmationNumber)
    {
        if (Status == BookingStatus.CancellationRequested)
            throw new InvalidOperationException("Cannot confirm a cancelled booking.");
        ConfirmationNumber = confirmationNumber;
        Status = BookingStatus.Confirmed;
        RaiseDomainEvent(new BookingConfirmedEvent(Id, confirmationNumber));
    }

    public void Cancel(string remarks, string cancelledBy)
    {
        if (Status == BookingStatus.CancellationRequested)
            throw new InvalidOperationException("Booking is already cancelled.");
        Status = BookingStatus.CancellationRequested;
        CancelledOn = DateTime.UtcNow;
        CancellationRemarks = remarks;
        CancelledBy = cancelledBy;
        RaiseDomainEvent(new BookingCancelledEvent(Id, cancelledBy, remarks));
    }

    public bool CanBeConfirmed => Status == BookingStatus.New;
    public bool CanBeCancelled => Status is BookingStatus.New or BookingStatus.Confirmed;
}
