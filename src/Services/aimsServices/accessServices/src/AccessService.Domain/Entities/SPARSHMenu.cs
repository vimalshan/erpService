namespace AccessService.Domain.Entities;

/// <summary>
/// SPARSHMENU_MASTER - SPARSH System Menu Master
/// Defines menus specific to the SPARSH system
/// </summary>
public class SPARSHMenu : AggregateRoot
{
    public long MenuId { get; private set; }
    
    public string Name { get; private set; }
    
    public string PageName { get; private set; }
    
    public long LastModifiedBy { get; private set; }
    
    public DateTime LastModifiedOn { get; private set; }

    private SPARSHMenu() { }

    public SPARSHMenu(long menuId, string name, string pageName, long lastModifiedBy)
    {
        if (menuId <= 0)
            throw new ArgumentException("Menu ID must be greater than 0", nameof(menuId));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Menu name cannot be empty", nameof(name));

        if (string.IsNullOrWhiteSpace(pageName))
            throw new ArgumentException("Page name cannot be empty", nameof(pageName));

        if (lastModifiedBy <= 0)
            throw new ArgumentException("Last modified by must be greater than 0", nameof(lastModifiedBy));

        MenuId = menuId;
        Name = name;
        PageName = pageName;
        LastModifiedBy = lastModifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }

    public void Update(string name, string pageName, long lastModifiedBy)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Menu name cannot be empty", nameof(name));

        if (string.IsNullOrWhiteSpace(pageName))
            throw new ArgumentException("Page name cannot be empty", nameof(pageName));

        if (lastModifiedBy <= 0)
            throw new ArgumentException("Last modified by must be greater than 0", nameof(lastModifiedBy));

        Name = name;
        PageName = pageName;
        LastModifiedBy = lastModifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }
}
