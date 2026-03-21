using EnergyService.Application.DTOs;
using EnergyService.Application.Features.Processes.Commands.CreateProcess;
using EnergyService.Application.Features.Processes.Commands.DeleteProcess;
using EnergyService.Application.Features.Processes.Commands.UpdateProcess;
using EnergyService.Application.Features.Processes.Queries.GetAllProcesses;
using EnergyService.Application.Features.Processes.Queries.GetProcessById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnergyService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProcessesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProcessesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<EcProcessDto>>> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllProcessesQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EcProcessDto>> GetById(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetProcessByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<EcProcessDto>> Create([FromBody] CreateProcessCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.EcProcessId }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<EcProcessDto>> Update(int id, [FromBody] UpdateProcessCommand command, CancellationToken ct)
    {
        if (id != command.EcProcessId) return BadRequest("ID mismatch.");
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteProcessCommand(id), ct);
        return result ? NoContent() : NotFound();
    }
}
