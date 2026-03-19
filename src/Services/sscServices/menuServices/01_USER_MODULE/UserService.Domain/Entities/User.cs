using UserService.Domain.Abstractions;

namespace UserService.Domain.Entities;

/// <summary>
/// User Role enumeration
/// </summary>
public enum UserRole
{
    EndUser = 1,
    SpecialApprover = 2,
    UnitMailRoom = 3,
    SSCMailRoom = 4,
    APProcessor = 5,
    APValidator = 6,
    Admin = 7
}

/// <summary>
/// User entity - aggregate root
/// </summary>
public class User : Domain.Abstractions.AggregateRoot
{
    public required string Name { get; set; }
    public required string PasswordHash { get; set; }
    public required string EmailId { get; set; }
    public string? SparchUserId { get; set; }
    public long? HrEmpSysId { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime? ClosureDate { get; set; }
    public long EnteredBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }

    // Navigation properties
    public ICollection<UserRoleMapping> RoleMappings { get; } = new List<UserRoleMapping>();
    public ICollection<UserOrganizationMapping> OrganizationMappings { get; } = new List<UserOrganizationMapping>();
    public ICollection<UserLocationMapping> LocationMappings { get; } = new List<UserLocationMapping>();

    private User()
    {
    }

    public static User Create(
        string name,
        string passwordHash,
        string emailId,
        long enteredBy,
        string? sparchUserId = null,
        long? hrEmpSysId = null)
    {
        var user = new User
        {
            Name = name,
            PasswordHash = passwordHash,
            EmailId = emailId,
            SparchUserId = sparchUserId,
            HrEmpSysId = hrEmpSysId,
            EffectiveDate = DateTime.UtcNow,
            EnteredBy = enteredBy,
            CreatedDate = DateTime.UtcNow,
            IsActive = true
        };

        return user;
    }

    public void AssignRole(long roleId, bool isDefault = false)
    {
        var existingRole = RoleMappings.FirstOrDefault(r => r.RoleId == roleId);
        if (existingRole == null)
        {
            RoleMappings.Add(new UserRoleMapping
            {
                UserId = Id,
                RoleId = roleId,
                IsDefault = isDefault,
                CreatedDate = DateTime.UtcNow
            });
        }
    }

    public void AssignOrganization(string buId)
    {
        var existingOrg = OrganizationMappings.FirstOrDefault(o => o.BusinessUnitId == buId);
        if (existingOrg == null)
        {
            OrganizationMappings.Add(new UserOrganizationMapping
            {
                UserId = Id,
                BusinessUnitId = buId,
                CreatedDate = DateTime.UtcNow
            });
        }
    }

    public void AssignLocation(int locationId)
    {
        var existingLocation = LocationMappings.FirstOrDefault(l => l.LocationId == locationId);
        if (existingLocation == null)
        {
            LocationMappings.Add(new UserLocationMapping
            {
                UserId = Id,
                LocationId = locationId,
                CreatedDate = DateTime.UtcNow
            });
        }
    }

    public void Deactivate()
    {
        IsActive = false;
        ClosureDate = DateTime.UtcNow;
        ModifiedDate = DateTime.UtcNow;
    }
}

/// <summary>
/// User Role Mapping entity
/// </summary>
public class UserRoleMapping : Domain.Abstractions.Entity
{
    public long UserId { get; set; }
    public long RoleId { get; set; }
    public bool IsDefault { get; set; }
    public DateTime CreatedDate { get; set; }
    public long CreatedBy { get; set; }

    // Navigation properties
    public User? User { get; set; }
}

/// <summary>
/// User Organization Mapping entity
/// </summary>
public class UserOrganizationMapping : Domain.Abstractions.Entity
{
    public long UserId { get; set; }
    public string BusinessUnitId { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public long CreatedBy { get; set; }

    // Navigation properties
    public User? User { get; set; }
}

/// <summary>
/// User Location Mapping entity
/// </summary>
public class UserLocationMapping : Domain.Abstractions.Entity
{
    public long UserId { get; set; }
    public int LocationId { get; set; }
    public DateTime CreatedDate { get; set; }
    public long CreatedBy { get; set; }

    // Navigation properties
    public User? User { get; set; }
}
