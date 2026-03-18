namespace AccessService.Domain.Events;

/// <summary>
/// Domain events for user-related state changes
/// </summary>

public class UserMapCreatedEvent : DomainEvent
{
    public long EmployeeSystemId { get; set; }
    
    public UserMapCreatedEvent(long employeeSystemId)
    {
        EmployeeSystemId = employeeSystemId;
    }
}

public class UserMapActivatedEvent : DomainEvent
{
    public long EmployeeSystemId { get; set; }
    
    public DateTime EffectiveDate { get; set; }
    
    public UserMapActivatedEvent(long employeeSystemId, DateTime effectiveDate)
    {
        EmployeeSystemId = employeeSystemId;
        EffectiveDate = effectiveDate;
    }
}

public class UserMapDeactivatedEvent : DomainEvent
{
    public long EmployeeSystemId { get; set; }
    
    public DateTime ClosureDate { get; set; }
    
    public UserMapDeactivatedEvent(long employeeSystemId, DateTime closureDate)
    {
        EmployeeSystemId = employeeSystemId;
        ClosureDate = closureDate;
    }
}

public class UserRoleAssignedEvent : DomainEvent
{
    public int RoleId { get; set; }
    
    public long EmployeeSystemId { get; set; }
    
    public char RoleType { get; set; }
    
    public UserRoleAssignedEvent(int roleId, long employeeSystemId, char roleType)
    {
        RoleId = roleId;
        EmployeeSystemId = employeeSystemId;
        RoleType = roleType;
    }
}

public class UserRoleRevokedEvent : DomainEvent
{
    public int RoleId { get; set; }
    
    public long EmployeeSystemId { get; set; }
    
    public UserRoleRevokedEvent(int roleId, long employeeSystemId)
    {
        RoleId = roleId;
        EmployeeSystemId = employeeSystemId;
    }
}

public class MenuAccessGrantedEvent : DomainEvent
{
    public int UserRoleId { get; set; }
    
    public int MenuId { get; set; }
    
    public MenuAccessGrantedEvent(int userRoleId, int menuId)
    {
        UserRoleId = userRoleId;
        MenuId = menuId;
    }
}

public class MenuAccessRevokedEvent : DomainEvent
{
    public int UserRoleId { get; set; }
    
    public int MenuId { get; set; }
    
    public MenuAccessRevokedEvent(int userRoleId, int menuId)
    {
        UserRoleId = userRoleId;
        MenuId = menuId;
    }
}
