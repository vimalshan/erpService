using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Asp.Versioning;
using PromotionService.DTOs;
using PromotionService.Features.Commands;
using PromotionService.Features.Queries;

namespace PromotionService.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize]
[Produces("application/json")]
public class PromotionsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<PromotionsController> _logger;

    public PromotionsController(IMediator mediator, ILogger<PromotionsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    #region Rating Endpoints
    /// <summary>Get rating by ID</summary>
    [HttpGet("ratings/{id}")]
    [ProducesResponseType(typeof(RatingDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRating(long id, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Fetching rating: {id}");
            var result = await _mediator.Send(new GetRatingByIdQuery { RatingId = id }, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (Exception ex) { _logger.LogError(ex, "Error fetching rating"); return StatusCode(500, "Internal server error"); }
    }

    /// <summary>Get ratings for employee</summary>
    [HttpGet("employees/{employeeId}/ratings")]
    [ProducesResponseType(typeof(IEnumerable<RatingDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEmployeeRatings(long employeeId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation($"Fetching ratings for employee: {employeeId}");
            var result = await _mediator.Send(new GetRatingsByEmployeeQuery { EmployeeSystemId = employeeId, PageNumber = pageNumber, PageSize = pageSize }, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex) { _logger.LogError(ex, "Error fetching employee ratings"); return StatusCode(500, "Internal server error"); }
    }

    /// <summary>Get all ratings (HR/Admin)</summary>
    [HttpGet("ratings")]
    [Authorize(Roles = "HR,Admin")]
    [ProducesResponseType(typeof(IEnumerable<RatingDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllRatings([FromQuery] int? ddYear, [FromQuery] string status, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation($"Fetching all ratings - Page: {pageNumber}");
            var result = await _mediator.Send(new GetAllRatingsQuery { DDYear = ddYear, Status = status, PageNumber = pageNumber, PageSize = pageSize }, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex) { _logger.LogError(ex, "Error fetching ratings"); return StatusCode(500, "Internal server error"); }
    }

    /// <summary>Get pending ratings (HR/Admin)</summary>
    [HttpGet("ratings/pending")]
    [Authorize(Roles = "HR,Admin")]
    [ProducesResponseType(typeof(IEnumerable<RatingDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPendingRatings([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation($"Fetching pending ratings - Page: {pageNumber}");
            var result = await _mediator.Send(new GetPendingRatingsQuery { PageNumber = pageNumber, PageSize = pageSize }, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex) { _logger.LogError(ex, "Error fetching pending ratings"); return StatusCode(500, "Internal server error"); }
    }

    /// <summary>Create rating</summary>
    [HttpPost("ratings")]
    [Authorize(Roles = "HR,Admin")]
    [ProducesResponseType(typeof(RatingDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRating([FromBody] CreateRatingDto dto, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Creating rating for employee: {dto.EmployeeSystemId}");
            var result = await _mediator.Send(new CreateRatingCommand
            {
                EmployeeSystemId = dto.EmployeeSystemId,
                DDYear = dto.DDYear,
                AppraisalScore = dto.AppraisalScore,
                CompetencyScore = dto.CompetencyScore,
                GoalCompletionScore = dto.GoalCompletionScore
            }, cancellationToken);
            return CreatedAtAction(nameof(GetRating), new { id = result.RatingId }, result);
        }
        catch (Exception ex) { _logger.LogError(ex, "Error creating rating"); return BadRequest("Failed to create rating"); }
    }

    /// <summary>Update rating</summary>
    [HttpPut("ratings/{id}")]
    [Authorize(Roles = "HR,Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRating(long id, [FromBody] UpdateRatingDto dto, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Updating rating: {id}");
            await _mediator.Send(new UpdateRatingCommand
            {
                RatingId = id,
                AppraisalScore = dto.AppraisalScore,
                CompetencyScore = dto.CompetencyScore,
                GoalCompletionScore = dto.GoalCompletionScore
            }, cancellationToken);
            return Ok();
        }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (Exception ex) { _logger.LogError(ex, "Error updating rating"); return StatusCode(500, "Internal server error"); }
    }

    /// <summary>Finalize rating</summary>
    [HttpPost("ratings/{id}/finalize")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> FinalizeRating(long id, [FromBody] FinalizeRatingDto dto, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Finalizing rating: {id}");
            await _mediator.Send(new FinalizeRatingCommand { RatingId = id, ApprovedBySystemId = dto.ApprovedBySystemId }, cancellationToken);
            return Ok();
        }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (Exception ex) { _logger.LogError(ex, "Error finalizing rating"); return StatusCode(500, "Internal server error"); }
    }

    /// <summary>Delete rating (Admin only)</summary>
    [HttpDelete("ratings/{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRating(long id, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Deleting rating: {id}");
            await _mediator.Send(new DeleteRatingCommand { RatingId = id }, cancellationToken);
            return Ok();
        }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (Exception ex) { _logger.LogError(ex, "Error deleting rating"); return StatusCode(500, "Internal server error"); }
    }
    #endregion

    #region Promotion Endpoints
    /// <summary>Get promotion by ID</summary>
    [HttpGet("recommendations/{id}")]
    [ProducesResponseType(typeof(PromotionRecommendationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPromotion(long id, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Fetching promotion: {id}");
            var result = await _mediator.Send(new GetPromotionByIdQuery { PromotionId = id }, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (Exception ex) { _logger.LogError(ex, "Error fetching promotion"); return StatusCode(500, "Internal server error"); }
    }

    /// <summary>Get promotions for employee</summary>
    [HttpGet("employees/{employeeId}/recommendations")]
    [ProducesResponseType(typeof(IEnumerable<PromotionRecommendationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEmployeePromotions(long employeeId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation($"Fetching promotions for employee: {employeeId}");
            var result = await _mediator.Send(new GetPromotionsByEmployeeQuery { EmployeeSystemId = employeeId, PageNumber = pageNumber, PageSize = pageSize }, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex) { _logger.LogError(ex, "Error fetching employee promotions"); return StatusCode(500, "Internal server error"); }
    }

    /// <summary>Get all promotions (HR/Admin)</summary>
    [HttpGet("recommendations")]
    [Authorize(Roles = "HR,Admin")]
    [ProducesResponseType(typeof(IEnumerable<PromotionRecommendationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllPromotions([FromQuery] string status, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation($"Fetching all promotions - Page: {pageNumber}");
            var result = await _mediator.Send(new GetAllPromotionsQuery { Status = status, PageNumber = pageNumber, PageSize = pageSize }, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex) { _logger.LogError(ex, "Error fetching promotions"); return StatusCode(500, "Internal server error"); }
    }

    /// <summary>Get pending promotions (HR/Admin)</summary>
    [HttpGet("recommendations/pending")]
    [Authorize(Roles = "HR,Admin")]
    [ProducesResponseType(typeof(IEnumerable<PromotionRecommendationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPendingPromotions([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation($"Fetching pending promotions - Page: {pageNumber}");
            var result = await _mediator.Send(new GetPendingPromotionsQuery { PageNumber = pageNumber, PageSize = pageSize }, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex) { _logger.LogError(ex, "Error fetching pending promotions"); return StatusCode(500, "Internal server error"); }
    }

    /// <summary>Create promotion recommendation</summary>
    [HttpPost("recommendations")]
    [Authorize(Roles = "HR,Admin")]
    [ProducesResponseType(typeof(PromotionRecommendationDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreatePromotion([FromBody] CreatePromotionRecommendationDto dto, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Creating promotion for employee: {dto.EmployeeSystemId}");
            var result = await _mediator.Send(new CreatePromotionRecommendationCommand
            {
                RatingId = dto.RatingId,
                EmployeeSystemId = dto.EmployeeSystemId,
                CurrentDesignation = dto.CurrentDesignation,
                CurrentGrade = dto.CurrentGrade,
                ProposedDesignation = dto.ProposedDesignation,
                ProposedGrade = dto.ProposedGrade,
                PromotionEffectiveDate = dto.PromotionEffectiveDate,
                ProposedSalaryIncrease = dto.ProposedSalaryIncrease,
                PromotionReason = dto.PromotionReason
            }, cancellationToken);
            return CreatedAtAction(nameof(GetPromotion), new { id = result.PromotionId }, result);
        }
        catch (Exception ex) { _logger.LogError(ex, "Error creating promotion"); return BadRequest("Failed to create promotion"); }
    }

    /// <summary>Update promotion recommendation</summary>
    [HttpPut("recommendations/{id}")]
    [Authorize(Roles = "HR,Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePromotion(long id, [FromBody] UpdatePromotionRecommendationDto dto, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Updating promotion: {id}");
            await _mediator.Send(new UpdatePromotionRecommendationCommand
            {
                PromotionId = id,
                ProposedDesignation = dto.ProposedDesignation,
                ProposedGrade = dto.ProposedGrade,
                PromotionEffectiveDate = dto.PromotionEffectiveDate,
                ProposedSalaryIncrease = dto.ProposedSalaryIncrease,
                PromotionReason = dto.PromotionReason
            }, cancellationToken);
            return Ok();
        }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (Exception ex) { _logger.LogError(ex, "Error updating promotion"); return StatusCode(500, "Internal server error"); }
    }

    /// <summary>Approve promotion (Admin only)</summary>
    [HttpPost("recommendations/{id}/approve")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ApprovePromotion(long id, [FromBody] ApprovePromotionDto dto, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Approving promotion: {id}");
            await _mediator.Send(new ApprovePromotionRecommendationCommand { PromotionId = id, ApprovedBySystemId = dto.ApprovedBySystemId }, cancellationToken);
            return Ok();
        }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (Exception ex) { _logger.LogError(ex, "Error approving promotion"); return StatusCode(500, "Internal server error"); }
    }

    /// <summary>Reject promotion (Admin only)</summary>
    [HttpPost("recommendations/{id}/reject")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RejectPromotion(long id, [FromBody] RejectPromotionDto dto, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Rejecting promotion: {id}");
            await _mediator.Send(new RejectPromotionRecommendationCommand { PromotionId = id, ReasonForRejection = dto.ReasonForRejection }, cancellationToken);
            return Ok();
        }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (Exception ex) { _logger.LogError(ex, "Error rejecting promotion"); return StatusCode(500, "Internal server error"); }
    }

    /// <summary>Delete promotion (Admin only)</summary>
    [HttpDelete("recommendations/{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePromotion(long id, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Deleting promotion: {id}");
            await _mediator.Send(new DeletePromotionRecommendationCommand { PromotionId = id }, cancellationToken);
            return Ok();
        }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (Exception ex) { _logger.LogError(ex, "Error deleting promotion"); return StatusCode(500, "Internal server error"); }
    }
    #endregion

    #region Increment Endpoints
    /// <summary>Get increment by ID</summary>
    [HttpGet("increments/{id}")]
    [ProducesResponseType(typeof(IncrementRequestDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetIncrement(long id, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Fetching increment: {id}");
            var result = await _mediator.Send(new GetIncrementByIdQuery { IncrementId = id }, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (Exception ex) { _logger.LogError(ex, "Error fetching increment"); return StatusCode(500, "Internal server error"); }
    }

    /// <summary>Get increments for employee</summary>
    [HttpGet("employees/{employeeId}/increments")]
    [ProducesResponseType(typeof(IEnumerable<IncrementRequestDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEmployeeIncrements(long employeeId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation($"Fetching increments for employee: {employeeId}");
            var result = await _mediator.Send(new GetIncrementsByEmployeeQuery { EmployeeSystemId = employeeId, PageNumber = pageNumber, PageSize = pageSize }, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex) { _logger.LogError(ex, "Error fetching employee increments"); return StatusCode(500, "Internal server error"); }
    }

    /// <summary>Get all increments (HR/Admin)</summary>
    [HttpGet("increments")]
    [Authorize(Roles = "HR,Admin")]
    [ProducesResponseType(typeof(IEnumerable<IncrementRequestDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllIncrements([FromQuery] string incrementType, [FromQuery] string status, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation($"Fetching all increments - Page: {pageNumber}");
            var result = await _mediator.Send(new GetAllIncrementsQuery { IncrementType = incrementType, Status = status, PageNumber = pageNumber, PageSize = pageSize }, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex) { _logger.LogError(ex, "Error fetching increments"); return StatusCode(500, "Internal server error"); }
    }

    /// <summary>Get pending increments (HR/Admin)</summary>
    [HttpGet("increments/pending")]
    [Authorize(Roles = "HR,Admin")]
    [ProducesResponseType(typeof(IEnumerable<IncrementRequestDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPendingIncrements([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation($"Fetching pending increments - Page: {pageNumber}");
            var result = await _mediator.Send(new GetPendingIncrementsQuery { PageNumber = pageNumber, PageSize = pageSize }, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex) { _logger.LogError(ex, "Error fetching pending increments"); return StatusCode(500, "Internal server error"); }
    }

    /// <summary>Create increment request</summary>
    [HttpPost("increments")]
    [Authorize(Roles = "HR,Admin")]
    [ProducesResponseType(typeof(IncrementRequestDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateIncrement([FromBody] CreateIncrementRequestDto dto, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Creating increment for employee: {dto.EmployeeSystemId}");
            var result = await _mediator.Send(new CreateIncrementRequestCommand
            {
                RatingId = dto.RatingId,
                EmployeeSystemId = dto.EmployeeSystemId,
                IncrementType = dto.IncrementType,
                CurrentBaseSalary = dto.CurrentBaseSalary,
                ProposedBaseSalary = dto.ProposedBaseSalary,
                IncrementReason = dto.IncrementReason,
                EffectiveFromDate = dto.EffectiveFromDate
            }, cancellationToken);
            return CreatedAtAction(nameof(GetIncrement), new { id = result.IncrementId }, result);
        }
        catch (Exception ex) { _logger.LogError(ex, "Error creating increment"); return BadRequest("Failed to create increment"); }
    }

    /// <summary>Update increment request</summary>
    [HttpPut("increments/{id}")]
    [Authorize(Roles = "HR,Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateIncrement(long id, [FromBody] UpdateIncrementRequestDto dto, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Updating increment: {id}");
            await _mediator.Send(new UpdateIncrementRequestCommand
            {
                IncrementId = id,
                ProposedBaseSalary = dto.ProposedBaseSalary,
                IncrementReason = dto.IncrementReason,
                EffectiveFromDate = dto.EffectiveFromDate
            }, cancellationToken);
            return Ok();
        }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (Exception ex) { _logger.LogError(ex, "Error updating increment"); return StatusCode(500, "Internal server error"); }
    }

    /// <summary>Approve increment (Admin only)</summary>
    [HttpPost("increments/{id}/approve")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ApproveIncrement(long id, [FromBody] ApproveIncrementDto dto, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Approving increment: {id}");
            await _mediator.Send(new ApproveIncrementRequestCommand { IncrementId = id, ApprovedBySystemId = dto.ApprovedBySystemId }, cancellationToken);
            return Ok();
        }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (Exception ex) { _logger.LogError(ex, "Error approving increment"); return StatusCode(500, "Internal server error"); }
    }

    /// <summary>Delete increment (Admin only)</summary>
    [HttpDelete("increments/{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteIncrement(long id, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Deleting increment: {id}");
            await _mediator.Send(new DeleteIncrementRequestCommand { IncrementId = id }, cancellationToken);
            return Ok();
        }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (Exception ex) { _logger.LogError(ex, "Error deleting increment"); return StatusCode(500, "Internal server error"); }
    }
    #endregion

    #region VTC Assessment Endpoints
    /// <summary>Get VTC assessment by ID</summary>
    [HttpGet("assessments/{id}")]
    [ProducesResponseType(typeof(VTCAssessmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetVTCAssessment(long id, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Fetching VTC assessment: {id}");
            var result = await _mediator.Send(new GetVTCAssessmentByIdQuery { VTCAssessmentId = id }, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (Exception ex) { _logger.LogError(ex, "Error fetching VTC assessment"); return StatusCode(500, "Internal server error"); }
    }

    /// <summary>Get VTC assessments for employee</summary>
    [HttpGet("employees/{employeeId}/assessments")]
    [ProducesResponseType(typeof(IEnumerable<VTCAssessmentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEmployeeVTCAssessments(long employeeId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation($"Fetching VTC assessments for employee: {employeeId}");
            var result = await _mediator.Send(new GetVTCAssessmentsByEmployeeQuery { EmployeeSystemId = employeeId, PageNumber = pageNumber, PageSize = pageSize }, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex) { _logger.LogError(ex, "Error fetching employee VTC assessments"); return StatusCode(500, "Internal server error"); }
    }

    /// <summary>Get VTC assessments by year (HR/Admin)</summary>
    [HttpGet("assessments/year/{year}")]
    [Authorize(Roles = "HR,Admin")]
    [ProducesResponseType(typeof(IEnumerable<VTCAssessmentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetVTCAssessmentsByYear(int year, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation($"Fetching VTC assessments for year: {year}");
            var result = await _mediator.Send(new GetVTCAssessmentsByYearQuery { DDYear = year, PageNumber = pageNumber, PageSize = pageSize }, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex) { _logger.LogError(ex, "Error fetching VTC assessments by year"); return StatusCode(500, "Internal server error"); }
    }

    /// <summary>Create VTC assessment</summary>
    [HttpPost("assessments")]
    [Authorize(Roles = "HR,Admin")]
    [ProducesResponseType(typeof(VTCAssessmentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateVTCAssessment([FromBody] CreateVTCAssessmentDto dto, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Creating VTC assessment for employee: {dto.EmployeeSystemId}");
            var result = await _mediator.Send(new CreateVTCAssessmentCommand
            {
                EmployeeSystemId = dto.EmployeeSystemId,
                DDYear = dto.DDYear,
                Quarter = dto.Quarter,
                Score = dto.Score
            }, cancellationToken);
            return CreatedAtAction(nameof(GetVTCAssessment), new { id = result.VTCAssessmentId }, result);
        }
        catch (Exception ex) { _logger.LogError(ex, "Error creating VTC assessment"); return BadRequest("Failed to create VTC assessment"); }
    }

    /// <summary>Update VTC assessment</summary>
    [HttpPut("assessments/{id}")]
    [Authorize(Roles = "HR,Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateVTCAssessment(long id, [FromBody] UpdateVTCAssessmentDto dto, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Updating VTC assessment: {id}");
            await _mediator.Send(new UpdateVTCAssessmentCommand
            {
                VTCAssessmentId = id,
                Score = dto.Score
            }, cancellationToken);
            return Ok();
        }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (Exception ex) { _logger.LogError(ex, "Error updating VTC assessment"); return StatusCode(500, "Internal server error"); }
    }

    /// <summary>Delete VTC assessment (Admin only)</summary>
    [HttpDelete("assessments/{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteVTCAssessment(long id, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Deleting VTC assessment: {id}");
            await _mediator.Send(new DeleteVTCAssessmentCommand { VTCAssessmentId = id }, cancellationToken);
            return Ok();
        }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (Exception ex) { _logger.LogError(ex, "Error deleting VTC assessment"); return StatusCode(500, "Internal server error"); }
    }
    #endregion

    #region HorizontalPromotion Endpoints
    /// <summary>Get horizontal promotion by transaction ID</summary>
    [HttpGet("horizontal/{transId}")]
    [ProducesResponseType(typeof(HorizontalPromotionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHorizontalPromotion(decimal transId, CancellationToken cancellationToken)
    {
        try { return Ok(await _mediator.Send(new GetHorizontalPromotionByIdQuery { TransactionId = transId }, cancellationToken)); }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (Exception ex) { _logger.LogError(ex, "Error fetching horizontal promotion"); return StatusCode(500, "Internal server error"); }
    }

    /// <summary>Get all horizontal promotions (HR/Admin)</summary>
    [HttpGet("horizontal")]
    [Authorize(Roles = "HR,Admin")]
    [ProducesResponseType(typeof(IEnumerable<HorizontalPromotionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllHorizontalPromotions([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try { return Ok(await _mediator.Send(new GetAllHorizontalPromotionsQuery { PageNumber = pageNumber, PageSize = pageSize }, cancellationToken)); }
        catch (Exception ex) { _logger.LogError(ex, "Error"); return StatusCode(500, "Internal server error"); }
    }

    /// <summary>Create horizontal promotion</summary>
    [HttpPost("horizontal")]
    [Authorize(Roles = "HR,Admin")]
    [ProducesResponseType(typeof(HorizontalPromotionDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateHorizontalPromotion([FromBody] CreateHorizontalPromotionDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new CreateHorizontalPromotionCommand
            {
                EmployeeSystemId = dto.EmployeeSystemId,
                PromotionScore = dto.PromotionScore,
                GradeId = dto.GradeId,
                CurrentLevelId = dto.CurrentLevelId,
                NewLevelId = dto.NewLevelId,
                EffectiveFrom = dto.EffectiveFrom,
                PositionId = dto.PositionId,
                OldPositionName = dto.OldPositionName,
                OldPositionDesignation = dto.OldPositionDesignation,
                NewPositionName = dto.NewPositionName,
                NewPositionDesignation = dto.NewPositionDesignation,
                UpdatedBy = 0
            }, cancellationToken);
            return CreatedAtAction(nameof(GetHorizontalPromotion), new { transId = result.TransactionId }, result);
        }
        catch (Exception ex) { _logger.LogError(ex, "Error creating horizontal promotion"); return BadRequest("Failed"); }
    }

    /// <summary>Confirm horizontal promotion in HRMS</summary>
    [HttpPost("horizontal/{transId}/confirm")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ConfirmHorizontalPromotion(decimal transId, CancellationToken cancellationToken)
    {
        try { await _mediator.Send(new ConfirmHorizontalPromotionCommand { TransactionId = transId }, cancellationToken); return Ok(); }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (Exception ex) { _logger.LogError(ex, "Error"); return StatusCode(500, "Internal server error"); }
    }
    #endregion

    #region VTCCorrection Endpoints
    /// <summary>Get VTC correction by ID</summary>
    [HttpGet("vtccorrections/{rateId}")]
    [Authorize(Roles = "HR,Admin")]
    [ProducesResponseType(typeof(VTCCorrectionDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetVTCCorrection(decimal rateId, CancellationToken cancellationToken)
    {
        try { return Ok(await _mediator.Send(new GetVTCCorrectionByIdQuery { RateId = rateId }, cancellationToken)); }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (Exception ex) { _logger.LogError(ex, "Error"); return StatusCode(500, "Internal server error"); }
    }

    /// <summary>Get pending VTC corrections</summary>
    [HttpGet("vtccorrections/pending")]
    [Authorize(Roles = "HR,Admin")]
    [ProducesResponseType(typeof(IEnumerable<VTCCorrectionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPendingVTCCorrections([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try { return Ok(await _mediator.Send(new GetPendingVTCCorrectionsQuery { PageNumber = pageNumber, PageSize = pageSize }, cancellationToken)); }
        catch (Exception ex) { _logger.LogError(ex, "Error"); return StatusCode(500, "Internal server error"); }
    }

    /// <summary>Create VTC correction request</summary>
    [HttpPost("vtccorrections")]
    [Authorize(Roles = "HR,Admin")]
    [ProducesResponseType(typeof(VTCCorrectionDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateVTCCorrection([FromBody] CreateVTCCorrectionDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new CreateVTCCorrectionCommand
            {
                EmployeeSystemId = dto.EmployeeSystemId,
                FinancialYearId = dto.FinancialYearId,
                GradeId = dto.GradeId,
                OldRating = dto.OldRating,
                NewRating = dto.NewRating,
                OldCash = dto.OldCash,
                NewCash = dto.NewCash,
                OldPromotion = dto.OldPromotion,
                NewPromotion = dto.NewPromotion,
                OldRationalization = dto.OldRationalization,
                NewRationalization = dto.NewRationalization,
                Reason = dto.Reason,
                CreatedBy = dto.CreatedBy
            }, cancellationToken);
            return CreatedAtAction(nameof(GetVTCCorrection), new { rateId = result.RateId }, result);
        }
        catch (Exception ex) { _logger.LogError(ex, "Error creating VTC correction"); return BadRequest("Failed"); }
    }

    /// <summary>Approve VTC correction</summary>
    [HttpPost("vtccorrections/{rateId}/approve")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ApproveVTCCorrection(decimal rateId, [FromBody] ApproveVTCCorrectionDto dto, CancellationToken cancellationToken)
    {
        try { await _mediator.Send(new ApproveVTCCorrectionCommand { RateId = rateId, ApprovedBy = dto.ApprovedBy }, cancellationToken); return Ok(); }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (Exception ex) { _logger.LogError(ex, "Error"); return StatusCode(500, "Internal server error"); }
    }
    #endregion

    #region AppraisalAmount Endpoints
    /// <summary>Get appraisal amounts (HR/Admin)</summary>
    [HttpGet("appraisalamounts")]
    [Authorize(Roles = "HR,Admin")]
    [ProducesResponseType(typeof(IEnumerable<AppraisalAmountDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAppraisalAmounts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
    {
        try { return Ok(await _mediator.Send(new GetAllAppraisalAmountsQuery { PageNumber = pageNumber, PageSize = pageSize }, cancellationToken)); }
        catch (Exception ex) { _logger.LogError(ex, "Error"); return StatusCode(500, "Internal server error"); }
    }

    /// <summary>Get appraisal amounts by band</summary>
    [HttpGet("appraisalamounts/band/{bandId}")]
    [Authorize(Roles = "HR,Admin")]
    [ProducesResponseType(typeof(IEnumerable<AppraisalAmountDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAppraisalAmountsByBand(decimal bandId, CancellationToken cancellationToken)
    {
        try { return Ok(await _mediator.Send(new GetAppraisalAmountsByBandQuery { BandId = bandId }, cancellationToken)); }
        catch (Exception ex) { _logger.LogError(ex, "Error"); return StatusCode(500, "Internal server error"); }
    }
    #endregion
}
