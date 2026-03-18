namespace AccessService.Application.CQRS.Commands;

using MediatR;

/// <summary>
/// Commands for UserRole aggregate
/// </summary>

public class AssignUserRoleCommand : IRequest<int>
{
    public long EmployeeSystemId { get; set; }
    
    public char RoleType { get; set; }
    
    public char? MenuAccess { get; set; }
    
    public int? OrganizationId { get; set; }
    
    public int? UnitId { get; set; }
    
    public long? CalendarId { get; set; }
}

public class RevokeUserRoleCommand : IRequest
{
    public int RoleId { get; set; }
    
    public DateTime ClosureDate { get; set; }
}

public class UpdateUserRoleCommand : IRequest
{
    public int RoleId { get; set; }
    
    public char? MenuAccess { get; set; }
    
    public int? OrganizationId { get; set; }
    
    public int? UnitId { get; set; }
    
    public long? CalendarId { get; set; }
}


