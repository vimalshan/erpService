using AdminService.Domain.Entities;

namespace AdminService.Domain.Aggregates;

/// <summary>
/// AdminMaster is the aggregate root for admin location management.
/// It manages its child UserMaps and AccessRights.
/// </summary>
public class AdminAggregate
{
    public AdminMaster Root { get; }

    public AdminAggregate(AdminMaster root)
    {
        Root = root ?? throw new ArgumentNullException(nameof(root));
    }

    public void AddUserMap(AdminUserMap userMap)
    {
        if (string.IsNullOrWhiteSpace(userMap.AdminMapId))
            throw new InvalidOperationException("Map ID is required.");
        userMap.AdminId = Root.AdminId;
        Root.UserMaps.Add(userMap);
    }

    public void GrantAccessRights(AdminAccessRights rights)
    {
        if (string.IsNullOrWhiteSpace(rights.AdminRightsId))
            throw new InvalidOperationException("Rights ID is required.");
        rights.AdminLocationId = Root.AdminId;
        Root.AccessRights.Add(rights);
    }

    public void Deactivate()
    {
        Root.AdminLocStatus = 'I';
    }

    public void Activate()
    {
        Root.AdminLocStatus = 'A';
    }
}
