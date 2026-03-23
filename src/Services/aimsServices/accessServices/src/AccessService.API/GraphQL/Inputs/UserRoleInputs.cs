namespace AccessService.API.GraphQL.Inputs;

/// <summary>
/// Input for assigning a role to an employee.
/// roleType: "S" = SuperUser | "U" = UnitAccess | "C" = CalendarAccess
/// menuAccess: single character menu access code (optional)
/// </summary>
public class AssignUserRoleInput
{
    public long EmployeeSystemId { get; set; }

    /// <summary>S = SuperUser | U = UnitAccess | C = CalendarAccess</summary>
    public string RoleType { get; set; } = string.Empty;

    public string? MenuAccess { get; set; }
    public int? OrganizationId { get; set; }
    public int? UnitId { get; set; }
    public long? CalendarId { get; set; }
}

/// <summary>Input for updating an existing UserRole</summary>
public class UpdateUserRoleInput
{
    public int RoleId { get; set; }
    public string? MenuAccess { get; set; }
    public int? OrganizationId { get; set; }
    public int? UnitId { get; set; }
    public long? CalendarId { get; set; }
}

/// <summary>Input for revoking (closing) a UserRole</summary>
public class RevokeUserRoleInput
{
    public int RoleId { get; set; }
    public DateTime ClosureDate { get; set; }
}
