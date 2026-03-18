namespace CommunityService.Domain.Entities;

using Interfaces;
using ValueObjects;

public class CommunityMember : IEntity
{
    public long MemberId { get; private set; }
    public long CommunityId { get; private set; }
    public long UserSysId { get; private set; }
    public MemberRole MemberRole { get; private set; } = null!;
    public DateTime JoinDate { get; private set; }
    public DateTime? LeaveDate { get; private set; }
    public MemberStatus MemberStatus { get; private set; } = null!;
    public int ContributionCount { get; private set; }
    public AuditInfo AuditInfo { get; private set; } = null!;

    long IEntity.Id => MemberId;

    private CommunityMember() { }

    public static CommunityMember Create(long communityId, long userSysId, string role, long createdBy)
    {
        return new CommunityMember
        {
            CommunityId = communityId,
            UserSysId = userSysId,
            MemberRole = MemberRole.Create(role),
            JoinDate = DateTime.UtcNow,
            MemberStatus = MemberStatus.Create("ACTIVE"),
            ContributionCount = 0,
            AuditInfo = new AuditInfo
            {
                CreatedBy = createdBy,
                CreatedOn = DateTime.UtcNow
            }
        };
    }

    public void ChangeRole(string newRole, long updatedBy)
    {
        MemberRole = MemberRole.Create(newRole);
        AuditInfo.UpdatedBy = updatedBy;
        AuditInfo.UpdatedOn = DateTime.UtcNow;
    }

    public void Remove(long updatedBy)
    {
        MemberStatus = MemberStatus.Create("REMOVED");
        LeaveDate = DateTime.UtcNow;
        AuditInfo.UpdatedBy = updatedBy;
        AuditInfo.UpdatedOn = DateTime.UtcNow;
    }

    public void IncrementContribution()
    {
        ContributionCount++;
    }
}
