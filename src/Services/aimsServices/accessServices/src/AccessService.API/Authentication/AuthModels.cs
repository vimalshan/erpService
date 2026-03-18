namespace AccessService.API.Authentication;

/// <summary>
/// Authentication-related models and DTOs
/// </summary>

public class LoginRequest
{
    public long EmployeeSystemId { get; set; }
    
    public string Email { get; set; } = string.Empty;
    
    public string? Password { get; set; }
}

public class LoginResponse
{
    public bool Success { get; set; }
    
    public string? Token { get; set; }
    
    public string? Message { get; set; }
    
    public TokenInfo? TokenInfo { get; set; }
}

public class TokenInfo
{
    public long EmployeeSystemId { get; set; }
    
    public string Email { get; set; } = string.Empty;
    
    public string[] Roles { get; set; } = Array.Empty<string>();
    
    public DateTime ExpiresAt { get; set; }
}

public class RefreshTokenRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}

public class AuthenticationResponse
{
    public long EmployeeSystemId { get; set; }
    
    public string Email { get; set; } = string.Empty;
    
    public string AccessToken { get; set; } = string.Empty;
    
    public int ExpiresIn { get; set; }
    
    public string[] Roles { get; set; } = Array.Empty<string>();
}
