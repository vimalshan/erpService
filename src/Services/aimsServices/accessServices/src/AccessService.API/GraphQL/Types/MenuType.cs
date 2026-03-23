namespace AccessService.API.GraphQL.Types;

using AccessService.Domain.Entities;

/// <summary>
/// GraphQL output type for Menu.
/// char? fields (CalendarRole, Type) are projected as string? for GraphQL compatibility.
/// </summary>
public class MenuType
{
    public int MenuId { get; init; }
    public string? Name { get; init; }
    public int? ParentMenuId { get; init; }
    public string? Path { get; init; }

    /// <summary>Single-character calendar role code</summary>
    public string? CalendarRole { get; init; }

    /// <summary>Single-character menu type code</summary>
    public string? Type { get; init; }

    public int? DisplayOrder { get; init; }
    public long? ModifiedBy { get; init; }
    public DateTime? ModifiedOn { get; init; }
    public bool IsRootMenu { get; init; }

    public static MenuType FromEntity(Menu m) => new()
    {
        MenuId       = m.MenuId,
        Name         = m.Name,
        ParentMenuId = m.ParentMenuId,
        Path         = m.Path,
        CalendarRole = m.CalendarRole?.ToString(),
        Type         = m.Type?.ToString(),
        DisplayOrder = m.DisplayOrder,
        ModifiedBy   = m.ModifiedBy,
        ModifiedOn   = m.ModifiedOn,
        IsRootMenu   = m.IsRootMenu()
    };
}
