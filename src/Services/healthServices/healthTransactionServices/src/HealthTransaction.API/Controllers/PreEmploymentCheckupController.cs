using HealthTransaction.Application.DTOs;
using HealthTransaction.Application.Features.PreEmploymentCheckups.Commands.Create;
using HealthTransaction.Application.Features.PreEmploymentCheckups.Queries.GetAll;
using HealthTransaction.Application.Features.PreEmploymentCheckups.Queries.GetByDateRange;
using HealthTransaction.Application.Features.PreEmploymentCheckups.Queries.GetByEmployeeNum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthTransaction.API.Controllers;

[ApiController]
[Route("api/pre-employment")]
[Authorize]
public class PreEmploymentCheckupController : ControllerBase
{
    private readonly IMediator _mediator;
    public PreEmploymentCheckupController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PreEmploymentCheckupDto>>> GetAll(CancellationToken ct)
        => Ok(await _mediator.Send(new GetAllPreEmploymentCheckupsQuery(), ct));

    [HttpGet("by-employee/{empNum}")]
    public async Task<ActionResult<IReadOnlyList<PreEmploymentCheckupDto>>> GetByEmployee(decimal empNum, CancellationToken ct)
        => Ok(await _mediator.Send(new GetPreEmploymentCheckupsByEmployeeNumQuery(empNum), ct));

    [HttpGet("by-date-range")]
    public async Task<ActionResult<IReadOnlyList<PreEmploymentCheckupDto>>> GetByDateRange(
        [FromQuery] DateTime from, [FromQuery] DateTime to, CancellationToken ct)
        => Ok(await _mediator.Send(new GetPreEmploymentCheckupsByDateRangeQuery(from, to), ct));

    [HttpPost]
    public async Task<ActionResult<PreEmploymentCheckupDto>> Create([FromBody] CreatePreEmploymentCheckupDto dto, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreatePreEmploymentCheckupCommand(dto), ct);
        return CreatedAtAction(nameof(GetByEmployee), new { empNum = result.EmpNum }, result);
    }
}
