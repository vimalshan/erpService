using BookingService.Domain.Common;
using BookingService.Domain.ValueObjects;

namespace BookingService.Domain.Entities;

public class BookAttendee : BaseEntity
{
    public long BookingId { get; private set; }
    public long AttendeeSysId { get; private set; }
    public int AttendeeSerial { get; private set; }
    public AttendanceStatus AttendanceStatus { get; private set; } = AttendanceStatus.Registered;

    private BookAttendee() { }

    public static BookAttendee Create(long bookingId, long attendeeSysId, int serial, long createdBy)
    {
        return new BookAttendee
        {
            BookingId = bookingId,
            AttendeeSysId = attendeeSysId,
            AttendeeSerial = serial,
            AttendanceStatus = AttendanceStatus.Registered,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };
    }

    public void MarkAttended(long updatedBy)
    {
        AttendanceStatus = AttendanceStatus.Attended;
        SetUpdatedAudit(updatedBy);
    }

    public void Cancel(long updatedBy)
    {
        AttendanceStatus = AttendanceStatus.Cancelled;
        SetUpdatedAudit(updatedBy);
    }
}
