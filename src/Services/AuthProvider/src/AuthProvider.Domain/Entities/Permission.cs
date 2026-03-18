namespace AuthProvider.Domain.Entities;

/// <summary>Permission entity – fine-grained access right.</summary>
public class Permission
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Resource { get; private set; } = string.Empty;
    public string Action { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    private Permission() { }

    public static Permission Create(string name, string resource, string action) =>
        new()
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Resource = resource.Trim().ToLowerInvariant(),
            Action = action.Trim().ToLowerInvariant(),
            CreatedAt = DateTime.UtcNow
        };
}
