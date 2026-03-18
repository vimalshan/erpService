namespace AccessService.Domain.Entities;

/// <summary>
/// MENU_MASTER - Application Menu Hierarchy
/// Defines the menu structure for the AIMS application
/// </summary>
public class Menu : AggregateRoot
{
    public int MenuId { get; private set; }
    
    public string? Name { get; private set; }
    
    public int? ParentMenuId { get; private set; }
    
    public string? Path { get; private set; }
    
    public char? CalendarRole { get; private set; }
    
    public char? Type { get; private set; }
    
    public int? DisplayOrder { get; private set; }
    
    public long? ModifiedBy { get; private set; }
    
    public DateTime? ModifiedOn { get; private set; }

    private Menu() { }

    public Menu(int menuId, string name)
    {
        if (menuId <= 0)
            throw new ArgumentException("Menu ID must be greater than 0", nameof(menuId));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Menu name cannot be empty", nameof(name));

        MenuId = menuId;
        Name = name;
    }

    public void SetParentMenuId(int? parentMenuId)
    {
        if (parentMenuId == MenuId)
            throw new InvalidOperationException("Menu cannot be its own parent");

        ParentMenuId = parentMenuId;
    }

    public void SetMenuPath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("Menu path cannot be empty", nameof(path));

        Path = path;
    }

    public void SetCalendarRole(char? calendarRole)
    {
        CalendarRole = calendarRole;
    }

    public void SetMenuType(char? type)
    {
        Type = type;
    }

    public void SetDisplayOrder(int? displayOrder)
    {
        if (displayOrder.HasValue && displayOrder < 0)
            throw new ArgumentException("Display order must be non-negative", nameof(displayOrder));

        DisplayOrder = displayOrder;
    }

    public void MarkAsModified(long modifiedBy)
    {
        if (modifiedBy <= 0)
            throw new ArgumentException("Modified by must be greater than 0", nameof(modifiedBy));

        ModifiedBy = modifiedBy;
        ModifiedOn = DateTime.UtcNow;
    }

    public bool IsRootMenu() => ParentMenuId == null || ParentMenuId == 0;
}
