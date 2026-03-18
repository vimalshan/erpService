using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LeaveServices.Application.Commands.Leave;
using LeaveServices.Application.Queries.Leave;

namespace LeaveServices.API.Controllers;

[ApiController]
[Route("api/leave-master")]
[Authorize]
[Produces("application/json")]
public sealed class LeaveMasterController : ControllerBase
{
    private readonly IMediator _mediator;
    public LeaveMasterController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await _mediator.Send(new GetLeaveMasterQuery(), ct));

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetLeaveMasterByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,HR")]
    public async Task<IActionResult> Create([FromBody] CreateLeaveMasterCommand cmd, CancellationToken ct)
    {
        var id = await _mediator.Send(cmd, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:long}")]
    [Authorize(Roles = "Admin,HR")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateLeaveMasterCommand cmd, CancellationToken ct)
    {
        if (id != cmd.LeaveId) return BadRequest("Route id and payload id mismatch.");
        await _mediator.Send(cmd, ct);
        return NoContent();
    }
}
