using UserManagement.Domain.Common;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Events;

namespace UserManagement.Domain.Aggregates;

/// <summary>
/// User Aggregate Root — encapsulates UserPolicy and WebsiteContactEmail
/// for a single user (identified by UserSysId).
/// </summary>
public class UserAggregate : BaseEntity
{
    public long UserSysId { get; private set; }
    public UserPolicy? Policy { get; private set; }
    public IReadOnlyList<WebsiteContactEmail> Contacts { get; private set; } = [];

    private UserAggregate() { }

    public static UserAggregate Reconstitute(
        long userSysId,
        UserPolicy? policy,
        IEnumerable<WebsiteContactEmail> contacts)
    {
        if (userSysId <= 0) throw new DomainException("UserSysId must be positive.");
        return new UserAggregate
        {
            UserSysId = userSysId,
            Policy = policy,
            Contacts = contacts.ToList().AsReadOnly()
        };
    }

    public void AttachPolicy(UserPolicy policy)
    {
        if (policy.UserSysId != UserSysId)
            throw new DomainException("Policy does not belong to this user.");
        Policy = policy;
    }

    public bool HasActivePolicy => Policy?.IsActive == true;
    public bool HasActiveContact => Contacts.Any(c => c.IsActive);

    public UserPolicy EnsurePolicy()
    {
        if (Policy is null)
            throw new DomainException($"User {UserSysId} has no policy assigned.");
        return Policy;
    }
}
