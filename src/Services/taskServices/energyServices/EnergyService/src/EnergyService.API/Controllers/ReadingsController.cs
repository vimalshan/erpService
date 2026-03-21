using EnergyService.Application.DTOs;
using EnergyService.Application.Features.Readings.Commands.InsertReading;
using EnergyService.Application.Features.Readings.Queries.GetReadingById;
using EnergyService.Application.Features.Readings.Queries.GetReadingsByProcess;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnergyService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReadingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReadingsController(IMediator mediator) => _mediator = mediator;

    [HttpGet("process/{processId:int}")]
    public async Task<ActionResult<IReadOnlyList<EcReadingDto>>> GetByProcess(int processId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetReadingsByProcessQuery(processId), ct);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EcReadingDto>> GetById(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetReadingByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<EcReadingDto>> Insert([FromBody] InsertReadingCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.EbId }, result);
    }
}
