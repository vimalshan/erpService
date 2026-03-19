using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Commands;
using UserService.Application.DTOs;
using UserService.Application.Queries;

namespace UserService.API.Controllers;

/// <summary>
/// User REST API Controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IMediator mediator, ILogger<UsersController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Create a new user
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<long>> CreateUser(CreateUserRequest request)
    {
        _logger.LogInformation("Creating user: {UserName}", request.UserName);

        var command = new CreateUserCommand
        {
            UserName = request.UserName,
            Password = request.Password,
            EmailId = request.EmailId,
            EnteredBy = request.EnteredBy,
            SparchUserId = request.SparchUserId,
            HrEmpSysId = request.HrEmpSysId
        };

        var userId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetUserById), new { id = userId }, userId);
    }

    /// <summary>
    /// Get user by ID
    /// </summary>
    [HttpGet("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> GetUserById(long id)
    {
        _logger.LogInformation("Getting user: {UserId}", id);

        var query = new GetUserByIdQuery { UserId = id };
        var user = await _mediator.Send(query);

        if (user == null)
            return NotFound();

        return Ok(user);
    }

    /// <summary>
    /// Get user by email
    /// </summary>
    [HttpGet("email/{email}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> GetUserByEmail(string email)
    {
        _logger.LogInformation("Getting user by email: {Email}", email);

        var query = new GetUserByEmailQuery { Email = email };
        var user = await _mediator.Send(query);

        if (user == null)
            return NotFound();

        return Ok(user);
    }

    /// <summary>
    /// Get all users
    /// </summary>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
    {
        _logger.LogInformation("Getting all users");

        var query = new GetAllUsersQuery();
        var users = await _mediator.Send(query);

        return Ok(users);
    }

    /// <summary>
    /// Get active users
    /// </summary>
    [HttpGet("active")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetActiveUsers()
    {
        _logger.LogInformation("Getting active users");

        var query = new GetActiveUsersQuery();
        var users = await _mediator.Send(query);

        return Ok(users);
    }

    /// <summary>
    /// Update user
    /// </summary>
    [HttpPut("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUser(long id, UpdateUserRequest request)
    {
        _logger.LogInformation("Updating user: {UserId}", id);

        var command = new UpdateUserCommand
        {
            UserId = id,
            UserName = request.UserName,
            EmailId = request.EmailId,
            SparchUserId = request.SparchUserId
        };

        var result = await _mediator.Send(command);

        if (!result)
            return NotFound();

        return NoContent();
    }

    /// <summary>
    /// Deactivate user
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeactivateUser(long id)
    {
        _logger.LogInformation("Deactivating user: {UserId}", id);

        var command = new DeactivateUserCommand { UserId = id };
        var result = await _mediator.Send(command);

        if (!result)
            return NotFound();

        return NoContent();
    }

    /// <summary>
    /// Assign role to user
    /// </summary>
    [HttpPost("{id}/roles")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignRole(long id, AssignRoleRequest request)
    {
        _logger.LogInformation("Assigning role {RoleId} to user {UserId}", request.RoleId, id);

        var command = new AssignRoleToUserCommand
        {
            UserId = id,
            RoleId = request.RoleId,
            IsDefault = request.IsDefault
        };

        var result = await _mediator.Send(command);

        if (!result)
            return NotFound();

        return NoContent();
    }

    /// <summary>
    /// Assign organization to user
    /// </summary>
    [HttpPost("{id}/organizations")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignOrganization(long id, AssignOrganizationRequest request)
    {
        _logger.LogInformation("Assigning organization {BusinessUnitId} to user {UserId}", request.BusinessUnitId, id);

        var command = new AssignOrganizationToUserCommand
        {
            UserId = id,
            BusinessUnitId = request.BusinessUnitId
        };

        var result = await _mediator.Send(command);

        if (!result)
            return NotFound();

        return NoContent();
    }

    /// <summary>
    /// Assign location to user
    /// </summary>
    [HttpPost("{id}/locations")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignLocation(long id, AssignLocationRequest request)
    {
        _logger.LogInformation("Assigning location {LocationId} to user {UserId}", request.LocationId, id);

        var command = new AssignLocationToUserCommand
        {
            UserId = id,
            LocationId = request.LocationId
        };

        var result = await _mediator.Send(command);

        if (!result)
            return NotFound();

        return NoContent();
    }
}

/// <summary>
/// Authentication Controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IMediator mediator, ILogger<AuthController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Login user
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        _logger.LogInformation("Login attempt for user: {Email}", request.UserEmail);

        var command = new LoginUserCommand
        {
            Email = request.UserEmail,
            Password = request.Password
        };

        try
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Invalid email or password");
        }
    }
}
