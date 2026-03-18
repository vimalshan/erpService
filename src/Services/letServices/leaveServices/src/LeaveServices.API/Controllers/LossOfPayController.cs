using LeaveServices.Application.Features.LossOfPay.Commands;
using LeaveServices.Application.Features.LossOfPay.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeaveServices.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class LossOfPayController : ControllerBase
{
    private readonly IMediator _mediator;
    public LossOfPayController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get all LOP records for an employee.</summary>
    [HttpGet("employee/{empSysId:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEmployee(long empSysId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetLossOfPayByEmployeeQuery(empSysId), ct);
        return Ok(result);
    }

    /// <summary>Record a loss of pay entry.</summary>
    [HttpPost]
    [Authorize(Roles = "Manager,Admin,HR")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Record([FromBody] RecordLossOfPayCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return StatusCode(StatusCodes.Status201Created, result);
    }
}
