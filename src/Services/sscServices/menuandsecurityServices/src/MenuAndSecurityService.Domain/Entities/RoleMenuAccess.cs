using MenuAndSecurityService.Domain.Common;
using MenuAndSecurityService.Domain.Events;

namespace MenuAndSecurityService.Domain.Entities;

public class RoleMenuAccess : BaseEntity
{
    public long MenuAccessId { get; set; }
    public long MenuId { get; set; }
    public long MenuRoleId { get; set; }
    public long? RoleModifiedBy { get; set; }
    public DateTime? RoleModifiedOn { get; set; }

    // Navigation
    public MenuMaster? Menu { get; set; }

    public static RoleMenuAccess Grant(long accessId, long menuId, long roleId, long modifiedBy)
    {
        var access = new RoleMenuAccess
        {
            MenuAccessId = accessId,
            MenuId = menuId,
            MenuRoleId = roleId,
            RoleModifiedBy = modifiedBy,
            RoleModifiedOn = DateTime.UtcNow
        };

        access.AddDomainEvent(new MenuAccessGrantedEvent(accessId, menuId, roleId));
        return access;
    }

    public void Revoke()
    {
        AddDomainEvent(new MenuAccessRevokedEvent(MenuAccessId, MenuId, MenuRoleId));
    }
}
