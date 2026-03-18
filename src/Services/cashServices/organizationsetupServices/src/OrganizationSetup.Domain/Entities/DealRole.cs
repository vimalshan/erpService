using OrganizationSetup.Domain.Common;
using OrganizationSetup.Domain.Events;
using OrganizationSetup.Domain.ValueObjects;

namespace OrganizationSetup.Domain.Entities;

/// <summary>Maps to DEAL_ROLE table - Role Master.</summary>
public class DealRole : BaseEntity, IAggregateRoot
{
    public long RoleId { get; private set; }
    public RoleName RoleName { get; private set; } = default!;
    public long RoleLevel { get; private set; }
    public decimal RoleModifiedBy { get; private set; }
    public DateTime RoleModifiedOn { get; private set; }

    // Navigation
    public ICollection<DealUserMap> UserMaps { get; private set; } = [];

    private DealRole() { }

    public static DealRole Create(long roleId, string roleName, long roleLevel, decimal modifiedBy)
    {
        var role = new DealRole
        {
            RoleId = roleId,
            RoleName = RoleName.Create(roleName),
            RoleLevel = roleLevel,
            RoleModifiedBy = modifiedBy,
            RoleModifiedOn = DateTime.UtcNow
        };
        role.AddDomainEvent(new RoleCreatedEvent(roleId, roleName));
        return role;
    }

    public void Update(string roleName, long roleLevel, decimal modifiedBy)
    {
        RoleName = RoleName.Create(roleName);
        RoleLevel = roleLevel;
        RoleModifiedBy = modifiedBy;
        RoleModifiedOn = DateTime.UtcNow;
        AddDomainEvent(new RoleUpdatedEvent(RoleId, roleName));
    }
}
