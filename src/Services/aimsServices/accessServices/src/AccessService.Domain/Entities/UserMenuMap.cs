namespace AccessService.Domain.Entities;

/// <summary>
/// AIMS_USERMENUMAP - User Role Menu Access Mapping
/// Maps specific menu access to user roles
/// </summary>
public class UserMenuMap : Entity
{
    public int? UserRoleId { get; private set; }
    
    public int? MenuId { get; private set; }
    
    public long? ModifiedBy { get; private set; }
    
    public DateTime? ModifiedOn { get; private set; }

    private UserMenuMap() { }

    public UserMenuMap(int userRoleId, int menuId)
    {
        if (userRoleId <= 0)
            throw new ArgumentException("User role ID must be greater than 0", nameof(userRoleId));

        if (menuId <= 0)
            throw new ArgumentException("Menu ID must be greater than 0", nameof(menuId));

        UserRoleId = userRoleId;
        MenuId = menuId;
    }

    public void MarkAsModified(long modifiedBy)
    {
        if (modifiedBy <= 0)
            throw new ArgumentException("Modified by must be greater than 0", nameof(modifiedBy));

        ModifiedBy = modifiedBy;
        ModifiedOn = DateTime.UtcNow;
    }
}
