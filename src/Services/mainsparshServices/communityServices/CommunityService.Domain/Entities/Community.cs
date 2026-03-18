namespace CommunityService.Domain.Entities;

using Interfaces;
using ValueObjects;
using Events;

public class Community : IAggregateRoot
{
    private readonly List<IDomainEvent> _domainEvents = new();

    public long CommunityId { get; private set; }
    public CommunityCode CommunityCode { get; private set; } = null!;
    public CommunityName CommunityName { get; private set; } = null!;
    public string? CommunityDescription { get; private set; }
    public CommunityType CommunityType { get; private set; } = null!;
    public string? CommunityIcon { get; private set; }
    public string? CommunityBanner { get; private set; }
    public PrivacyLevel PrivacyLevel { get; private set; } = null!;
    public long OwnerId { get; private set; }
    public long? ApproverId { get; private set; }
    public CommunityStatus CommunityStatus { get; private set; } = null!;
    public int MemberCount { get; private set; }
    public AuditInfo AuditInfo { get; private set; } = null!;
    public List<CommunityMember> Members { get; private set; } = new();

    private Community() { }

    public static Community Create(
        string code,
        string name,
        string? description,
        string type,
        string? icon,
        string? banner,
        string privacyLevel,
        long ownerId,
        long createdBy)
    {
        var community = new Community
        {
            CommunityCode = CommunityCode.Create(code),
            CommunityName = CommunityName.Create(name),
            CommunityDescription = description,
            CommunityType = CommunityType.Create(type),
            CommunityIcon = icon,
            CommunityBanner = banner,
            PrivacyLevel = PrivacyLevel.Create(privacyLevel),
            OwnerId = ownerId,
            CommunityStatus = CommunityStatus.Create("ACTIVE"),
            MemberCount = 0,
            AuditInfo = new AuditInfo
            {
                CreatedBy = createdBy,
                CreatedOn = DateTime.UtcNow
            }
        };

        community._domainEvents.Add(new CommunityCreatedEvent(
            community.CommunityId,
            community.CommunityCode.Value,
            community.CommunityName.Value,
            ownerId));

        return community;
    }

    public void Update(string name, string? description, string privacyLevel, long updatedBy)
    {
        CommunityName = CommunityName.Create(name);
        CommunityDescription = description;
        PrivacyLevel = PrivacyLevel.Create(privacyLevel);
        AuditInfo.UpdatedBy = updatedBy;
        AuditInfo.UpdatedOn = DateTime.UtcNow;

        _domainEvents.Add(new CommunityUpdatedEvent(CommunityId, name));
    }

    public void AddMember(long userId, string role, long createdBy)
    {
        if (Members.Any(m => m.UserSysId == userId && m.MemberStatus.Value != "REMOVED"))
            throw new InvalidOperationException("User is already a member of this community.");

        var member = CommunityMember.Create(CommunityId, userId, role, createdBy);
        Members.Add(member);
        MemberCount++;

        _domainEvents.Add(new MemberAddedEvent(CommunityId, userId, role));
    }

    public void RemoveMember(long userId, long updatedBy)
    {
        var member = Members.FirstOrDefault(m => m.UserSysId == userId && m.MemberStatus.Value != "REMOVED");
        if (member == null)
            throw new InvalidOperationException("Member not found.");

        member.Remove(updatedBy);
        MemberCount--;

        _domainEvents.Add(new MemberRemovedEvent(CommunityId, userId));
    }

    public void ChangeMemberRole(long userId, string newRole, long updatedBy)
    {
        var member = Members.FirstOrDefault(m => m.UserSysId == userId && m.MemberStatus.Value == "ACTIVE");
        if (member == null)
            throw new InvalidOperationException("Active member not found.");

        member.ChangeRole(newRole, updatedBy);

        _domainEvents.Add(new MemberRoleChangedEvent(CommunityId, userId, newRole));
    }

    public void Archive(long updatedBy)
    {
        CommunityStatus = CommunityStatus.Create("ARCHIVED");
        AuditInfo.UpdatedBy = updatedBy;
        AuditInfo.UpdatedOn = DateTime.UtcNow;

        _domainEvents.Add(new CommunityDeletedEvent(CommunityId));
    }

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.AsReadOnly();

    public void ClearDomainEvents() => _domainEvents.Clear();
}
