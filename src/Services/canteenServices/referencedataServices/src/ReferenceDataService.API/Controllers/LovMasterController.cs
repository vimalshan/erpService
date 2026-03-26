using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReferenceDataService.Application.Commands.CreateLovMaster;
using ReferenceDataService.Application.Commands.DeleteLovMaster;
using ReferenceDataService.Application.Commands.UpdateLovMaster;
using ReferenceDataService.Application.DTOs;
using ReferenceDataService.Application.Queries.GetAllLovMasters;
using ReferenceDataService.Application.Queries.GetLovMasterById;

namespace ReferenceDataService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LovMasterController : ControllerBase
{
    private readonly IMediator _mediator;

    public LovMasterController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<LovMasterDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllLovMastersQuery());
        return Ok(result);
    }

    [HttpGet("{lovId}")]
    [ProducesResponseType(typeof(LovMasterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(string lovId)
    {
        var result = await _mediator.Send(new GetLovMasterByIdQuery(lovId));
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(LovMasterDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateLovMasterCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { lovId = result.LovId }, result);
    }

    [HttpPut("{lovId}")]
    [ProducesResponseType(typeof(LovMasterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(string lovId, [FromBody] UpdateLovMasterCommand command)
    {
        if (lovId != command.LovId)
            return BadRequest("Route lovId does not match body lovId.");

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{lovId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string lovId)
    {
        var result = await _mediator.Send(new DeleteLovMasterCommand(lovId));
        return result ? NoContent() : NotFound();
    }
}
