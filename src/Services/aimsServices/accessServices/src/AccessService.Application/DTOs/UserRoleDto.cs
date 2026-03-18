namespace AccessService.Application.DTOs;

/// <summary>
/// DTOs for UserRole entity
/// </summary>

public class CreateUserRoleDto
{
    public long EmployeeSystemId { get; set; }
    
    public char RoleType { get; set; }  // S, U, C
    
    public char? MenuAccess { get; set; }
    
    public int? OrganizationId { get; set; }
    
    public int? UnitId { get; set; }
    
    public long? CalendarId { get; set; }
}

public class UpdateUserRoleDto
{
    public char? MenuAccess { get; set; }
    
    public int? OrganizationId { get; set; }
    
    public int? UnitId { get; set; }
    
    public long? CalendarId { get; set; }
    
    public DateTime? EffectiveDate { get; set; }
    
    public DateTime? ClosureDate { get; set; }
}

public class UserRoleDto
{
    public int RoleId { get; set; }
    
    public long? EmployeeSystemId { get; set; }
    
    public char? RoleType { get; set; }
    
    public string? RoleTypeDescription { get; set; }
    
    public char? MenuAccess { get; set; }
    
    public int? OrganizationId { get; set; }
    
    public int? UnitId { get; set; }
    
    public long? CalendarId { get; set; }
    
    public DateTime? EffectiveDate { get; set; }
    
    public DateTime? ClosureDate { get; set; }
    
    public long? ModifiedBy { get; set; }
    
    public DateTime? ModifiedOn { get; set; }
    
    public bool IsActive { get; set; }
}
