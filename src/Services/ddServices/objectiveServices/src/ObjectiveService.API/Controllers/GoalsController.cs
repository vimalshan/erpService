using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ObjectiveService.Application.Features.Goals.Commands;
using ObjectiveService.Application.Features.Goals.Queries;
using ObjectiveService.Application.DTOs;
using ObjectiveService.Application.Common;

namespace ObjectiveService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GoalsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<GoalsController> _logger;

    public GoalsController(IMediator mediator, ILogger<GoalsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get goal by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<GoalDto>> GetGoalById(decimal id)
    {
        _logger.LogInformation("Getting goal with ID: {GoalId}", id);
        var query = new GetGoalByIdQuery(id);
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// Get goals by employee
    /// </summary>
    [HttpGet("employee/{userId}/{pinNumber}")]
    public async Task<ActionResult<List<GoalDto>>> GetGoalsByEmployee(string userId, decimal pinNumber)
    {
        _logger.LogInformation("Getting goals for employee {UserId}", userId);
        var query = new GetGoalsByEmployeeQuery(userId, pinNumber);
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Get goals by period
    /// </summary>
    [HttpGet("period")]
    public async Task<ActionResult<List<GoalDto>>> GetGoalsByPeriod(
        [FromQuery] DateTime periodFrom,
        [FromQuery] DateTime periodTo,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("Getting goals for period {PeriodFrom} to {PeriodTo}", periodFrom, periodTo);
        var query = new GetGoalsByPeriodQuery
        {
            PeriodFrom = periodFrom,
            PeriodTo = periodTo,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Get active draft goals
    /// </summary>
    [HttpGet("active-drafts")]
    public async Task<ActionResult<List<GoalDto>>> GetActiveDraftGoals(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("Getting active draft goals");
        var query = new GetActiveDraftGoalsQuery { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Create a new goal
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<CommandResult<decimal>>> CreateGoal(CreateGoalDto dto)
    {
        _logger.LogInformation("Creating goal for employee {UserId}", dto.UserId);
        
        var command = new CreateGoalCommand
        {
            UserId = dto.UserId,
            PinNumber = dto.PinNumber,
            PeriodFrom = dto.PeriodFrom,
            PeriodTo = dto.PeriodTo,
            ReferenceNumber = dto.ReferenceNumber,
            FormFlag = dto.FormFlag,
            SubGoals = dto.SubGoals.Select(sg => new CreateGoalSubGoalItem
            {
                Description = sg.Description,
                UnitFrom = sg.UnitFrom,
                UnitTo = sg.UnitTo,
                UnitOfMeasurement = sg.UnitOfMeasurement,
                Category = sg.Category
            }).ToList()
        };

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetGoalById), new { id = result.Data }, result);
    }

    /// <summary>
    /// Submit goal for approval
    /// </summary>
    [HttpPost("{id}/submit")]
    public async Task<ActionResult<CommandResult>> SubmitGoalForApproval(decimal id)
    {
        _logger.LogInformation("Submitting goal {GoalId} for approval", id);
        var command = new SubmitGoalForApprovalCommand { GoalId = id };
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Approve a goal
    /// </summary>
    [HttpPost("{id}/approve")]
    [Authorize(Roles = "Approver")]
    public async Task<ActionResult<CommandResult>> ApproveGoal(decimal id, [FromBody] ApproveGoalDto dto)
    {
        _logger.LogInformation("Approving goal {GoalId}", id);
        var command = new ApproveGoalCommand { GoalId = id, Remarks = dto.Remarks };
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Return goal for revision
    /// </summary>
    [HttpPost("{id}/return")]
    [Authorize(Roles = "Approver")]
    public async Task<ActionResult<CommandResult>> ReturnGoal(decimal id, [FromBody] ReturnGoalDto dto)
    {
        _logger.LogInformation("Returning goal {GoalId}", id);
        var command = new ReturnGoalCommand { GoalId = id, Remarks = dto.Remarks };
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Close a goal
    /// </summary>
    [HttpPost("{id}/close")]
    public async Task<ActionResult<CommandResult>> CloseGoal(decimal id)
    {
        _logger.LogInformation("Closing goal {GoalId}", id);
        var command = new CloseGoalCommand { GoalId = id };
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
}
