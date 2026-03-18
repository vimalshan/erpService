namespace AccessService.Domain.Entities;

/// <summary>
/// AIMS_USERROLE - User Access Role Mapping
/// Defines user roles with type (SuperUser, Unit Access, Calendar Access)
/// </summary>
public class UserRole : AggregateRoot
{
    public int RoleId { get; private set; }
    
    public long? EmployeeSystemId { get; private set; }
    
    public char? RoleType { get; private set; }  // S=SuperUser, U=Unit Access, C=Calendar Access
    
    public char? MenuAccess { get; private set; }  // All Menus / View Only / Specific Menus
    
    public int? OrganizationId { get; private set; }
    
    public int? UnitId { get; private set; }
    
    public long? CalendarId { get; private set; }
    
    public DateTime? EffectiveDate { get; private set; }
    
    public DateTime? ClosureDate { get; private set; }
    
    public long? ModifiedBy { get; private set; }
    
    public DateTime? ModifiedOn { get; private set; }

    private UserRole() { }

    public UserRole(int roleId)
    {
        if (roleId <= 0)
            throw new ArgumentException("Role ID must be greater than 0", nameof(roleId));

        RoleId = roleId;
    }

    /// <summary>
    /// Creates a new UserRole for persistence. RoleId will be assigned by the database.
    /// </summary>
    public static UserRole CreateNew() => new UserRole();

    public void SetEmployeeSystemId(long employeeSystemId)
    {
        if (employeeSystemId <= 0)
            throw new ArgumentException("Employee system ID must be greater than 0", nameof(employeeSystemId));

        EmployeeSystemId = employeeSystemId;
    }

    public void SetRoleType(char roleType)
    {
        var validTypes = new[] { 'S', 'U', 'C' };
        if (!validTypes.Contains(roleType))
            throw new ArgumentException("Invalid role type. Must be S, U, or C", nameof(roleType));

        RoleType = roleType;
    }

    public void SetMenuAccess(char menuAccess)
    {
        MenuAccess = menuAccess;
    }

    public void SetOrganizationAndUnit(int? orgId, int? unitId)
    {
        OrganizationId = orgId;
        UnitId = unitId;
    }

    public void SetCalendarId(long? calendarId)
    {
        CalendarId = calendarId;
    }

    public void SetEffectiveDate(DateTime effectiveDate)
    {
        EffectiveDate = effectiveDate;
    }

    public void SetClosureDate(DateTime? closureDate)
    {
        ClosureDate = closureDate;
    }

    public void MarkAsModified(long modifiedBy)
    {
        if (modifiedBy <= 0)
            throw new ArgumentException("Modified by must be greater than 0", nameof(modifiedBy));

        ModifiedBy = modifiedBy;
        ModifiedOn = DateTime.UtcNow;
    }

    public bool IsActive()
    {
        var now = DateTime.UtcNow;
        var effectiveCheck = EffectiveDate == null || EffectiveDate <= now;
        var closureCheck = ClosureDate == null || ClosureDate > now;
        return effectiveCheck && closureCheck;
    }

    public string GetRoleTypeDescription() => RoleType switch
    {
        'S' => "Super User",
        'U' => "Unit Access",
        'C' => "Calendar Access",
        _ => "Unknown"
    };
}
