namespace AccessService.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AccessService.API.Authentication;
using AccessService.Infrastructure.Repositories;

/// <summary>
/// Authentication Controller for login and token management
/// </summary>
[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AuthController> _logger;

    public AuthController(ITokenService tokenService, IUnitOfWork unitOfWork, ILogger<AuthController> logger)
    {
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Login with employee system ID
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<AuthenticationResponse>> Login([FromBody] LoginRequest request)
    {
        if (request.EmployeeSystemId <= 0)
            return BadRequest("Invalid employee system ID");

        // Get user roles from database
        var userRoles = await _unitOfWork.UserRoles.GetRolesByEmployeeIdAsync(request.EmployeeSystemId);
        var roles = userRoles
            .Where(r => r.IsActive())
            .Select(r => r.GetRoleTypeDescription())
            .Distinct()
            .ToArray();

        var token = _tokenService.GenerateToken(request.EmployeeSystemId, request.Email, roles);

        _logger.LogInformation($"User {request.EmployeeSystemId} logged in successfully");

        return Ok(new AuthenticationResponse
        {
            EmployeeSystemId = request.EmployeeSystemId,
            Email = request.Email,
            AccessToken = token,
            ExpiresIn = 3600, // 1 hour in seconds
            Roles = roles
        });
    }

    /// <summary>
    /// Verify current token validity
    /// </summary>
    [HttpPost("verify")]
    [Authorize]
    public ActionResult VerifyToken()
    {
        var employeeId = User.FindFirst("EmployeeId")?.Value;
        return Ok(new { message = "Token is valid", employeeId });
    }

    /// <summary>
    /// Get current user info
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    public ActionResult GetCurrentUser()
    {
        var employeeId = User.FindFirst("EmployeeId")?.Value;
        var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        var roles = User.FindAll(System.Security.Claims.ClaimTypes.Role).Select(c => c.Value).ToList();

        return Ok(new
        {
            employeeId,
            email,
            roles
        });
    }
}
