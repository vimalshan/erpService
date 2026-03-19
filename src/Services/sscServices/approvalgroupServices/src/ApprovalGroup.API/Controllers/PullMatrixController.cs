using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApprovalGroup.Application.PullMatrix.Commands;
using ApprovalGroup.Application.PullMatrix.Queries;
using ApprovalGroup.Application.DTOs;

namespace ApprovalGroup.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class PullMatrixController : ControllerBase
{
    private readonly IMediator _mediator;

    public PullMatrixController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{matId:long}")]
    [ProducesResponseType(typeof(PullMatrixDetailDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(long matId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetPullMatrixByIdQuery(matId), ct));

    [HttpGet("unit/{unitId:long}")]
    [ProducesResponseType(typeof(IEnumerable<PullMatrixDetailDto>), 200)]
    public async Task<IActionResult> GetByUnitId(long unitId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetPullMatrixByUnitIdQuery(unitId), ct));

    [HttpPost]
    [ProducesResponseType(typeof(PullMatrixDetailDto), 201)]
    public async Task<IActionResult> Create([FromBody] CreatePullMatrixCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { matId = result.MatId }, result);
    }

    [HttpPut("{matId:long}")]
    [ProducesResponseType(typeof(PullMatrixDetailDto), 200)]
    public async Task<IActionResult> Update(long matId, [FromBody] UpdatePullMatrixCommand command, CancellationToken ct)
    {
        if (matId != command.MatId) return BadRequest("ID mismatch.");
        return Ok(await _mediator.Send(command, ct));
    }
}
