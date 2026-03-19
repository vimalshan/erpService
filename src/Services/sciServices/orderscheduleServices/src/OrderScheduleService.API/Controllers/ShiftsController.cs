namespace OrderScheduleService.API.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderScheduleService.Application.Commands;
using OrderScheduleService.Application.DTOs;
using OrderScheduleService.Application.Queries;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class ShiftsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ShiftsController> _logger;

    public ShiftsController(IMediator mediator, ILogger<ShiftsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all shifts
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<ShiftDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllShifts()
    {
        try
        {
            var shifts = await _mediator.Send(new GetAllShiftsQuery());
            return Ok(shifts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving shifts");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Get shift by ID
    /// </summary>
    [HttpGet("{shiftCode}/company/{companyUnitId}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ShiftDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetShiftById(char shiftCode, decimal companyUnitId)
    {
        try
        {
            var shift = await _mediator.Send(new GetShiftByIdQuery(shiftCode, companyUnitId));
            if (shift == null)
                return NotFound();

            return Ok(shift);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving shift {shiftCode}");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Get shifts by company
    /// </summary>
    [HttpGet("company/{companyUnitId}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<ShiftDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetShiftsByCompany(decimal companyUnitId)
    {
        try
        {
            var shifts = await _mediator.Send(new GetShiftsByCompanyQuery(companyUnitId));
            return Ok(shifts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving shifts for company {companyUnitId}");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Create a new shift
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateShift([FromBody] CreateShiftDto shiftDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new CreateShiftCommand(shiftDto));
            return CreatedAtAction(nameof(GetShiftById), new { shiftCode = shiftDto.ShiftCode, companyUnitId = shiftDto.CompanyUnitId }, new { success = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating shift");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Update shift
    /// </summary>
    [HttpPut("{shiftCode}/company/{companyUnitId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateShift(char shiftCode, decimal companyUnitId, [FromBody] CreateShiftDto shiftDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateShiftCommand(shiftCode, companyUnitId, shiftDto));
            return Ok(new { success = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating shift {shiftCode}");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Delete shift
    /// </summary>
    [HttpDelete("{shiftCode}/company/{companyUnitId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteShift(char shiftCode, decimal companyUnitId)
    {
        try
        {
            var result = await _mediator.Send(new DeleteShiftCommand(shiftCode, companyUnitId));
            return Ok(new { success = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting shift {shiftCode}");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }
}
