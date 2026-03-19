using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurityService.Application.DTOs;
using SecurityService.Application.Queries;

namespace SecurityService.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public sealed class MenusController : ControllerBase
{
    private readonly IMediator _mediator;
    public MenusController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get all menus.</summary>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<MenuDto>>> GetAll(CancellationToken ct = default)
        => Ok(await _mediator.Send(new GetAllMenusQuery(), ct));

    /// <summary>Get menus accessible by a role.</summary>
    [HttpGet("role/{roleId:long}")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<MenuDto>>> GetByRole(long roleId, CancellationToken ct = default)
        => Ok(await _mediator.Send(new GetMenusByRoleQuery(roleId), ct));
}
