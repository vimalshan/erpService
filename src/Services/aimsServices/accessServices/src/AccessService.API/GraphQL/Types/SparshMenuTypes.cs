namespace AccessService.API.GraphQL.Types;

using AccessService.Domain.Entities;

/// <summary>GraphQL output type for SPARSH Menu</summary>
public class SparshMenuType
{
    public long MenuId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string PageName { get; init; } = string.Empty;
    public long LastModifiedBy { get; init; }
    public DateTime LastModifiedOn { get; init; }

    public static SparshMenuType FromEntity(SPARSHMenu m) => new()
    {
        MenuId         = m.MenuId,
        Name           = m.Name,
        PageName       = m.PageName,
        LastModifiedBy = m.LastModifiedBy,
        LastModifiedOn = m.LastModifiedOn
    };
}

/// <summary>GraphQL output type for SPARSH Menu Access</summary>
public class SparshMenuAccessType
{
    public long AccessId { get; init; }
    public long UnitId { get; init; }
    public long CalendarId { get; init; }

    /// <summary>3-character grade category code</summary>
    public string GradeCategory { get; init; } = string.Empty;
    public long SparshMenuId { get; init; }

    public static SparshMenuAccessType FromEntity(SPARSHMenuAccess a) => new()
    {
        AccessId      = a.AccessId,
        UnitId        = a.UnitId,
        CalendarId    = a.CalendarId,
        GradeCategory = a.GradeCategory,
        SparshMenuId  = a.SPARSHMenuId
    };
}
