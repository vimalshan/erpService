using ClubMembershipService.Domain.Common;
using ClubMembershipService.Domain.Events;
using ClubMembershipService.Domain.ValueObjects;

namespace ClubMembershipService.Domain.Entities;

public class ClubMaster : AggregateRoot
{
    public long ClubId { get; private set; }
    public string ClubName { get; private set; } = string.Empty;
    public ClubStatus Status { get; private set; } = ClubStatus.Active;
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public long? ModifiedBy { get; private set; }
    public DateTime? ModifiedOn { get; private set; }

    private readonly List<ClubMembership> _memberships = new();
    private readonly List<ClubActivity> _activities = new();

    public IReadOnlyCollection<ClubMembership> Memberships => _memberships.AsReadOnly();
    public IReadOnlyCollection<ClubActivity> Activities => _activities.AsReadOnly();

    private ClubMaster() { }

    public static ClubMaster Create(string clubName, long createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(clubName);
        if (clubName.Length > 100) throw new ArgumentException("Club name cannot exceed 100 characters.");

        var club = new ClubMaster
        {
            ClubName = clubName.Trim(),
            Status = ClubStatus.Active,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };

        club.RaiseDomainEvent(new ClubCreatedEvent(club.ClubId, club.ClubName));
        return club;
    }

    public void UpdateName(string newName, long modifiedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newName);
        ClubName = newName.Trim();
        ModifiedBy = modifiedBy;
        ModifiedOn = DateTime.UtcNow;
    }

    public void Deactivate(long modifiedBy)
    {
        Status = ClubStatus.Inactive;
        ModifiedBy = modifiedBy;
        ModifiedOn = DateTime.UtcNow;
    }

    public void Activate(long modifiedBy)
    {
        Status = ClubStatus.Active;
        ModifiedBy = modifiedBy;
        ModifiedOn = DateTime.UtcNow;
    }
}
