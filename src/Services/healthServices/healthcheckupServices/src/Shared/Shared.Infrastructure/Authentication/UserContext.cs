namespace Shared.Infrastructure.Authentication;

using System.Security.Claims;
using Microsoft.AspNetCore.Http;

/// <summary>
/// User context service for extracting user information from JWT token
/// </summary>
public interface IUserContext
{
    string? UserId { get; }
    string? EmployeeNumber { get; }
    string? Email { get; }
    List<string> Roles { get; }
    List<string> Permissions { get; }
    Dictionary<string, object> Claims { get; }
}

/// <summary>
/// Implementation of user context from HTTP context principal
/// </summary>
public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private ClaimsPrincipal? _user => _httpContextAccessor?.HttpContext?.User;

    public string? UserId => _user?.FindFirst("sub")?.Value 
        ?? _user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    public string? EmployeeNumber => _user?.FindFirst("employee_number")?.Value 
        ?? _user?.FindFirst("emp_no")?.Value;

    public string? Email => _user?.FindFirst(ClaimTypes.Email)?.Value;

    public List<string> Roles => _user?
        .FindAll(ClaimTypes.Role)
        .Select(c => c.Value)
        .ToList() ?? new List<string>();

    public List<string> Permissions => _user?
        .FindAll("permission")
        .Select(c => c.Value)
        .ToList() ?? new List<string>();

    public Dictionary<string, object> Claims
    {
        get
        {
            var claims = new Dictionary<string, object>();
            if (_user?.Claims != null)
            {
                foreach (var claim in _user.Claims)
                {
                    claims.TryAdd(claim.Type, claim.Value);
                }
            }
            return claims;
        }
    }

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool HasRole(string role) => Roles.Contains(role, StringComparer.OrdinalIgnoreCase);
    public bool HasPermission(string permission) => Permissions.Contains(permission, StringComparer.OrdinalIgnoreCase);
    public bool HasAnyRole(params string[] roles) => roles.Any(HasRole);
    public bool HasAnyPermission(params string[] permissions) => permissions.Any(HasPermission);
}

/// <summary>
/// Token generation and validation service
/// </summary>
public interface ITokenService
{
    string GenerateToken(TokenClaims tokenClaims, TimeSpan? expirationTime = null);
    bool ValidateToken(string token);
    ClaimsPrincipal? GetPrincipalFromToken(string token);
}

/// <summary>
/// Claims for token generation
/// </summary>
public class TokenClaims
{
    public string UserId { get; set; } = string.Empty;
    public string? EmployeeNumber { get; set; }
    public string? Email { get; set; }
    public string? FullName { get; set; }
    public List<string> Roles { get; set; } = new();
    public List<string> Permissions { get; set; } = new();
    public Dictionary<string, string> AdditionalClaims { get; set; } = new();
}

/// <summary>
/// Authorization policy definitions
/// </summary>
public static class AuthorizationPolicies
{
    public const string AdminOnly = "AdminOnly";
    public const string DoctorOnly = "DoctorOnly";
    public const string EmployeeOnly = "EmployeeOnly";
    public const string ManagerOnly = "ManagerOnly";
    public const string ViewOwnDataOnly = "ViewOwnDataOnly";
    public const string ModifyOwnDataOnly = "ModifyOwnDataOnly";
    public const string HealthServicesSuperAdmin = "HealthServicesSuperAdmin";

    /// <summary>
    /// Add health services authorization policies
    /// </summary>
    public static void AddHealthServicesAuthorizationPolicies(
        this Microsoft.AspNetCore.Authorization.AuthorizationBuilder builder)
    {
        builder.AddPolicy(AdminOnly, policy =>
            policy.RequireRole("Admin", "SuperAdmin"));

        builder.AddPolicy(DoctorOnly, policy =>
            policy.RequireRole("Doctor"));

        builder.AddPolicy(EmployeeOnly, policy =>
            policy.RequireRole("Employee", "Doctor", "Admin"));

        builder.AddPolicy(ManagerOnly, policy =>
            policy.RequireRole("Manager", "Admin"));

        builder.AddPolicy(ViewOwnDataOnly, policy =>
            policy.Requirements.Add(new SameUserRequirement()));

        builder.AddPolicy(ModifyOwnDataOnly, policy =>
        {
            policy.Requirements.Add(new SameUserRequirement());
            policy.Requirements.Add(new HasPermissionRequirement("modify:data"));
        });

        builder.AddPolicy(HealthServicesSuperAdmin, policy =>
            policy.RequireRole("SuperAdmin")
                  .RequireClaim("service", "HealthServices"));
    }
}

/// <summary>
/// Authorization requirement for same user validation
/// </summary>
public class SameUserRequirement : Microsoft.AspNetCore.Authorization.IAuthorizationRequirement
{
}

/// <summary>
/// Authorization requirement for permission validation
/// </summary>
public class HasPermissionRequirement : Microsoft.AspNetCore.Authorization.IAuthorizationRequirement
{
    public string Permission { get; }

    public HasPermissionRequirement(string permission)
    {
        Permission = permission;
    }
}

/// <summary>
/// Email verification service
/// </summary>
public interface IEmailVerificationService
{
    Task<bool> SendVerificationEmailAsync(string email, string verificationCode);
    Task<bool> VerifyEmailAsync(string email, string verificationCode);
}

/// <summary>
/// Password hashing service
/// </summary>
public interface IPasswordHashService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
}
