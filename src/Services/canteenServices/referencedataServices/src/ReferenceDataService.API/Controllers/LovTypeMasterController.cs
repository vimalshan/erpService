using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReferenceDataService.Application.Commands.CreateLovTypeMaster;
using ReferenceDataService.Application.Commands.DeleteLovTypeMaster;
using ReferenceDataService.Application.Commands.UpdateLovTypeMaster;
using ReferenceDataService.Application.DTOs;
using ReferenceDataService.Application.Queries.GetAllLovTypeMasters;
using ReferenceDataService.Application.Queries.GetLovTypeMasterByCode;

namespace ReferenceDataService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LovTypeMasterController : ControllerBase
{
    private readonly IMediator _mediator;

    public LovTypeMasterController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<LovTypeMasterDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllLovTypeMastersQuery());
        return Ok(result);
    }

    [HttpGet("{lovTypeCode}")]
    [ProducesResponseType(typeof(LovTypeMasterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByCode(string lovTypeCode)
    {
        var result = await _mediator.Send(new GetLovTypeMasterByCodeQuery(lovTypeCode));
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(LovTypeMasterDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateLovTypeMasterCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetByCode), new { lovTypeCode = result.LovTypeCode }, result);
    }

    [HttpPut("{lovTypeCode}")]
    [ProducesResponseType(typeof(LovTypeMasterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(string lovTypeCode, [FromBody] UpdateLovTypeMasterCommand command)
    {
        if (lovTypeCode != command.LovTypeCode)
            return BadRequest("Route lovTypeCode does not match body lovTypeCode.");

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{lovTypeCode}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string lovTypeCode)
    {
        var result = await _mediator.Send(new DeleteLovTypeMasterCommand(lovTypeCode));
        return result ? NoContent() : NotFound();
    }
}
