using EnergyService.Application.DTOs;
using EnergyService.Application.Features.ProcessAccess.Commands.UpdateProcessAccess;
using EnergyService.Application.Features.ProcessAccess.Queries.GetProcessAccessByProcess;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnergyService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProcessAccessController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProcessAccessController(IMediator mediator) => _mediator = mediator;

    [HttpGet("process/{processId:int}")]
    public async Task<ActionResult<IReadOnlyList<EcProcessAccessDto>>> GetByProcess(int processId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetProcessAccessByProcessQuery(processId), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<EcProcessAccessDto>> Upsert([FromBody] UpdateProcessAccessCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }
}
