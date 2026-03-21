using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurityService.Application.Commands;
using SecurityService.Application.DTOs;
using SecurityService.Application.Queries;

namespace SecurityService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    public AuthController(IMediator mediator) => _mediator = mediator;

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto dto)
    {
        var result = await _mediator.Send(new LoginCommand(dto));
        return Ok(result);
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<UserDto>> Register([FromBody] UserCreateDto dto)
    {
        var result = await _mediator.Send(new CreateUserCommand(dto));
        return CreatedAtAction(nameof(UsersController.GetById), "Users", new { id = result.UserId }, result);
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    public UsersController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<IReadOnlyList<UserDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllUsersQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetById(int id)
    {
        var result = await _mediator.Send(new GetUserByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserDto>> Update([FromBody] UserUpdateDto dto)
    {
        var result = await _mediator.Send(new UpdateUserCommand(dto));
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteUserCommand(id));
        return NoContent();
    }

    [HttpPost("{userId}/roles/{roleId}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> AssignRole(int userId, int roleId)
    {
        await _mediator.Send(new AssignRoleToUserCommand(userId, roleId));
        return NoContent();
    }

    [HttpDelete("{userId}/roles/{roleId}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> RemoveRole(int userId, int roleId)
    {
        await _mediator.Send(new RemoveRoleFromUserCommand(userId, roleId));
        return NoContent();
    }

    [HttpPost("{userId}/deactivate")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Deactivate(int userId)
    {
        await _mediator.Send(new DeactivateUserCommand(userId));
        return NoContent();
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RolesController : ControllerBase
{
    private readonly IMediator _mediator;
    public RolesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RoleDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllRolesQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoleDto>> GetById(int id)
    {
        var result = await _mediator.Send(new GetRoleByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<RoleDto>> Create([FromBody] RoleCreateDto dto)
    {
        var result = await _mediator.Send(new CreateRoleCommand(dto));
        return CreatedAtAction(nameof(GetById), new { id = result.RoleId }, result);
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<RoleDto>> Update([FromBody] RoleUpdateDto dto)
    {
        var result = await _mediator.Send(new UpdateRoleCommand(dto));
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteRoleCommand(id));
        return NoContent();
    }

    [HttpPost("{roleId}/permissions/{permissionId}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> AssignPermission(int roleId, int permissionId)
    {
        await _mediator.Send(new AssignPermissionToRoleCommand(roleId, permissionId));
        return NoContent();
    }

    [HttpDelete("{roleId}/permissions/{permissionId}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> RemovePermission(int roleId, int permissionId)
    {
        await _mediator.Send(new RemovePermissionFromRoleCommand(roleId, permissionId));
        return NoContent();
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PermissionsController : ControllerBase
{
    private readonly IMediator _mediator;
    public PermissionsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PermissionDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllPermissionsQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PermissionDto>> GetById(int id)
    {
        var result = await _mediator.Send(new GetPermissionByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PermissionDto>> Create([FromBody] PermissionCreateDto dto)
    {
        var result = await _mediator.Send(new CreatePermissionCommand(dto));
        return CreatedAtAction(nameof(GetById), new { id = result.PermissionId }, result);
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PermissionDto>> Update([FromBody] PermissionUpdateDto dto)
    {
        var result = await _mediator.Send(new UpdatePermissionCommand(dto));
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(int id)
    {
        await _mediator.Send(new DeletePermissionCommand(id));
        return NoContent();
    }
}
