using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MediatR;
using CompensationService.Application.Commands;
using CompensationService.Application.Queries;
using CompensationService.Application.DTOs;
using System.ComponentModel.DataAnnotations;

namespace CompensationService.API.Controllers;

/// <summary>
/// API Controller for Compensation Grade management
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CompensationGradesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CompensationGradesController> _logger;

    public CompensationGradesController(IMediator mediator, ILogger<CompensationGradesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all compensation grades
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<CompensationGradeDto>>> GetAllGrades(CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetAllCompensationGradesQuery();
            var grades = await _mediator.Send(query, cancellationToken);
            return Ok(grades);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving grades: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving grades");
        }
    }

    /// <summary>
    /// Get active compensation grades
    /// </summary>
    [HttpGet("active")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<CompensationGradeDto>>> GetActiveGrades(CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetActiveCompensationGradesQuery();
            var grades = await _mediator.Send(query, cancellationToken);
            return Ok(grades);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving active grades: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving active grades");
        }
    }

    /// <summary>
    /// Get compensation grade by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CompensationGradeDto>> GetGradeById([FromRoute, Required] long id, CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetCompensationGradeByIdQuery { GradeId = id };
            var grade = await _mediator.Send(query, cancellationToken);
            return Ok(grade);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning($"Grade not found: {ex.Message}");
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving grade: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the grade");
        }
    }

    /// <summary>
    /// Create a new compensation grade
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CompensationGradeDto>> CreateGrade([FromBody, Required] CreateCompensationGradeDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var command = new CreateCompensationGradeCommand
            {
                GradeCode = dto.GradeCode,
                GradeName = dto.GradeName,
                GradeLevel = dto.GradeLevel,
                BaseSalary = dto.BaseSalary,
                HraPercentage = dto.HraPercentage ?? 0,
                DaPercentage = dto.DaPercentage ?? 0,
                EffectiveFrom = dto.EffectiveFrom,
                CreatedBy = 1 // TODO: Get from current user context
            };

            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetGradeById), new { id = result.GradeId }, result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning($"Conflict creating grade: {ex.Message}");
            return Conflict(ex.Message);
        }
        catch (FluentValidation.ValidationException ex)
        {
            _logger.LogWarning($"Validation error creating grade: {ex.Message}");
            return BadRequest(ex.Errors.Select(e => e.ErrorMessage));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating grade: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the grade");
        }
    }

    /// <summary>
    /// Update a compensation grade
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CompensationGradeDto>> UpdateGrade([FromRoute, Required] long id, [FromBody, Required] UpdateCompensationGradeDto dto, CancellationToken cancellationToken)
    {
        try
        {
            dto.GradeId = id;
            var command = new UpdateCompensationGradeCommand
            {
                GradeId = dto.GradeId,
                GradeName = dto.GradeName,
                BaseSalary = dto.BaseSalary,
                HraPercentage = dto.HraPercentage ?? 0,
                DaPercentage = dto.DaPercentage ?? 0,
                UpdatedBy = 1 // TODO: Get from current user context
            };

            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning($"Grade not found: {ex.Message}");
            return NotFound(ex.Message);
        }
        catch (FluentValidation.ValidationException ex)
        {
            _logger.LogWarning($"Validation error updating grade: {ex.Message}");
            return BadRequest(ex.Errors.Select(e => e.ErrorMessage));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating grade: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the grade");
        }
    }

    /// <summary>
    /// Change compensation grade status
    /// </summary>
    [HttpPatch("{id}/status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<bool>> ChangeGradeStatus([FromRoute, Required] long id, [FromBody, Required] ChangeGradeStatusDto dto, CancellationToken cancellationToken)
    {
        try
        {
            dto.GradeId = id;
            var command = new ChangeCompensationGradeStatusCommand
            {
                GradeId = dto.GradeId,
                NewStatus = dto.NewStatus[0],
                ChangedBy = 1 // TODO: Get from current user context
            };

            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning($"Grade not found: {ex.Message}");
            return NotFound(ex.Message);
        }
        catch (FluentValidation.ValidationException ex)
        {
            _logger.LogWarning($"Validation error changing status: {ex.Message}");
            return BadRequest(ex.Errors.Select(e => e.ErrorMessage));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error changing grade status: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while changing the grade status");
        }
    }
}
