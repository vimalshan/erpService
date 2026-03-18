using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LovService.Application.DTOs;
using LovService.Application.Features.ProgramLovMast.Commands;
using LovService.Application.Features.ProgramLovMast.Queries;

namespace LovService.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
[Produces("application/json")]
public class ProgramLovController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IEnumerable<ProgramLovMastDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] string? prlovTypeCode, CancellationToken ct)
        => Ok(await mediator.Send(new GetAllProgramLovsQuery(prlovTypeCode), ct));

    [HttpGet("{typeCode}/{code}")]
    [ProducesResponseType<ProgramLovMastDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(string typeCode, string code, CancellationToken ct)
    {
        var result = await mediator.Send(new GetProgramLovByIdQuery(typeCode, code), ct);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [ProducesResponseType<ProgramLovMastDto>(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateProgramLovCommand cmd, CancellationToken ct)
    {
        var result = await mediator.Send(cmd, ct);
        return CreatedAtAction(nameof(GetById),
            new { typeCode = result.PrlovTypeCode, code = result.PrlovCode }, result);
    }

    [HttpPut("{typeCode}/{code}")]
    [ProducesResponseType<ProgramLovMastDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(string typeCode, string code,
        [FromBody] UpdateProgramLovCommand cmd, CancellationToken ct)
    {
        if (typeCode != cmd.PrlovTypeCode || code != cmd.PrlovCode) return BadRequest();
        return Ok(await mediator.Send(cmd, ct));
    }

    [HttpDelete("{typeCode}/{code}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string typeCode, string code, CancellationToken ct)
    {
        var deleted = await mediator.Send(new DeleteProgramLovCommand(typeCode, code), ct);
        return deleted ? NoContent() : NotFound();
    }
}
