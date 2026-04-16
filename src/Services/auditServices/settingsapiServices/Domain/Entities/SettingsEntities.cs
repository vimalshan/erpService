using SettingsService.Domain.Events;

namespace SettingsService.Domain.Entities;

public class User
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime? LastLoginDate { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
    public int? CreatedBy { get; set; }
    public int? ModifiedBy { get; set; }
    public string? Phone { get; set; }
    public string? Position { get; set; }
    public string? Department { get; set; }
    public string? TimeZone { get; set; } = "UTC";
    public string? Language { get; set; } = "EN";
    public bool IsEmailVerified { get; set; }
    public string? EmailVerificationToken { get; set; }
    public string? PasswordResetToken { get; set; }
    public DateTime? PasswordResetExpiry { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public string? TwoFactorSecret { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<UserPreference> Preferences { get; set; } = new List<UserPreference>();

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public void ClearDomainEvents() => _domainEvents.Clear();

    public static User Create(string username, string email, string firstName, string lastName,
        string passwordHash, int? createdBy)
    {
        var u = new User
        {
            Username = username, Email = email, FirstName = firstName, LastName = lastName,
            PasswordHash = passwordHash, IsActive = true, CreatedBy = createdBy, ModifiedBy = createdBy,
            CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow
        };
        u._domainEvents.Add(new UserCreatedEvent(0, username, email));
        return u;
    }

    public void Deactivate(int? modifiedBy)
    {
        IsActive = false; ModifiedDate = DateTime.UtcNow; ModifiedBy = modifiedBy;
        _domainEvents.Add(new UserDeactivatedEvent(UserId, Username));
    }
}

public class Role
{
    public int RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public string RoleCode { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
    public int? CreatedBy { get; set; }
    public int? ModifiedBy { get; set; }
    public bool IsSystemRole { get; set; }
    public string? Permissions { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}

public class UserRole
{
    public int UserRoleId { get; set; }
    public int UserId { get; set; }
    public int RoleId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
    public int? CreatedBy { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ExpiryDate { get; set; }

    public User User { get; set; } = null!;
    public Role Role { get; set; } = null!;
}

public class UserPreference
{
    public int UserPreferenceId { get; set; }
    public int UserId { get; set; }
    public string PreferenceKey { get; set; } = string.Empty;
    public string? PreferenceValue { get; set; }
    public string PreferenceType { get; set; } = "String";
    public string? Category { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
    public int? CreatedBy { get; set; }
    public int? ModifiedBy { get; set; }

    public User User { get; set; } = null!;
}
