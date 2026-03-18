using UserSecurityService.Domain.Common;

namespace UserSecurityService.Domain.Entities;

/// <summary>Audit log snapshot of UserMenuMap changes.</summary>
public class UserMenuMapLog : BaseEntity
{
    public decimal UserRoleId { get; private set; }
    public decimal UserMenuId { get; private set; }
    public decimal UserModifiedBy { get; private set; }
    public DateTime UserModifiedOn { get; private set; }
    public decimal LogCreatedBy { get; private set; }
    public DateTime LogCreatedOn { get; private set; }

    private UserMenuMapLog() { }

    public static UserMenuMapLog FromMenuMap(UserMenuMap map, decimal logCreatedBy)
    {
        return new UserMenuMapLog
        {
            UserRoleId = map.UserRoleId,
            UserMenuId = map.UserMenuId,
            UserModifiedBy = map.UserModifiedBy,
            UserModifiedOn = map.UserModifiedOn,
            LogCreatedBy = logCreatedBy,
            LogCreatedOn = DateTime.UtcNow
        };
    }
}
