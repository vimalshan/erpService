namespace ApprovalService.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MediatR;
using ApprovalService.Application.CQRS.Commands;
using ApprovalService.Application.CQRS.Queries;
using ApprovalService.Application.DTOs;

/// <summary>
/// API Controller for Approver Employee management
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ApproversController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ApproversController> _logger;

    public ApproversController(IMediator mediator, ILogger<ApproversController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get approver employee by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApproverEmployeeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id)
    {
        try
        {
            var result = await _mediator.Send(new GetApproverEmployeeByIdQuery { Id = id });
            if (result == null)
            {
                return NotFound(new { message = $"Approver employee with ID {id} not found" });
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting approver employee by ID");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred", details = ex.Message });
        }
    }

    /// <summary>
    /// Get approvers by approval master ID
    /// </summary>
    [HttpGet("approval/{approvalMasterId}")]
    [ProducesResponseType(typeof(List<ApproverEmployeeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByApprovalMaster(long approvalMasterId)
    {
        try
        {
            var result = await _mediator.Send(new GetApproversByApprovalMasterQuery { ApprovalMasterId = approvalMasterId });
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting approvers by approval master");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred", details = ex.Message });
        }
    }

    /// <summary>
    /// Get approvers by employee ID
    /// </summary>
    [HttpGet("employee/{employeeId}")]
    [ProducesResponseType(typeof(List<ApproverEmployeeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEmployee(long employeeId)
    {
        try
        {
            var result = await _mediator.Send(new GetApproversByEmployeeQuery { EmployeeSysId = employeeId });
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting approvers by employee");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred", details = ex.Message });
        }
    }

    /// <summary>
    /// Create a new approver employee assignment
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CreateApproverEmployeeCommandResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateApproverEmployeeDto dto)
    {
        try
        {
            var userId = long.Parse(User.FindFirst("sub")?.Value ?? "0");
            var command = new CreateApproverEmployeeCommand
            {
                ApprovalMasterId = dto.ApprovalMasterId,
                EmployeeSysId = dto.EmployeeSysId,
                ApproverLevel = dto.ApproverLevel,
                EffectiveFrom = dto.EffectiveFrom,
                EffectiveTo = dto.EffectiveTo,
                UserId = userId
            };

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Required entity not found");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating approver employee");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred", details = ex.Message });
        }
    }

    /// <summary>
    /// Update an approver employee
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateApproverEmployeeDto dto)
    {
        try
        {
            var userId = long.Parse(User.FindFirst("sub")?.Value ?? "0");
            var command = new UpdateApproverEmployeeCommand
            {
                Id = id,
                ApproverLevel = dto.ApproverLevel,
                EffectiveTo = dto.EffectiveTo,
                UserId = userId
            };

            await _mediator.Send(command);
            return Ok(new { message = "Approver employee updated successfully" });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Approver employee not found");
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating approver employee");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred", details = ex.Message });
        }
    }

    /// <summary>
    /// Deactivate an approver employee
    /// </summary>
    [HttpPut("{id}/deactivate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(long id)
    {
        try
        {
            var userId = long.Parse(User.FindFirst("sub")?.Value ?? "0");
            var command = new DeactivateApproverEmployeeCommand { Id = id, UserId = userId };
            await _mediator.Send(command);
            return Ok(new { message = "Approver employee deactivated successfully" });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Approver employee not found");
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deactivating approver employee");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred", details = ex.Message });
        }
    }

    /// <summary>
    /// Activate an approver employee
    /// </summary>
    [HttpPut("{id}/activate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Activate(long id)
    {
        try
        {
            var userId = long.Parse(User.FindFirst("sub")?.Value ?? "0");
            var command = new ActivateApproverEmployeeCommand { Id = id, UserId = userId };
            await _mediator.Send(command);
            return Ok(new { message = "Approver employee activated successfully" });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Approver employee not found");
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error activating approver employee");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred", details = ex.Message });
        }
    }
}
