using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;

namespace Services.AuthProvider.Authentication
{
    /// <summary>
    /// JWT Token generation and validation service interface
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Generate JWT token for authenticated user
        /// </summary>
        string GenerateToken(AuthUser user);

        /// <summary>
        /// Validate and parse JWT token
        /// </summary>
        ClaimsPrincipal? ValidateToken(string token);

        /// <summary>
        /// Refresh expired token
        /// </summary>
        string RefreshToken(string token);

        /// <summary>
        /// Authenticate user with username and password
        /// </summary>
        Task<AuthToken?> AuthenticateAsync(string username, string password, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// User model for authentication
    /// </summary>
    public class AuthUser
    {
        public long Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
    }

    /// <summary>
    /// Token response model
    /// </summary>
    public class AuthToken
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public int ExpiresIn { get; set; }
    }

    /// <summary>
    /// JWT Authentication Service Implementation
    /// Uses System.IdentityModel.Tokens.Jwt for real JWT token generation
    /// </summary>
    public class JwtAuthService : IAuthService
    {
        private readonly ILogger<JwtAuthService> _logger;
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expirationMinutes;

        /// <summary>
        /// Constructor requires JWT configuration
        /// </summary>
        public JwtAuthService(
            string secretKey,
            string issuer,
            string audience,
            int expirationMinutes,
            ILogger<JwtAuthService> logger)
        {
            if (string.IsNullOrEmpty(secretKey) || secretKey.Length < 32)
                throw new ArgumentException("Secret key must be at least 32 characters long", nameof(secretKey));

            _secretKey = secretKey;
            _issuer = issuer;
            _audience = audience;
            _expirationMinutes = expirationMinutes;
            _logger = logger;
        }

        /// <summary>
        /// Generate JWT token for user
        /// Creates a signed token with user claims that expires in configured time
        /// </summary>
        public string GenerateToken(AuthUser user)
        {
            try
            {
                _logger.LogInformation(
                    "Generating JWT token for user: {Username} (ID: {UserId})",
                    user.Username,
                    user.Id);

                // Create claims from user information
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                };

                // Add role claims
                foreach (var role in user.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                // Token expiration
                var expiresAt = DateTime.UtcNow.AddMinutes(_expirationMinutes);

                // Create signing key from secret
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // Create token descriptor
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = expiresAt,
                    Issuer = _issuer,
                    Audience = _audience,
                    SigningCredentials = credentials
                };

                // Generate token
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);

