using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserSecurityService.Application.DTOs;
using UserSecurityService.Application.Features.PasswordChange.Commands;
using UserSecurityService.Application.Features.UserProfile.Commands;
using UserSecurityService.Application.Features.UserProfile.Queries;
using UserSecurityService.Infrastructure.Dapper;

namespace UserSecurityService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserProfileController(IMediator mediator, DapperUserRepository dapperRepo) : ControllerBase
{
    /// <summary>Returns all active user profiles.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserProfileDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UserProfileDto>>> GetAll(CancellationToken ct)
        => Ok(await mediator.Send(new GetAllActiveUsersQuery(), ct));

    /// <summary>Returns a single user profile by ID.</summary>
    [HttpGet("{userId}")]
    [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserProfileDto>> GetById(string userId, CancellationToken ct)
    {
        var profile = await mediator.Send(new GetUserProfileByIdQuery(userId), ct);
        return profile is null ? NotFound() : Ok(profile);
    }

    /// <summary>Searches user profiles with pagination (Dapper).</summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<UserProfileDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UserProfileDto>>> Search(
        [FromQuery] string? name, [FromQuery] string? unit,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
        => Ok(await dapperRepo.SearchUsersAsync(name, unit, pageSize, page, ct));

    /// <summary>Creates a new user profile.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserProfileDto>> Create(
        [FromBody] CreateUserProfileCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { userId = result.UserId }, result);
    }

    /// <summary>Updates an existing user profile.</summary>
    [HttpPut("{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(string userId, [FromBody] UpdateUserProfileCommand command, CancellationToken ct)
    {
        if (userId != command.UserId) return BadRequest("UserId mismatch.");
        await mediator.Send(command, ct);
        return NoContent();
    }

    /// <summary>Deactivates a user.</summary>
    [HttpDelete("{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Deactivate(string userId, CancellationToken ct)
    {
        await mediator.Send(new DeactivateUserCommand(userId), ct);
        return NoContent();
    }

    /// <summary>Changes the password for a user.</summary>
    [HttpPost("{userId}/change-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ChangePassword(
        string userId, [FromBody] ChangePasswordRequest req, CancellationToken ct)
    {
        await mediator.Send(new ChangePasswordCommand(
            userId, req.EmpSysId, req.CurrentPassword, req.NewPassword, req.ChangedBy), ct);
        return NoContent();
    }
}

public record ChangePasswordRequest(decimal EmpSysId, string CurrentPassword, string NewPassword, decimal ChangedBy);
