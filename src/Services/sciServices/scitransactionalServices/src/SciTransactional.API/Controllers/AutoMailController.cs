using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SciTransactional.Application.Commands.CreateAutoMail;
using SciTransactional.Application.DTOs;
using SciTransactional.Application.Queries.GetAutoMailStatus;

namespace SciTransactional.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class AutoMailController(IMediator mediator) : ControllerBase
{
    [HttpGet("status")]
    [ProducesResponseType(typeof(IReadOnlyList<AutoMailStatusDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStatus(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAutoMailStatusQuery(), ct);
        return Ok(result);
    }

    [HttpPost("status")]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateAutoMailStatusCommand command, CancellationToken ct)
    {
        var id = await mediator.Send(command, ct);
        return StatusCode(StatusCodes.Status201Created, id);
    }
}
