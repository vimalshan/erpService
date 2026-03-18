using MeetingModule.Domain.Common;
using MeetingModule.Domain.Events;
using MeetingModule.Domain.ValueObjects;

namespace MeetingModule.Domain.Entities;

public class MeetingSchedule : BaseEntity
{
    public long MeetingId { get; private set; }
    public long MeetTypeId { get; private set; }
    public string MeetingTitle { get; private set; } = null!;
    public DateTime MeetingDate { get; private set; }
    public string? MeetingLocation { get; private set; }
    public int? MeetingDuration { get; private set; }
    public long OrganizerId { get; private set; }
    public string MeetingStatus { get; private set; } = "SCHEDULED";
    public string? Notes { get; private set; }

    // Navigation
    public MeetingType MeetingType { get; private set; } = null!;
    public ICollection<PollDetail> Polls { get; private set; } = [];

    private MeetingSchedule() { }

    public static MeetingSchedule Create(
        long meetTypeId, string title, DateTime meetingDate,
        string? location, int? duration, long organizerId,
        string? notes, long createdBy)
    {
        var meeting = new MeetingSchedule
        {
            MeetTypeId = meetTypeId,
            MeetingTitle = title,
            MeetingDate = meetingDate,
            MeetingLocation = location,
            MeetingDuration = duration,
            OrganizerId = organizerId,
            MeetingStatus = ValueObjects.MeetingStatus.Scheduled.Value,
            Notes = notes,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };

        meeting.AddDomainEvent(new MeetingCreatedEvent(meeting));
        return meeting;
    }

    public void Update(string title, DateTime meetingDate, string? location,
        int? duration, string? notes, long updatedBy)
    {
        MeetingTitle = title;
        MeetingDate = meetingDate;
        MeetingLocation = location;
        MeetingDuration = duration;
        Notes = notes;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }

    public void Start(long updatedBy)
    {
        if (MeetingStatus != ValueObjects.MeetingStatus.Scheduled.Value)
            throw new InvalidOperationException("Only scheduled meetings can be started.");

        MeetingStatus = ValueObjects.MeetingStatus.Ongoing.Value;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;

        AddDomainEvent(new MeetingStatusChangedEvent(this, ValueObjects.MeetingStatus.Ongoing.Value));
    }

    public void Complete(long updatedBy)
    {
        if (MeetingStatus != ValueObjects.MeetingStatus.Ongoing.Value)
            throw new InvalidOperationException("Only ongoing meetings can be completed.");

        MeetingStatus = ValueObjects.MeetingStatus.Completed.Value;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;

        AddDomainEvent(new MeetingStatusChangedEvent(this, ValueObjects.MeetingStatus.Completed.Value));
    }

    public void Cancel(long updatedBy)
    {
        if (MeetingStatus == ValueObjects.MeetingStatus.Completed.Value)
            throw new InvalidOperationException("Completed meetings cannot be cancelled.");

        MeetingStatus = ValueObjects.MeetingStatus.Cancelled.Value;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;

        AddDomainEvent(new MeetingStatusChangedEvent(this, ValueObjects.MeetingStatus.Cancelled.Value));
    }
}
