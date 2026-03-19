namespace UserService.Application.DTOs;

/// <summary>
/// User response DTO
/// </summary>
public class UserDto
{
    public long UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string EmailId { get; set; } = string.Empty;
    public string? SparchUserId { get; set; }
    public long? HrEmpSysId { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime? ClosureDate { get; set; }
    public bool IsActive { get; set; }
    public List<UserRoleMappingDto> RoleMappings { get; set; } = new();
    public List<UserOrganizationMappingDto> OrganizationMappings { get; set; } = new();
    public List<UserLocationMappingDto> LocationMappings { get; set; } = new();
}

/// <summary>
/// Create User request DTO
/// </summary>
public class CreateUserRequest
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string EmailId { get; set; } = string.Empty;
    public long EnteredBy { get; set; }
    public string? SparchUserId { get; set; }
    public long? HrEmpSysId { get; set; }
}

/// <summary>
/// Update User request DTO
/// </summary>
public class UpdateUserRequest
{
    public long UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string EmailId { get; set; } = string.Empty;
    public string? SparchUserId { get; set; }
}

/// <summary>
/// Login request DTO
/// </summary>
public class LoginRequest
{
    public string UserEmail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// Login response DTO
/// </summary>
public class LoginResponse
{
    public long UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string EmailId { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public DateTime TokenExpiry { get; set; }
}

/// <summary>
/// User Role Mapping DTO
/// </summary>
public class UserRoleMappingDto
{
    public long RoleMapId { get; set; }
    public long UserId { get; set; }
    public long RoleId { get; set; }
    public bool IsDefault { get; set; }
    public DateTime CreatedDate { get; set; }
}

/// <summary>
/// User Organization Mapping DTO
/// </summary>
public class UserOrganizationMappingDto
{
    public long OrgMapId { get; set; }
    public long UserId { get; set; }
    public string BusinessUnitId { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}

/// <summary>
/// User Location Mapping DTO
/// </summary>
public class UserLocationMappingDto
{
    public long LocationMapId { get; set; }
    public long UserId { get; set; }
    public int LocationId { get; set; }
    public DateTime CreatedDate { get; set; }
}

/// <summary>
/// Assign Role request DTO
/// </summary>
public class AssignRoleRequest
{
    public long UserId { get; set; }
    public long RoleId { get; set; }
    public bool IsDefault { get; set; }
}

/// <summary>
/// Assign Organization request DTO
/// </summary>
public class AssignOrganizationRequest
{
    public long UserId { get; set; }
    public string BusinessUnitId { get; set; } = string.Empty;
}

/// <summary>
/// Assign Location request DTO
/// </summary>
public class AssignLocationRequest
{
    public long UserId { get; set; }
    public int LocationId { get; set; }
}
