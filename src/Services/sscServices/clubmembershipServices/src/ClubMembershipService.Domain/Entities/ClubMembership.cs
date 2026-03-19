using ClubMembershipService.Domain.Common;
using ClubMembershipService.Domain.Events;
using ClubMembershipService.Domain.ValueObjects;

namespace ClubMembershipService.Domain.Entities;

public class ClubMembership : AggregateRoot
{
    public long MembershipId { get; private set; }
    public long ClubId { get; private set; }
    public long MemberId { get; private set; }
    public DateOnly JoinDate { get; private set; }
    public decimal? MembershipFee { get; private set; }
    public MembershipStatus Status { get; private set; } = MembershipStatus.Active;
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public long? ModifiedBy { get; private set; }
    public DateTime? ModifiedOn { get; private set; }

    public ClubMaster? Club { get; private set; }

    private ClubMembership() { }

    public static ClubMembership Create(long clubId, long memberId, DateOnly joinDate,
        decimal? membershipFee, long enrolledBy)
    {
        if (clubId <= 0) throw new ArgumentException("Club ID must be positive.");
        if (memberId <= 0) throw new ArgumentException("Member ID must be positive.");

        var membership = new ClubMembership
        {
            ClubId = clubId,
            MemberId = memberId,
            JoinDate = joinDate,
            MembershipFee = membershipFee,
            Status = MembershipStatus.Active,
            CreatedBy = enrolledBy,
            CreatedOn = DateTime.UtcNow
        };

        membership.RaiseDomainEvent(new MembershipCreatedEvent(
            membership.MembershipId, clubId, memberId));
        return membership;
    }

    public void Deactivate(long modifiedBy)
    {
        Status = MembershipStatus.Inactive;
        ModifiedBy = modifiedBy;
        ModifiedOn = DateTime.UtcNow;
    }
}
