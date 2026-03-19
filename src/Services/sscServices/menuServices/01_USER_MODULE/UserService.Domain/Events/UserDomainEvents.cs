using UserService.Domain.Abstractions;

namespace UserService.Domain.Events;

/// <summary>
/// Event raised when a new user is created
/// </summary>
public class UserCreatedDomainEvent : IDomainEvent
{
    public long UserId { get; }
    public string UserName { get; }
    public string EmailId { get; }
    public DateTime OccurredOnUtc { get; }

    public UserCreatedDomainEvent(long userId, string userName, string emailId)
    {
        UserId = userId;
        UserName = userName;
        EmailId = emailId;
        OccurredOnUtc = DateTime.UtcNow;
    }
}

/// <summary>
/// Event raised when a user is deactivated
/// </summary>
public class UserDeactivatedDomainEvent : IDomainEvent
{
    public long UserId { get; }
    public DateTime OccurredOnUtc { get; }

    public UserDeactivatedDomainEvent(long userId)
    {
        UserId = userId;
        OccurredOnUtc = DateTime.UtcNow;
    }
}

/// <summary>
/// Event raised when a role is assigned to a user
/// </summary>
public class UserRoleAssignedDomainEvent : IDomainEvent
{
    public long UserId { get; }
    public long RoleId { get; }
    public DateTime OccurredOnUtc { get; }

    public UserRoleAssignedDomainEvent(long userId, long roleId)
    {
        UserId = userId;
        RoleId = roleId;
        OccurredOnUtc = DateTime.UtcNow;
    }
}

/// <summary>
/// Event raised when a user is assigned to an organization
/// </summary>
public class UserOrganizationAssignedDomainEvent : IDomainEvent
{
    public long UserId { get; }
    public string BusinessUnitId { get; }
    public DateTime OccurredOnUtc { get; }

    public UserOrganizationAssignedDomainEvent(long userId, string businessUnitId)
    {
        UserId = userId;
        BusinessUnitId = businessUnitId;
        OccurredOnUtc = DateTime.UtcNow;
    }
}

/// <summary>
/// Event raised when a user is assigned to a location
/// </summary>
public class UserLocationAssignedDomainEvent : IDomainEvent
{
    public long UserId { get; }
    public int LocationId { get; }
    public DateTime OccurredOnUtc { get; }

    public UserLocationAssignedDomainEvent(long userId, int locationId)
    {
        UserId = userId;
        LocationId = locationId;
        OccurredOnUtc = DateTime.UtcNow;
    }
}
