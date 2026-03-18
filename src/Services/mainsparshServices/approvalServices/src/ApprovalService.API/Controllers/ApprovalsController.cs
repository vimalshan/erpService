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
/// API Controller for Approval Master management
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ApprovalsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ApprovalsController> _logger;

    public ApprovalsController(IMediator mediator, ILogger<ApprovalsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get approval master by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApprovalMasterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetById(long id)
    {
        try
        {
            var result = await _mediator.Send(new GetApprovalMasterByIdQuery { Id = id });
            if (result == null)
            {
                return NotFound(new { message = $"Approval master with ID {id} not found" });
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting approval master by ID");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred", details = ex.Message });
        }
    }

    /// <summary>
    /// Get approval master by code
    /// </summary>
    [HttpGet("code/{code}")]
    [ProducesResponseType(typeof(ApprovalMasterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByCode(string code)
    {
        try
        {
            var result = await _mediator.Send(new GetApprovalMasterByCodeQuery { Code = code });
            if (result == null)
            {
                return NotFound(new { message = $"Approval master with code {code} not found" });
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting approval master by code");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred", details = ex.Message });
        }
    }

    /// <summary>
    /// Get all approvals by module
    /// </summary>
    [HttpGet("module/{module}")]
    [ProducesResponseType(typeof(List<ApprovalMasterDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByModule(string module)
    {
        try
        {
            var result = await _mediator.Send(new GetApprovalsByModuleQuery { Module = module });
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting approvals by module");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred", details = ex.Message });
        }
    }

    /// <summary>
    /// Get all approval masters
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<ApprovalMasterDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await _mediator.Send(new GetAllApprovalsQuery());
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all approvals");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred", details = ex.Message });
        }
    }

    /// <summary>
    /// Create a new approval master
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CreateApprovalMasterCommandResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateApprovalMasterDto dto)
    {
        try
        {
            var userId = long.Parse(User.FindFirst("sub")?.Value ?? "0");
            var command = new CreateApprovalMasterCommand
            {
                Code = dto.Code,
                Name = dto.Name,
                Module = dto.Module,
                Level = dto.Level,
                UserId = userId
            };

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation creating approval master");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating approval master");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred", details = ex.Message });
        }
    }

    /// <summary>
    /// Update an approval master
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateApprovalMasterDto dto)
    {
        try
        {
            var userId = long.Parse(User.FindFirst("sub")?.Value ?? "0");
            var command = new UpdateApprovalMasterCommand
            {
                Id = id,
                Name = dto.Name,
                Level = dto.Level,
                UserId = userId
            };

            var result = await _mediator.Send(command);
            return Ok(new { message = "Approval master updated successfully" });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Approval master not found");
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating approval master");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred", details = ex.Message });
        }
    }

    /// <summary>
    /// Deactivate an approval master
    /// </summary>
    [HttpPut("{id}/deactivate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(long id)
    {
        try
        {
            var userId = long.Parse(User.FindFirst("sub")?.Value ?? "0");
            var command = new DeactivateApprovalMasterCommand { Id = id, UserId = userId };
            await _mediator.Send(command);
            return Ok(new { message = "Approval master deactivated successfully" });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Approval master not found");
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deactivating approval master");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred", details = ex.Message });
        }
    }

    /// <summary>
    /// Activate an approval master
    /// </summary>
    [HttpPut("{id}/activate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Activate(long id)
    {
        try
        {
            var userId = long.Parse(User.FindFirst("sub")?.Value ?? "0");
            var command = new ActivateApprovalMasterCommand { Id = id, UserId = userId };
            await _mediator.Send(command);
            return Ok(new { message = "Approval master activated successfully" });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Approval master not found");
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error activating approval master");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred", details = ex.Message });
        }
    }
}
