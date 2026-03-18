namespace AuthProvider.Domain.Entities;

/// <summary>Role entity – represents a named set of permissions.</summary>
public class Role
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private readonly List<RolePermission> _rolePermissions = new();
    public IReadOnlyCollection<RolePermission> RolePermissions => _rolePermissions.AsReadOnly();

    private Role() { }

    public static Role Create(string name, string description = "")
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Role name cannot be empty.", nameof(name));

        return new Role
        {
            Id = Guid.NewGuid(),
            Name = name.Trim().ToUpperInvariant(),
            Description = description.Trim(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void AssignPermission(Permission permission)
    {
        if (_rolePermissions.Any(rp => rp.PermissionId == permission.Id)) return;
        _rolePermissions.Add(RolePermission.Create(Id, permission.Id));
    }

    public void Update(string name, string description)
    {
        Name = name.Trim().ToUpperInvariant();
        Description = description.Trim();
    }
}
