using BookingService.Domain.Common;
using BookingService.Domain.Events;
using BookingService.Domain.Exceptions;
using BookingService.Domain.ValueObjects;

namespace BookingService.Domain.Entities;

/// <summary>
/// Aggregate root for the Booking bounded context.
/// Maps to BOOK_MAIN with child collections BOOK_REC and BOOK_ATTENDEES.
/// </summary>
public class BookMain : BaseEntity
{
    public string BookingAppNo { get; private set; } = string.Empty;
    public string BookingTitle { get; private set; } = string.Empty;
    public string? LocationCode { get; private set; }
    public DateTime? BookingDate { get; private set; }
    public BookingStatus Status { get; private set; } = BookingStatus.Draft;

    private readonly List<BookRecord> _records = [];
    public IReadOnlyCollection<BookRecord> Records => _records.AsReadOnly();

    private readonly List<BookAttendee> _attendees = [];
    public IReadOnlyCollection<BookAttendee> Attendees => _attendees.AsReadOnly();

    private BookMain() { }

    public static BookMain Create(string appNo, string title, string? locationCode, DateTime? bookingDate, long createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(appNo);
        ArgumentException.ThrowIfNullOrWhiteSpace(title);

        var booking = new BookMain
        {
            BookingAppNo = appNo,
            BookingTitle = title,
            LocationCode = locationCode,
            BookingDate = bookingDate,
            Status = BookingStatus.Draft,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };

        booking.AddDomainEvent(new BookingCreatedDomainEvent(booking.BookingAppNo, createdBy));
        return booking;
    }

    public void Update(string title, string? locationCode, DateTime? bookingDate, long updatedBy)
    {
        if (Status != BookingStatus.Draft)
            throw new BookingDomainException("Only DRAFT bookings can be updated.");

        BookingTitle = title;
        LocationCode = locationCode;
        BookingDate = bookingDate;
        SetUpdatedAudit(updatedBy);
    }

    public void Submit(long updatedBy) => TransitionStatus(BookingStatus.Submitted, updatedBy);
    public void Approve(long updatedBy) => TransitionStatus(BookingStatus.Approved, updatedBy);
    public void Reject(long updatedBy) => TransitionStatus(BookingStatus.Rejected, updatedBy);
    public void Cancel(long updatedBy) => TransitionStatus(BookingStatus.Cancelled, updatedBy);

    private void TransitionStatus(BookingStatus next, long updatedBy)
    {
        if (!Status.CanTransitionTo(next))
            throw new BookingDomainException($"Cannot transition from {Status} to {next}.");

        var previous = Status;
        Status = next;
        SetUpdatedAudit(updatedBy);
        AddDomainEvent(new BookingStatusChangedDomainEvent(Id, BookingAppNo, previous.Value, next.Value, updatedBy));
    }

    public BookRecord AddRecord(string locationCode, string? recDetails, long createdBy)
    {
        var record = BookRecord.Create(Id, locationCode, recDetails, createdBy);
        _records.Add(record);
        return record;
    }

    public BookAttendee AddAttendee(long attendeeSysId, long createdBy)
    {
        if (_attendees.Any(a => a.AttendeeSysId == attendeeSysId))
            throw new BookingDomainException($"Attendee {attendeeSysId} is already registered.");

        var serial = _attendees.Count + 1;
        var attendee = BookAttendee.Create(Id, attendeeSysId, serial, createdBy);
        _attendees.Add(attendee);
        AddDomainEvent(new AttendeeRegisteredDomainEvent(Id, attendeeSysId, serial));
        return attendee;
    }

    public void RemoveAttendee(long attendeeSysId, long updatedBy)
    {
        var attendee = _attendees.FirstOrDefault(a => a.AttendeeSysId == attendeeSysId)
            ?? throw new BookingDomainException($"Attendee {attendeeSysId} not found.");
        attendee.Cancel(updatedBy);
    }
}
