using OrganizationSetup.Domain.Common;
using OrganizationSetup.Domain.Events;

namespace OrganizationSetup.Domain.Entities;

/// <summary>Maps to DEAL_USERMAP table - User Role Mapping.</summary>
public class DealUserMap : BaseEntity
{
    public long RoleMapId { get; private set; }
    public long RoleId { get; private set; }
    public long RoleEmpSysId { get; private set; }
    public long RoleOrgId { get; private set; }
    public long? RoleBusiness { get; private set; }

    // Navigation
    public DealRole Role { get; private set; } = default!;

    private DealUserMap() { }

    public static DealUserMap Create(long mapId, long roleId, long empSysId, long orgId, long? business)
    {
        var map = new DealUserMap
        {
            RoleMapId = mapId,
            RoleId = roleId,
            RoleEmpSysId = empSysId,
            RoleOrgId = orgId,
            RoleBusiness = business
        };
        map.AddDomainEvent(new UserMappedToRoleEvent(mapId, roleId, empSysId, orgId));
        return map;
    }

    public void UpdateBusiness(long? businessUnit)
    {
        RoleBusiness = businessUnit;
    }
}