                _logger.LogInformation(
                    "JWT token generated successfully for user: {Username}, expires at: {ExpiresAt}",
                    user.Username,
                    expiresAt);

                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating JWT token for user: {Username}", user.Username);
                throw;
            }
        }

        /// <summary>
        /// Validate JWT token and return ClaimsPrincipal
        /// Verifies signature, issuer, audience, and expiration
        /// </summary>
        public ClaimsPrincipal? ValidateToken(string token)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(token))
                {
                    _logger.LogWarning("Token validation failed: token is null or empty");
                    return null;
                }

                _logger.LogInformation("Validating JWT token");

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_secretKey);

                try
                {
                    var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidIssuer = _issuer,
                        ValidateAudience = true,
                        ValidAudience = _audience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero // No tolerance for expiration
                    }, out SecurityToken validatedToken);

                    _logger.LogInformation("JWT token validated successfully");
                    return principal;
                }
                catch (SecurityTokenExpiredException ex)
                {
                    _logger.LogWarning("Token validation failed: token has expired");
                    throw;
                }
                catch (SecurityTokenInvalidSignatureException ex)
                {
                    _logger.LogWarning("Token validation failed: invalid signature");
                    throw;
                }
                catch (SecurityTokenException ex)
                {
                    _logger.LogWarning("Token validation failed: {Message}", ex.Message);
                    throw;
                }
            }
            catch (SecurityTokenExpiredException)
            {
                _logger.LogWarning("Token validation failed: token expired");
                return null;
            }
            catch (SecurityTokenException ex)
            {
                _logger.LogWarning("Token validation failed: {Message}", ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error validating JWT token");
                return null;
            }
        }

        /// <summary>
        /// Refresh token by validating and regenerating
        /// Validates old token (ignoring expiration) and issues new token
        /// </summary>
        public string RefreshToken(string token)
        {
            try
            {
                _logger.LogInformation("Refreshing JWT token");

                if (string.IsNullOrWhiteSpace(token))
                    throw new ArgumentException("Token cannot be null or empty", nameof(token));

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_secretKey);

                // Validate without checking expiration
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = true,
                    ValidAudience = _audience,
                    ValidateLifetime = false // Allow expired tokens for refresh
                }, out SecurityToken validatedToken);

                // Extract claims from old token
                var claims = principal.Claims.ToList();

                // Create new token with same claims
                var expiresAt = DateTime.UtcNow.AddMinutes(_expirationMinutes);
                var credentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = expiresAt,
                    Issuer = _issuer,
                    Audience = _audience,
                    SigningCredentials = credentials
                };

                var newToken = tokenHandler.CreateToken(tokenDescriptor);
                var refreshedToken = tokenHandler.WriteToken(newToken);

                _logger.LogInformation("Token refreshed successfully, new expiration: {ExpiresAt}", expiresAt);
                return refreshedToken;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing JWT token");
                throw;
            }
        }

        /// <summary>
        /// Authenticate user with username and password
        /// Verifies credentials and returns JWT token if valid
        /// </summary>
        public async Task<AuthToken?> AuthenticateAsync(
            string username,
            string password,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    _logger.LogWarning("Authentication attempt with empty username or password");
                    return null;
                }

                _logger.LogInformation("Authenticating user: {Username}", username);

                // Verify credentials against database
                var user = await VerifyCredentialsAsync(username, password, cancellationToken);
                if (user == null)
                {
                    _logger.LogWarning("Authentication failed: invalid credentials for user {Username}", username);
                    return null;
                }

                // Generate tokens
                var accessToken = GenerateToken(user);
                var refreshToken = GenerateRefreshToken(user);
                var expiresAt = DateTime.UtcNow.AddMinutes(_expirationMinutes);

                _logger.LogInformation(
                    "User {Username} authenticated successfully, expires at: {ExpiresAt}",
                    username,
                    expiresAt);

                return new AuthToken
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresAt = expiresAt,
                    ExpiresIn = _expirationMinutes * 60 // Convert to seconds
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error authenticating user: {Username}", username);
                return null;
            }
        }

        /// <summary>
        /// Generate refresh token (longer expiration)
        /// Refresh tokens typically expire in 7-30 days
        /// </summary>
        private string GenerateRefreshToken(AuthUser user)
        {
            try
            {
                _logger.LogInformation("Generating refresh token for user: {Username}", user.Username);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim("token_type", "refresh"),
                };

                // Refresh token expires in 7 days
                var expiresAt = DateTime.UtcNow.AddDays(7);
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = expiresAt,
                    Issuer = _issuer,
                    Audience = _audience,
                    SigningCredentials = credentials,
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);

                _logger.LogInformation("Refresh token generated for user: {Username}", user.Username);
                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating refresh token for user: {Username}", user.Username);
                throw;
            }
        }

        /// <summary>
        /// Verify user credentials against database
        /// This is a placeholder - implement actual database lookup
        /// </summary>
        private async Task<AuthUser?> VerifyCredentialsAsync(
            string username,
            string password,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Verifying credentials for user: {Username}", username);

                // TODO: Implement actual credential verification
                // 1. Look up user in database by username
                // 2. Hash provided password with stored salt
                // 3. Compare hashes
                // 4. Return AuthUser if match, null if no match

                // Placeholder implementation - REMOVE IN PRODUCTION
                if (username == "admin" && password == "admin123")
                {
                    return new AuthUser
                    {
                        Id = 1,
                        Username = "admin",
                        Email = "admin@example.com",
                        Roles = new List<string> { "Admin" }
                    };
                }

                if (username == "user" && password == "user123")
                {
                    return new AuthUser
                    {
                        Id = 2,
                        Username = "user",
                        Email = "user@example.com",
                        Roles = new List<string> { "User" }
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying credentials for user: {Username}", username);
                return null;
            }
        }
    }
}
