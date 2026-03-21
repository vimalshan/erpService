using SecurityService.Domain.Entities;

namespace SecurityService.Domain.Aggregates;

/// <summary>
/// User aggregate root - manages user with roles and permissions.
/// </summary>
public class UserAggregate
{
    public User User { get; }
    public IReadOnlyList<Role> Roles { get; }
    public IReadOnlyList<Permission> Permissions { get; }

    public UserAggregate(User user, IReadOnlyList<Role> roles, IReadOnlyList<Permission> permissions)
    {
        User = user;
        Roles = roles;
        Permissions = permissions;
    }

    public bool HasPermission(string permissionName) =>
        Permissions.Any(p => p.PermissionName.Equals(permissionName, StringComparison.OrdinalIgnoreCase));

    public bool HasRole(string roleName) =>
        Roles.Any(r => r.RoleName.Equals(roleName, StringComparison.OrdinalIgnoreCase));
}
