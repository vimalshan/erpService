using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeService.API.Controllers
{
    /// <summary>
    /// Authentication Controller - for testing and token generation
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Generate JWT token for testing (No authentication required)
        /// </summary>
        /// <param name="role">User role: Admin, HR, Manager, Employee</param>
        /// <returns>JWT Bearer token</returns>
        /// <summary>
        /// Generate JWT token for testing
        /// </summary>
        /// <param name="role">User role (Admin, HR, Manager, Employee)</param>
        /// <returns>JWT token response</returns>
        [HttpPost("token")]
        [Produces("application/json")]
        public IActionResult GenerateToken([FromQuery] string role = "Admin")
        {
            try
            {
                var validRoles = new[] { "Admin", "HR", "Manager", "Employee" };
                if (!Array.Exists(validRoles, r => r == role))
                {
                    return BadRequest(new { error = "Invalid role. Use: Admin, HR, Manager, or Employee" });
                }

                var jwtKey = _configuration["Jwt:Key"] ?? "YourSuperSecretKeyChangeThisInProduction12345ShouldBeAtLeast32Chars";
                var jwtIssuer = _configuration["Jwt:Issuer"] ?? "EmployeeService";
                var jwtAudience = _configuration["Jwt:Audience"] ?? "EmployeeServiceAPI";
                var expiryMinutes = 60;
                
                if (int.TryParse(_configuration["Jwt:ExpiryMinutes"], out var configExpiry))
                {
                    expiryMinutes = configExpiry;
                }

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, $"test-user-{Guid.NewGuid().ToString().Substring(0, 8)}"),
                    new Claim(ClaimTypes.Name, $"Test {role}"),
                    new Claim(ClaimTypes.Email, $"test.{role.ToLower()}@example.com"),
                    new Claim(ClaimTypes.Role, role)
                };

                var token = new JwtSecurityToken(
                    issuer: jwtIssuer,
                    audience: jwtAudience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                    signingCredentials: credentials
                );

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenString = tokenHandler.WriteToken(token);

                var response = new
                {
                    token = tokenString,
                    expiresIn = expiryMinutes * 60,
                    tokenType = "Bearer",
                    role = role
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        /// <summary>
        /// Validate JWT token
        /// </summary>
        [HttpPost("validate")]
        public IActionResult ValidateToken([FromBody] ValidateTokenRequest request)
        {
            try
            {
                var jwtKey = _configuration["Jwt:Key"];
                var jwtIssuer = _configuration["Jwt:Issuer"];
                var jwtAudience = _configuration["Jwt:Audience"];

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
                var tokenHandler = new JwtSecurityTokenHandler();

                var principal = tokenHandler.ValidateToken(request.Token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    ValidateIssuer = true,
                    ValidIssuer = jwtIssuer,
                    ValidateAudience = true,
                    ValidAudience = jwtAudience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return Ok(new
                {
                    valid = true,
                    message = "Token is valid",
                    claims = principal.Claims.Select(c => new { c.Type, c.Value })
                });
            }
            catch (SecurityTokenException ex)
            {
                return BadRequest(new { valid = false, error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { valid = false, error = ex.Message });
            }
        }
    }

    /// <summary>
    /// Token response DTO
    /// </summary>
    public class TokenResponse
    {
        public string Token { get; set; }
        public int ExpiresIn { get; set; }
        public string TokenType { get; set; }
        public string Role { get; set; }
    }

    /// <summary>
    /// Token validation request DTO
    /// </summary>
    public class ValidateTokenRequest
    {
        public string Token { get; set; }
    }
}
