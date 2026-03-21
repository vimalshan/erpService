using InsuranceService.Application.Commands;
using InsuranceService.Application.DTOs;
using InsuranceService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InsuranceController : ControllerBase
{
    private readonly IMediator _mediator;

    public InsuranceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get insurance details by company code and/or plan number
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<TravelInsuranceDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetInsurance(
        [FromQuery] string? companyCode,
        [FromQuery] long? planNumber,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetInsuranceDetailsQuery(companyCode, planNumber), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get insurance by composite key
    /// </summary>
    [HttpGet("{companyCode}/{planNumber}")]
    [ProducesResponseType(typeof(TravelInsuranceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInsuranceByKey(
        string companyCode, long planNumber, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetInsuranceDetailsQuery(companyCode, planNumber), cancellationToken);

        if (result.Count == 0) return NotFound();
        return Ok(result[0]);
    }

    /// <summary>
    /// Register a new travel insurance
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(RegisterInsuranceResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterInsurance(
        [FromBody] RegisterInsuranceCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.Success) return BadRequest(result);
        return CreatedAtAction(nameof(GetInsuranceByKey),
            new { companyCode = command.CompanyCode, planNumber = command.PlanNumber }, result);
    }

    /// <summary>
    /// Update insurance status
    /// </summary>
    [HttpPut("status")]
    [ProducesResponseType(typeof(UpdateInsuranceStatusResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(
        [FromBody] UpdateInsuranceStatusCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.Success) return NotFound(result);
        return Ok(result);
    }
}
