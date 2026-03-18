using Microsoft.AspNetCore.Mvc;
using MediatR;
using EligibilityService.Application.Commands.EligibilityMaster;
using EligibilityService.Application.DTOs;
using EligibilityService.Application.Queries.EligibilityMaster;
using Microsoft.AspNetCore.Authorization;

namespace EligibilityService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EligibilityMasterController : ControllerBase
{
    private readonly IMediator _mediator;

    public EligibilityMasterController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get all eligibility master records, optionally filtered by canteen unit.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EligibilityMasterDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] long? canteenUnit, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllEligibilityMastersQuery(canteenUnit), ct);
        return Ok(result);
    }

    /// <summary>Get a single eligibility master record by composite key.</summary>
    [HttpGet("{canteenUnit}/{shiftCode}/{itemCode}")]
    [ProducesResponseType(typeof(EligibilityMasterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long canteenUnit, string shiftCode, decimal itemCode, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetEligibilityMasterQuery(canteenUnit, shiftCode, itemCode), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Check if an employee is eligible for a meal.</summary>
    [HttpGet("check")]
    [ProducesResponseType(typeof(EligibilityCheckResultDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Check(
        [FromQuery] long canteenUnit,
        [FromQuery] string shiftCode,
        [FromQuery] decimal itemCode,
        [FromQuery] int requestedQty,
        CancellationToken ct)
    {
        var result = await _mediator.Send(
            new CheckEmployeeEligibilityQuery(canteenUnit, shiftCode, itemCode, requestedQty), ct);
        return Ok(result);
    }

    /// <summary>Get audit history for an eligibility record.</summary>
    [HttpGet("{canteenUnit}/{shiftCode}/{itemCode}/history")]
    [ProducesResponseType(typeof(IEnumerable<EligibilityMasterHistoryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHistory(long canteenUnit, string shiftCode, decimal itemCode, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetEligibilityHistoryQuery(canteenUnit, shiftCode, itemCode), ct);
        return Ok(result);
    }

    /// <summary>Create a new eligibility master record.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(EligibilityMasterDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateEligibilityMasterCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById),
            new { canteenUnit = result.CanteenUnit, shiftCode = result.ShiftCode, itemCode = result.ItemCode },
            result);
    }

    /// <summary>Update an existing eligibility record.</summary>
    [HttpPut("{canteenUnit}/{shiftCode}/{itemCode}")]
    [ProducesResponseType(typeof(EligibilityMasterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        long canteenUnit, string shiftCode, decimal itemCode,
        [FromBody] UpdateEligibilityMasterCommand command, CancellationToken ct)
    {
        if (command.CanteenUnit != canteenUnit || command.ShiftCode != shiftCode || command.ItemCode != itemCode)
            return BadRequest("Route parameters must match the request body.");

        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }

    /// <summary>Delete an eligibility record.</summary>
    [HttpDelete("{canteenUnit}/{shiftCode}/{itemCode}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long canteenUnit, string shiftCode, decimal itemCode, CancellationToken ct)
    {
        await _mediator.Send(new DeleteEligibilityMasterCommand(canteenUnit, shiftCode, itemCode), ct);
        return NoContent();
    }
}
