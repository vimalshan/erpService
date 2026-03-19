using ClubMembershipService.Domain.Common;
using ClubMembershipService.Domain.Events;
using ClubMembershipService.Domain.ValueObjects;

namespace ClubMembershipService.Domain.Entities;

public class ClubActivity : AggregateRoot
{
    public long ActivityId { get; private set; }
    public long ClubId { get; private set; }
    public string ActivityName { get; private set; } = string.Empty;
    public DateOnly ActivityDate { get; private set; }
    public decimal? ActivityBudget { get; private set; }
    public long OrganizerId { get; private set; }
    public ActivityStatus Status { get; private set; } = ActivityStatus.Planned;
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public long? ModifiedBy { get; private set; }
    public DateTime? ModifiedOn { get; private set; }

    public ClubMaster? Club { get; private set; }

    private ClubActivity() { }

    public static ClubActivity Create(long clubId, string activityName, DateOnly activityDate,
        decimal? budget, long organizerId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(activityName);
        if (activityName.Length > 100) throw new ArgumentException("Activity name cannot exceed 100 characters.");
        if (clubId <= 0) throw new ArgumentException("Club ID must be positive.");

        var activity = new ClubActivity
        {
            ClubId = clubId,
            ActivityName = activityName.Trim(),
            ActivityDate = activityDate,
            ActivityBudget = budget,
            OrganizerId = organizerId,
            Status = ActivityStatus.Planned,
            CreatedBy = organizerId,
            CreatedOn = DateTime.UtcNow
        };

        activity.RaiseDomainEvent(new ActivityRecordedEvent(
            activity.ActivityId, clubId, activityName));
        return activity;
    }

    public void MarkOngoing(long modifiedBy)
    {
        Status = ActivityStatus.Ongoing;
        ModifiedBy = modifiedBy;
        ModifiedOn = DateTime.UtcNow;
    }

    public void MarkCompleted(long modifiedBy)
    {
        Status = ActivityStatus.Completed;
        ModifiedBy = modifiedBy;
        ModifiedOn = DateTime.UtcNow;
    }
}
