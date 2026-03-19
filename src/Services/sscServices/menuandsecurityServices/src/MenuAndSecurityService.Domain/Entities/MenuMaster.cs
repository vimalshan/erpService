using MenuAndSecurityService.Domain.Common;
using MenuAndSecurityService.Domain.Events;

namespace MenuAndSecurityService.Domain.Entities;

public class MenuMaster : AuditableEntity
{
    public long MenuId { get; set; }
    public string MenuName { get; set; } = string.Empty;
    public string MenuPageName { get; set; } = string.Empty;
    public long? MenuParentId { get; set; }
    public int MenuDisplayOrder { get; set; }

    // Navigation
    public MenuMaster? Parent { get; set; }
    public ICollection<MenuMaster> Children { get; set; } = new List<MenuMaster>();
    public ICollection<RoleMenuAccess> RoleMenuAccesses { get; set; } = new List<RoleMenuAccess>();

    public static MenuMaster Create(long menuId, string menuName, string pageName,
        long? parentId, int displayOrder, long modifiedBy)
    {
        var menu = new MenuMaster
        {
            MenuId = menuId,
            MenuName = menuName,
            MenuPageName = pageName,
            MenuParentId = parentId,
            MenuDisplayOrder = displayOrder,
            ModifiedBy = modifiedBy,
            ModifiedOn = DateTime.UtcNow
        };

        menu.AddDomainEvent(new MenuCreatedEvent(menu.MenuId, menu.MenuName));
        return menu;
    }

    public void Update(string menuName, string pageName, long? parentId, int displayOrder, long modifiedBy)
    {
        MenuName = menuName;
        MenuPageName = pageName;
        MenuParentId = parentId;
        MenuDisplayOrder = displayOrder;
        ModifiedBy = modifiedBy;
        ModifiedOn = DateTime.UtcNow;

        AddDomainEvent(new MenuUpdatedEvent(MenuId, MenuName));
    }
}
