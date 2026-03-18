using CanteenUnit.Application.Features.CanteenMasters.Commands;
using CanteenUnit.Application.Features.CanteenMasters.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CanteenUnit.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class CanteenMastersController : ControllerBase
{
    private readonly IMediator _mediator;
    public CanteenMastersController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _mediator.Send(new GetAllCanteenMastersQuery(), ct));

    [HttpGet("{comCode:decimal}")]
    public async Task<IActionResult> GetById(decimal comCode, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetCanteenMasterQuery(comCode), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCanteenMasterCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { comCode = result.CnComCod }, result);
    }

    [HttpPatch("{comCode:decimal}/live")]
    public async Task<IActionResult> SetLiveFlag(decimal comCode, [FromQuery] char flag, CancellationToken ct)
    {
        await _mediator.Send(new UpdateCanteenMasterLiveFlagCommand(comCode, flag), ct);
        return NoContent();
    }

    [HttpDelete("{comCode:decimal}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(decimal comCode, CancellationToken ct)
    {
        await _mediator.Send(new DeleteCanteenMasterCommand(comCode), ct);
        return NoContent();
    }
}
