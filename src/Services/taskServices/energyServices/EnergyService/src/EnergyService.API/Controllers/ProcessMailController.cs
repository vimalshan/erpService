using EnergyService.Application.DTOs;
using EnergyService.Application.Features.ProcessMail.Commands.ConfigureMailId;
using EnergyService.Application.Features.ProcessMail.Queries.GetMailIdsByProcess;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnergyService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProcessMailController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProcessMailController(IMediator mediator) => _mediator = mediator;

    [HttpGet("process/{processId:int}")]
    public async Task<ActionResult<IReadOnlyList<EcProcessMailIdDto>>> GetByProcess(int processId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetMailIdsByProcessQuery(processId), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<EcProcessMailIdDto>> Configure([FromBody] ConfigureMailIdCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }
}
