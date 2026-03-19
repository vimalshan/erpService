using MediatR;
using MenuAndSecurityService.Application.Commands.CreateMenu;
using MenuAndSecurityService.Application.Commands.DeleteMenu;
using MenuAndSecurityService.Application.Commands.UpdateMenu;
using MenuAndSecurityService.Application.DTOs;
using MenuAndSecurityService.Application.Queries.GetAllMenus;
using MenuAndSecurityService.Application.Queries.GetMenuById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MenuAndSecurityService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MenuController : ControllerBase
{
    private readonly IMediator _mediator;

    public MenuController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MenuMasterDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllMenusQuery());
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<MenuMasterDto>> GetById(long id)
    {
        var result = await _mediator.Send(new GetMenuByIdQuery(id));
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<MenuMasterDto>> Create([FromBody] CreateMenuCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.MenuId }, result);
    }

    [HttpPut("{id:long}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<MenuMasterDto>> Update(long id, [FromBody] UpdateMenuCommand command)
    {
        if (id != command.MenuId)
            return BadRequest("Menu ID mismatch");

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await _mediator.Send(new DeleteMenuCommand(id));
        if (!result) return NotFound();
        return NoContent();
    }
}
