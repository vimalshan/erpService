using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EmployeeRelations.Application.Commands.Ews;
using EmployeeRelations.Application.Queries.Ews;
using EmployeeRelations.Application.DTOs;

namespace EmployeeRelations.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class EwsController : ControllerBase
{
    private readonly IMediator _mediator;
    public EwsController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get EWS record by Id.</summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(EwsMainDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct) =>
        Ok(await _mediator.Send(new GetEwsByIdQuery(id), ct));

    /// <summary>Get EWS records by employee.</summary>
    [HttpGet("employee/{empSysId:long}")]
    [ProducesResponseType(typeof(IEnumerable<EwsMainDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEmployee(long empSysId, CancellationToken ct) =>
        Ok(await _mediator.Send(new GetEwsByEmpQuery(empSysId), ct));

    /// <summary>Get EWS records by period.</summary>
    [HttpGet("period/{periodNo:int}")]
    [ProducesResponseType(typeof(IEnumerable<EwsMainDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByPeriod(int periodNo, CancellationToken ct) =>
        Ok(await _mediator.Send(new GetEwsByPeriodQuery(periodNo), ct));

    /// <summary>Create a new EWS record.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(EwsMainDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateEwsCommand cmd, CancellationToken ct)
    {
        var result = await _mediator.Send(cmd, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Record HR input for an EWS record.</summary>
    [HttpPut("{ewsId:long}/hr-input")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RecordHrInput(long ewsId, [FromBody] RecordEwsHrInputCommand cmd, CancellationToken ct)
    {
        await _mediator.Send(cmd with { EwsId = ewsId }, ct);
        return NoContent();
    }
}
