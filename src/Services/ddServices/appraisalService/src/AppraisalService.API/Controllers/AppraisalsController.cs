using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using AppraisalService.Application.CQRS.Commands;
using AppraisalService.Application.CQRS.Queries;
using AppraisalService.Application.DTOs;
using AppraisalService.Infrastructure.Authentication;

namespace AppraisalService.API.Controllers;

/// <summary>
/// REST API controller for Appraisal operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AppraisalsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AppraisalsController> _logger;
    private readonly IJwtTokenService _jwtTokenService;

    public AppraisalsController(IMediator mediator, ILogger<AppraisalsController> logger, IJwtTokenService jwtTokenService)
    {
        _mediator = mediator;
        _logger = logger;
        _jwtTokenService = jwtTokenService;
    }

    /// <summary>
    /// Get appraisal by request number
    /// </summary>
    [HttpGet("{requestNumber}")]
    [ProducesResponseType(typeof(AppraisalDetailedDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAppraisal(long requestNumber, CancellationToken cancellationToken)
    {
        try
        {
            var appraisal = await _mediator.Send(
                new GetAppraisalByRequestQuery(requestNumber), 
                cancellationToken);

            if (appraisal == null)
                return NotFound($"Appraisal with request number {requestNumber} not found");

            return Ok(appraisal);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving appraisal {RequestNumber}", requestNumber);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get appraisal by user code
    /// </summary>
    [HttpGet("user/{userCode}")]
    [ProducesResponseType(typeof(AppraisalMainDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAppraisalByUser(string userCode, CancellationToken cancellationToken)
    {
        try
        {
            var appraisal = await _mediator.Send(
                new GetAppraisalByUserQuery(userCode),
                cancellationToken);

            if (appraisal == null)
                return NotFound($"Appraisal for user {userCode} not found");

            return Ok(appraisal);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving appraisal for user {UserCode}", userCode);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get appraisals by year
    /// </summary>
    [HttpGet("year/{yearId}")]
    [ProducesResponseType(typeof(IEnumerable<AppraisalMainDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAppraisalsByYear(long yearId, CancellationToken cancellationToken)
    {
        try
        {
            var appraisals = await _mediator.Send(
                new GetAppraisalsByYearQuery(yearId),
                cancellationToken);

            return Ok(appraisals);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving appraisals for year {YearId}", yearId);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get appraisals by status
    /// </summary>
    [HttpGet("status/{statusCode}")]
    [ProducesResponseType(typeof(IEnumerable<AppraisalMainDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAppraisalsByStatus(string statusCode, CancellationToken cancellationToken)
    {
        try
        {
            var appraisals = await _mediator.Send(
                new GetAppraisalsByStatusQuery(statusCode),
                cancellationToken);

            return Ok(appraisals);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving appraisals with status {StatusCode}", statusCode);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Create new appraisal
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAppraisal(
        [FromBody] CreateOrUpdateAppraisalDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            var requestNumber = await _mediator.Send(
                new CreateAppraisalCommand(dto),
                cancellationToken);

            return CreatedAtAction(nameof(GetAppraisal), new { requestNumber }, requestNumber);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Validation failed",
                Detail = string.Join("; ", ex.Errors.Select(e => e.ErrorMessage))
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating appraisal");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Update appraisal
    /// </summary>
    [HttpPut("{requestNumber}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAppraisal(
        long requestNumber,
        [FromBody] CreateOrUpdateAppraisalDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            await _mediator.Send(
                new UpdateAppraisalCommand(requestNumber, dto),
                cancellationToken);

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating appraisal {RequestNumber}", requestNumber);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Submit appraisal
    /// </summary>
    [HttpPost("{requestNumber}/submit")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SubmitAppraisal(
        long requestNumber,
        [FromBody] SubmitAppraisalDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            await _mediator.Send(
                new SubmitAppraisalCommand(requestNumber, dto.FinalVtcRating),
                cancellationToken);

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting appraisal {RequestNumber}", requestNumber);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Cancel appraisal
    /// </summary>
    [HttpPost("{requestNumber}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Admin,HR")]
    public async Task<IActionResult> CancelAppraisal(
        long requestNumber,
        [FromBody] Dictionary<string, string> request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (!request.TryGetValue("remarks", out var remarks))
                return BadRequest("Remarks field is required");

            var approverId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0";

            await _mediator.Send(
                new CancelAppraisalCommand(requestNumber, remarks, long.Parse(approverId)),
                cancellationToken);

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling appraisal {RequestNumber}", requestNumber);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get competency assessments for appraisal
    /// </summary>
    [HttpGet("{requestNumber}/competencies")]
    [ProducesResponseType(typeof(IEnumerable<CompetencyAssessmentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCompetencies(long requestNumber, CancellationToken cancellationToken)
    {
        try
        {
            var assessments = await _mediator.Send(
                new GetCompetencyAssessmentsQuery(requestNumber),
                cancellationToken);

            return Ok(assessments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving competencies for appraisal {RequestNumber}", requestNumber);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Add competency assessment
    /// </summary>
    [HttpPost("{requestNumber}/competencies")]
    [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
    [Authorize(Roles = "Appraiser,HR")]
    public async Task<IActionResult> AddCompetency(
        long requestNumber,
        [FromBody] CompetencyAssessmentDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            var serialNumber = await _mediator.Send(
                new AddCompetencyAssessmentCommand(requestNumber, dto),
                cancellationToken);

            return CreatedAtAction(nameof(GetCompetencies), new { requestNumber }, serialNumber);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding competency for appraisal {RequestNumber}", requestNumber);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Generate test JWT token (for testing only)
    /// </summary>
    [HttpGet("token")]
    [AllowAnonymous]
    public IActionResult GetTestToken()
    {
        try
        {
            var token = _jwtTokenService.GenerateToken(1, "TestUser", new[] { "Admin", "HR" });
            
            return Ok(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating test token");
            return StatusCode(500, ex.Message);
        }
    }
}

/// <summary>
/// REST API controller for AppraisalBand operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AppraisalBandsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AppraisalBandsController> _logger;

    public AppraisalBandsController(IMediator mediator, ILogger<AppraisalBandsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all appraisal bands
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AppraisalBandDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBands(CancellationToken cancellationToken)
    {
        try
        {
            var bands = await _mediator.Send(
                new GetAppraisalBandsQuery(),
                cancellationToken);

            return Ok(bands);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving appraisal bands");
            return StatusCode(500, "Internal server error");
        }
    }
}
