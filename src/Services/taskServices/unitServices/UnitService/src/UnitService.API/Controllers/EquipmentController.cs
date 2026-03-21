using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnitService.Application.Commands.RegisterEquipment;
using UnitService.Application.Commands.UpdateEquipmentStatus;
using UnitService.Application.DTOs;
using UnitService.Application.Queries.GetAllEquipment;
using UnitService.Application.Queries.GetEquipment;
using UnitService.Application.Queries.GetEquipmentStatus;

namespace UnitService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EquipmentController : ControllerBase
{
    private readonly IMediator _mediator;

    public EquipmentController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EquipmentDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllEquipmentQuery());
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EquipmentDto>> GetById(int id)
    {
        var result = await _mediator.Send(new GetEquipmentQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("{id:int}/statuses")]
    public async Task<ActionResult<IEnumerable<EquipmentStatusDto>>> GetStatuses(int id)
    {
        var result = await _mediator.Send(new GetEquipmentStatusQuery(id));
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Register([FromBody] RegisterEquipmentCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result }, result);
    }

    [HttpPost("{id:int}/status")]
    public async Task<ActionResult<int>> UpdateStatus(int id, [FromBody] UpdateEquipmentStatusCommand command)
    {
        if (id != command.EquipmentId)
            return BadRequest("Equipment ID mismatch.");

        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
