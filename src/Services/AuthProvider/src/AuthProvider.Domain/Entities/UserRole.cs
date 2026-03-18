namespace AuthProvider.Domain.Entities;

/// <summary>UserRole join entity (many-to-many User ↔ Role).</summary>
public class UserRole
{
    public Guid UserId { get; private set; }
    public Guid RoleId { get; private set; }
    public DateTime AssignedAt { get; private set; }

    public User User { get; private set; } = null!;
    public Role Role { get; private set; } = null!;

    private UserRole() { }

    public static UserRole Create(Guid userId, Guid roleId) =>
        new() { UserId = userId, RoleId = roleId, AssignedAt = DateTime.UtcNow };
}
