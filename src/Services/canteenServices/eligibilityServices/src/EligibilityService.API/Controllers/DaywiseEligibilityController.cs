using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using EligibilityService.Application.DTOs;
using EligibilityService.Application.Queries.DaywiseEligibility;
using EligibilityService.Application.Commands.DaywiseEligibility;

namespace EligibilityService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DaywiseEligibilityController : ControllerBase
{
    private readonly IMediator _mediator;

    public DaywiseEligibilityController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{serialNumber:long}")]
    [ProducesResponseType(typeof(DaywiseEligibilityDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long serialNumber, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetDaywiseEligibilityQuery(serialNumber), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("employee/{companyCode:long}/{employeeSysId:long}")]
    [ProducesResponseType(typeof(IEnumerable<DaywiseEligibilityDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEmployee(long companyCode, long employeeSysId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetDaywiseEligibilityByEmployeeQuery(companyCode, employeeSysId), ct);
        return Ok(result);
    }

    [HttpGet("date/{companyCode:long}")]
    [ProducesResponseType(typeof(IEnumerable<DaywiseEligibilityDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByDate(long companyCode, [FromQuery] DateTime date, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetDaywiseEligibilityByDateQuery(companyCode, date), ct);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(DaywiseEligibilityDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateDaywiseEligibilityCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { serialNumber = result.SerialNumber }, result);
    }

    [HttpDelete("{serialNumber:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(long serialNumber, CancellationToken ct)
    {
        await _mediator.Send(new DeleteDaywiseEligibilityCommand(serialNumber), ct);
        return NoContent();
    }
}
