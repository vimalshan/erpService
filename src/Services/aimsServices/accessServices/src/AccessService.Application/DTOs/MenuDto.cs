namespace AccessService.Application.DTOs;

/// <summary>
/// DTOs for Menu and related entities
/// </summary>

public class CreateMenuDto
{
    public string Name { get; set; } = string.Empty;
    
    public int? ParentMenuId { get; set; }
    
    public string? Path { get; set; }
    
    public char? CalendarRole { get; set; }
    
    public char? Type { get; set; }
    
    public int? DisplayOrder { get; set; }
}

public class UpdateMenuDto
{
    public string? Name { get; set; }
    
    public int? ParentMenuId { get; set; }
    
    public string? Path { get; set; }
    
    public char? CalendarRole { get; set; }
    
    public char? Type { get; set; }
    
    public int? DisplayOrder { get; set; }
}

public class MenuDto
{
    public int MenuId { get; set; }
    
    public string? Name { get; set; }
    
    public int? ParentMenuId { get; set; }
    
    public string? Path { get; set; }
    
    public char? CalendarRole { get; set; }
    
    public char? Type { get; set; }
    
    public int? DisplayOrder { get; set; }
    
    public long? ModifiedBy { get; set; }
    
    public DateTime? ModifiedOn { get; set; }
    
    public bool IsRootMenu { get; set; }
}

public class CreateSPARSHMenuDto
{
    public string Name { get; set; } = string.Empty;
    
    public string PageName { get; set; } = string.Empty;
}

public class UpdateSPARSHMenuDto
{
    public string? Name { get; set; }
    
    public string? PageName { get; set; }
}

public class SPARSHMenuDto
{
    public long MenuId { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string PageName { get; set; } = string.Empty;
    
    public long LastModifiedBy { get; set; }
    
    public DateTime LastModifiedOn { get; set; }
}

public class CreateSPARSHMenuAccessDto
{
    public long UnitId { get; set; }
    
    public long CalendarId { get; set; }
    
    public string GradeCategory { get; set; } = string.Empty;
    
    public long SPARSHMenuId { get; set; }
}

public class SPARSHMenuAccessDto
{
    public long AccessId { get; set; }
    
    public long UnitId { get; set; }
    
    public long CalendarId { get; set; }
    
    public string GradeCategory { get; set; } = string.Empty;
    
    public long SPARSHMenuId { get; set; }
}
