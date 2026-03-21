using BookingService.Domain.Common;
using BookingService.Domain.Events;

namespace BookingService.Domain.Aggregates;

/// <summary>
/// Coupon Aggregate Root – manages coupon lifecycle including sub-tickets.
/// </summary>
public sealed class CouponAggregate : AggregateRoot<long>
{
    public string? RefId { get; private set; }
    public long? RequestId { get; private set; }
    public int NumberOfTickets { get; private set; }
    public string? AirlineName { get; private set; }
    public long CouponStart { get; private set; }
    public long CouponEnd { get; private set; }
    public DateTime ValidFrom { get; private set; }
    public DateTime ValidTo { get; private set; }
    public long CouponCost { get; private set; }
    public string? IssuanceRemarks { get; private set; }
    public char UsageFlag { get; private set; } = 'A';
    public string? UserId { get; private set; }
    public string? AdminUser { get; private set; }
    public string? AdminUnit { get; private set; }
    public DateTime? IssueDate { get; private set; }

    private readonly List<CouponSubTicket> _tickets = new();
    public IReadOnlyCollection<CouponSubTicket> Tickets => _tickets.AsReadOnly();

    private CouponAggregate() { }

    public static CouponAggregate Create(
        long couponId,
        string? refId,
        long? requestId,
        int numberOfTickets,
        string? airlineName,
        long couponStart,
        long couponEnd,
        DateTime validFrom,
        DateTime validTo,
        long couponCost,
        string? issuanceRemarks,
        string? adminUser,
        string? adminUnit)
    {
        if (validTo < validFrom)
            throw new ArgumentException("Valid-to must be after valid-from.");

        var coupon = new CouponAggregate
        {
            Id = couponId,
            RefId = refId,
            RequestId = requestId,
            NumberOfTickets = numberOfTickets,
            AirlineName = airlineName,
            CouponStart = couponStart,
            CouponEnd = couponEnd,
            ValidFrom = validFrom,
            ValidTo = validTo,
            CouponCost = couponCost,
            IssuanceRemarks = issuanceRemarks,
            AdminUser = adminUser,
            AdminUnit = adminUnit,
            IssueDate = DateTime.UtcNow
        };
        coupon.RaiseDomainEvent(new CouponIssuedEvent(couponId, numberOfTickets, airlineName));
        return coupon;
    }

    public void AddTicket(long serialNumber, string ticketNumber)
    {
        if (_tickets.Any(t => t.TicketNumber == ticketNumber))
            throw new InvalidOperationException("Ticket already added.");
        _tickets.Add(new CouponSubTicket(Id, serialNumber, ticketNumber));
    }

    public void AssignToUser(string userId)
    {
        UserId = userId;
        UsageFlag = 'U';
    }
}

public record CouponSubTicket(long CouponId, long SerialNumber, string TicketNumber, char UsageFlag = 'A');
