namespace OrderScheduleService.API.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderScheduleService.Application.Commands;
using OrderScheduleService.Application.DTOs;
using OrderScheduleService.Application.Queries;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SchedulesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<SchedulesController> _logger;

    public SchedulesController(IMediator mediator, ILogger<SchedulesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get schedule by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ScheduleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetScheduleById(long id)
    {
        try
        {
            var schedule = await _mediator.Send(new GetScheduleByIdQuery(id));
            if (schedule == null)
                return NotFound();

            return Ok(schedule);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving schedule {id}");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Get schedules by item ID
    /// </summary>
    [HttpGet("item/{itemId}")]
    [ProducesResponseType(typeof(IEnumerable<ScheduleDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSchedulesByItem(decimal itemId)
    {
        try
        {
            var schedules = await _mediator.Send(new GetSchedulesByItemQuery(itemId));
            return Ok(schedules);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving schedules for item {itemId}");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Get schedules by date range
    /// </summary>
    [HttpGet("date-range")]
    [ProducesResponseType(typeof(IEnumerable<ScheduleDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSchedulesByDateRange([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
    {
        try
        {
            var schedules = await _mediator.Send(new GetSchedulesByDateRangeQuery(fromDate, toDate));
            return Ok(schedules);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving schedules for date range");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Get available capacity for schedule
    /// </summary>
    [HttpGet("{id}/available-capacity")]
    [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAvailableCapacity(long id)
    {
        try
        {
            var capacity = await _mediator.Send(new GetAvailableCapacityQuery(id));
            return Ok(new { availableCapacity = capacity });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving available capacity for schedule {id}");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Create a new schedule
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSchedule([FromBody] CreateScheduleDto scheduleDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var scheduleId = await _mediator.Send(new CreateScheduleCommand(scheduleDto));
            return CreatedAtAction(nameof(GetScheduleById), new { id = scheduleId }, new { scheduleId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating schedule");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Confirm schedule
    /// </summary>
    [HttpPut("{id}/confirm")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ConfirmSchedule(long id)
    {
        try
        {
            var result = await _mediator.Send(new ConfirmScheduleCommand(id));
            return Ok(new { success = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error confirming schedule {id}");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Delete schedule
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteSchedule(long id)
    {
        try
        {
            var result = await _mediator.Send(new DeleteScheduleCommand(id));
            return Ok(new { success = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting schedule {id}");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }
}
