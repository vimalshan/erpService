using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace Services.AuthProvider.Authorization
{
    /// <summary>
    /// Authorization service for role-based and claim-based authorization
    /// </summary>
    public interface IAuthorizationService
    {
        /// <summary>
        /// Check if user has required role
        /// </summary>
        bool HasRole(ClaimsPrincipal user, string role);

        /// <summary>
        /// Check if user has any of the required roles
        /// </summary>
        bool HasAnyRole(ClaimsPrincipal user, params string[] roles);

        /// <summary>
        /// Check if user has required claim
        /// </summary>
        bool HasClaim(ClaimsPrincipal user, string claimType, string claimValue);

        /// <summary>
        /// Get user ID from claims
        /// </summary>
        long? GetUserId(ClaimsPrincipal user);

        /// <summary>
        /// Get user roles from claims
        /// </summary>
        List<string> GetUserRoles(ClaimsPrincipal user);

        /// <summary>
        /// Authorize resource access
        /// </summary>
        bool CanAccessResource(ClaimsPrincipal user, string resourceId, string action);
    }

    /// <summary>
    /// Authorization Service Implementation
    /// </summary>
    public class AuthorizationService : IAuthorizationService
    {
        private readonly ILogger<AuthorizationService> _logger;

        public AuthorizationService(ILogger<AuthorizationService> logger)
        {
            _logger = logger;
        }

        public bool HasRole(ClaimsPrincipal user, string role)
        {
            try
            {
                if (user == null)
                    return false;

                var hasRole = user.IsInRole(role);
                _logger.LogInformation("Role check: {User} in role {Role}: {Result}", 
                    user.Identity?.Name, role, hasRole);
                return hasRole;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking role: {Role}", role);
                return false;
            }
        }

        public bool HasAnyRole(ClaimsPrincipal user, params string[] roles)
        {
            if (user == null || roles.Length == 0)
                return false;

            return roles.Any(role => user.IsInRole(role));
        }

        public bool HasClaim(ClaimsPrincipal user, string claimType, string claimValue)
        {
            if (user == null)
                return false;

            var claim = user.FindFirst(claimType);
            return claim?.Value == claimValue;
        }

        public long? GetUserId(ClaimsPrincipal user)
        {
            try
            {
                var userIdClaim = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (long.TryParse(userIdClaim, out var userId))
                    return userId;

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user ID from claims");
                return null;
            }
        }

        public List<string> GetUserRoles(ClaimsPrincipal user)
        {
            try
            {
                if (user == null)
                    return new List<string>();

                return user.FindAll(ClaimTypes.Role)
                    .Select(c => c.Value)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user roles from claims");
                return new List<string>();
            }
        }

        public bool CanAccessResource(ClaimsPrincipal user, string resourceId, string action)
        {
            try
            {
                _logger.LogInformation("Checking access for user {User} to resource {Resource} with action {Action}",
                    user?.Identity?.Name, resourceId, action);

                // Implement resource-level authorization logic
                // This could check database, policies, etc.
                
                // Check if user is admin (admin can access everything)
                if (HasRole(user, "Admin"))
                {
                    _logger.LogInformation("Admin access granted");
                    return true;
                }

                // Check specific resource access claims
                var resourceClaim = user?.FindFirst($"resource:{resourceId}:{action}");
                var hasAccess = resourceClaim?.Value == "true";

                if (hasAccess)
                {
                    _logger.LogInformation("Resource access granted");
                }
                else
                {
                    _logger.LogWarning("Resource access denied");
                }

                return hasAccess;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking resource access");
                return false;
            }
        }
    }

    /// <summary>
    /// Authorization policy definitions
    /// </summary>
    public static class AuthorizationPolicies
    {
        public const string AdminOnly = "AdminOnly";
        public const string UserOrAdmin = "UserOrAdmin";
        public const string FinancialYearManager = "FinancialYearManager";
        public const string FinancialYearViewer = "FinancialYearViewer";
    }
}
