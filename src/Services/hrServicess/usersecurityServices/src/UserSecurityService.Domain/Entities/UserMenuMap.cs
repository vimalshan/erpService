using UserSecurityService.Domain.Common;

namespace UserSecurityService.Domain.Entities;

/// <summary>Maps an HR role to menu items.</summary>
public class UserMenuMap : BaseEntity
{
    public decimal UserRoleId { get; private set; }
    public decimal UserMenuId { get; private set; }
    public decimal UserModifiedBy { get; private set; }
    public DateTime UserModifiedOn { get; private set; }

    private UserMenuMap() { }

    public static UserMenuMap Create(decimal roleId, decimal menuId, decimal createdBy)
    {
        return new UserMenuMap
        {
            UserRoleId = roleId,
            UserMenuId = menuId,
            UserModifiedBy = createdBy,
            UserModifiedOn = DateTime.UtcNow
        };
    }

    public void Update(decimal menuId, decimal modifiedBy)
    {
        UserMenuId = menuId;
        UserModifiedBy = modifiedBy;
        UserModifiedOn = DateTime.UtcNow;
    }
}
