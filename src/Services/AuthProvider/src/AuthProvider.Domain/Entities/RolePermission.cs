namespace AuthProvider.Domain.Entities;

/// <summary>RolePermission join entity (many-to-many Role ↔ Permission).</summary>
public class RolePermission
{
    public Guid RoleId { get; private set; }
    public Guid PermissionId { get; private set; }
    public DateTime AssignedAt { get; private set; }

    public Role Role { get; private set; } = null!;
    public Permission Permission { get; private set; } = null!;

    private RolePermission() { }

    public static RolePermission Create(Guid roleId, Guid permissionId) =>
        new() { RoleId = roleId, PermissionId = permissionId, AssignedAt = DateTime.UtcNow };
}
