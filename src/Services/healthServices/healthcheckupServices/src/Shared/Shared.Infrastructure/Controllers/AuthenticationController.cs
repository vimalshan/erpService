using Microsoft.AspNetCore.Mvc;
using Shared.Infrastructure.Authentication;

namespace Shared.Infrastructure.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IJwtTokenService _tokenService;

        public AuthenticationController(IJwtTokenService tokenService)
        {
            _tokenService = tokenService;
        }

        /// <summary>
        /// Generate JWT token for testing
        /// </summary>
        [HttpPost("login")]
        public ActionResult<LoginResponse> Login([FromBody] LoginRequest request)
        {
            // In production, validate credentials against a database
            // For testing, accept any credentials
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Email and password are required");

            // Generate token (in production, validate actual credentials)
            var token = _tokenService.GenerateToken(
                userId: request.Email,
                email: request.Email,
                role: request.Role ?? "User"
            );

            return Ok(new LoginResponse
            {
                Token = token,
                ExpiresIn = 3600,
                TokenType = "Bearer"
            });
        }

        /// <summary>
        /// Service-to-service authentication
        /// </summary>
        [HttpPost("service-login")]
        public ActionResult<LoginResponse> ServiceLogin([FromBody] ServiceLoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ServiceName) || string.IsNullOrWhiteSpace(request.ServiceKey))
                return BadRequest("ServiceName and ServiceKey are required");

            // In production, validate service key against a database
            // For testing, accept any key
            var claims = new Dictionary<string, object>
            {
                { "service", "true" },
                { "serviceName", request.ServiceName }
            };

            // Create a token for service-to-service communication
            var token = _tokenService.GenerateToken(
                userId: request.ServiceName,
                email: $"{request.ServiceName}@service.local",
                role: "Service"
            );

            return Ok(new LoginResponse
            {
                Token = token,
                ExpiresIn = 3600,
                TokenType = "Bearer"
            });
        }

        /// <summary>
        /// Verify token validity
        /// </summary>
        [HttpPost("verify")]
        public ActionResult<VerifyResponse> VerifyToken([FromBody] VerifyRequest request)
        {
            var principal = _tokenService.ValidateToken(request.Token);

            if (principal == null)
                return BadRequest(new VerifyResponse { IsValid = false });

            return Ok(new VerifyResponse
            {
                IsValid = true,
                UserId = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,
                Email = principal.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value,
                Role = principal.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value
            });
        }
    }

    public class LoginRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? Role { get; set; } = "User";
    }

    public class ServiceLoginRequest
    {
        public required string ServiceName { get; set; }
        public required string ServiceKey { get; set; }
    }

    public class LoginResponse
    {
        public required string Token { get; set; }
        public int ExpiresIn { get; set; }
        public string TokenType { get; set; } = "Bearer";
    }

    public class VerifyRequest
    {
        public required string Token { get; set; }
    }

    public class VerifyResponse
    {
        public bool IsValid { get; set; }
        public string? UserId { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
    }
}
