namespace AccidentManagementService.Infrastructure.Authentication;

/// <summary>
/// JWT Authentication configuration settings
/// Maps from appsettings.json -> Authentication section
/// </summary>
public class AuthSettings
{
    /// <summary>
    /// Authority/Issuer URL (e.g., https://auth.yourdomain.com)
    /// Used to validate JWT token issuer
    /// </summary>
    public string Authority { get; set; } = string.Empty;

    /// <summary>
    /// Audience for JWT tokens (e.g., accident-management-api)
    /// Used to validate JWT token audience
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Authentication scheme (default: Bearer)
    /// </summary>
    public string Scheme { get; set; } = "Bearer";

    /// <summary>
    /// Issuer URL (optional, if different from Authority)
    /// </summary>
    public string? Issuer { get; set; }

    /// <summary>
    /// Secret key for symmetric algorithms (if not using authority)
    /// </summary>
    public string? SecretKey { get; set; }

    /// <summary>
    /// Token lifetime in minutes (for refresh token logic)
    /// </summary>
    public int TokenLifetimeMinutes { get; set; } = 60;

    /// <summary>
    /// Refresh token lifetime in days
    /// </summary>
    public int RefreshTokenLifetimeDays { get; set; } = 7;

    /// <summary>
    /// Enable/disable token validation
    /// </summary>
    public bool ValidateToken { get; set; } = true;

    /// <summary>
    /// Clock skew tolerance in seconds
    /// </summary>
    public int ClockSkewSeconds { get; set; } = 0;
}
