using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todos.Application.Commands;
using Todos.Application.DTOs;
using Todos.Application.Queries;

namespace Todos.API.Controllers;

/// <summary>
/// REST API controller for Learning Feedback
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class LearningFeedbackController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<LearningFeedbackController> _logger;

    public LearningFeedbackController(IMediator mediator, ILogger<LearningFeedbackController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Gets learning feedback by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<LearningFeedbackDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<LearningFeedbackDto>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<LearningFeedbackDto>>> GetFeedback(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting learning feedback with ID: {FeedbackId}", id);
        var result = await _mediator.Send(new GetLearningFeedbackByIdQuery { Id = id }, cancellationToken);

        if (!result.Success || result.Data == null)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// Gets all learning feedback records
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<LearningFeedbackDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<LearningFeedbackDto>>>> GetAllFeedback(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting all learning feedback - Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);
        var result = await _mediator.Send(new GetAllLearningFeedbackQuery { PageNumber = pageNumber, PageSize = pageSize }, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Submits learning feedback
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<LearningFeedbackDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<LearningFeedbackDto>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<LearningFeedbackDto>>> SubmitFeedback(
        [FromBody] SubmitLearningFeedbackDto submitDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Submitting feedback for feedback ID: {FeedbackId}", submitDto.FeedbackId);

        var command = new SubmitLearningFeedbackCommand
        {
            FeedbackId = submitDto.FeedbackId,
            TrainingProgram = submitDto.TrainingProgram,
            FeedbackStatus = submitDto.FeedbackStatus,
            AppraiseeComments = submitDto.AppraiseeComments,
            AppraiserComments = submitDto.AppraiserComments,
            ReviewerComments = submitDto.ReviewerComments,
            ModifiedBy = submitDto.ModifiedBy
        };

        var result = await _mediator.Send(command, cancellationToken);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}
