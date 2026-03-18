using LoanDefinition.Application.DTOs;
using LoanDefinition.Application.Features.Festivals.Commands;
using LoanDefinition.Application.Features.Festivals.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanDefinition.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FestivalsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<LoanFestivalDto>>> GetAll()
    {
        var result = await mediator.Send(new GetAllFestivalsQuery());
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    [AllowAnonymous]
    public async Task<ActionResult<LoanFestivalDto>> GetById(long id)
    {
        var result = await mediator.Send(new GetFestivalByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("active")]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<LoanFestivalDto>>> GetActive([FromQuery] DateTime? asOfDate)
    {
        var result = await mediator.Send(new GetActiveFestivalsQuery(asOfDate ?? DateTime.UtcNow));
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<LoanFestivalDto>> Create([FromBody] CreateFestivalCommand command)
    {
        var result = await mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.FestivalId }, result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<LoanFestivalDto>> Update(long id, [FromBody] UpdateFestivalCommand command)
    {
        if (id != command.FestivalId) return BadRequest("ID mismatch");
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await mediator.Send(new DeleteFestivalCommand(id));
        return result ? NoContent() : NotFound();
    }
}
