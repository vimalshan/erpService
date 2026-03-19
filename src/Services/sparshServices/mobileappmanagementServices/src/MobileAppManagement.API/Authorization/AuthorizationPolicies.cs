using Microsoft.AspNetCore.Authorization;

namespace MobileAppManagement.API.Authorization;

/// <summary>
/// Custom authorization policies for the application
/// </summary>
public static class AuthorizationPolicies
{
    public const string DeviceManagement = "DeviceManagement";
    public const string RegistrationManagement = "RegistrationManagement";
    public const string AdminOnly = "AdminOnly";
    public const string ManagerOrAdmin = "ManagerOrAdmin";
    
    /// <summary>
    /// Register custom authorization policies
    /// </summary>
    public static void AddCustomAuthorizationPolicies(this AuthorizationBuilder builder)
    {
        // Device management - Managers and Admins only
        builder.AddPolicy(DeviceManagement, policy =>
            policy.RequireRole(AppRoles.Admin, AppRoles.Manager));
            
        // Registration management - Managers and Admins only
        builder.AddPolicy(RegistrationManagement, policy =>
            policy.RequireRole(AppRoles.Admin, AppRoles.Manager));
            
        // Admin only operations
        builder.AddPolicy(AdminOnly, policy =>
            policy.RequireRole(AppRoles.Admin));
        
        // Manager or Admin
        builder.AddPolicy(ManagerOrAdmin, policy =>
            policy.RequireRole(AppRoles.Manager, AppRoles.Admin));
    }
}
