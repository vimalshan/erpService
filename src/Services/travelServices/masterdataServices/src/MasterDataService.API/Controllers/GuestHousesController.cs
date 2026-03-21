using MasterDataService.Application.Commands.GuestHouse;
using MasterDataService.Application.DTOs;
using MasterDataService.Application.Queries.GuestHouse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MasterDataService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GuestHousesController : ControllerBase
{
    private readonly IMediator _mediator;

    public GuestHousesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<GuestHouseDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllGuestHousesQuery());
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    [AllowAnonymous]
    public async Task<ActionResult<GuestHouseDto>> GetById(long id)
    {
        var result = await _mediator.Send(new GetGuestHouseByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("admin/{adminCode:long}")]
    [AllowAnonymous]
    public async Task<ActionResult<GuestHouseDto>> GetByAdminCode(long adminCode)
    {
        var result = await _mediator.Send(new GetGuestHouseByAdminCodeQuery(adminCode));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<GuestHouseDto>> Create([FromBody] CreateGuestHouseCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<GuestHouseDto>> Update(long id, [FromBody] UpdateGuestHouseCommand command)
    {
        if (id != command.Id) return BadRequest("ID mismatch");
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _mediator.Send(new DeleteGuestHouseCommand(id));
        return NoContent();
    }
}
