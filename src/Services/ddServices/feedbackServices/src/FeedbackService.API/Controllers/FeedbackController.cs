namespace FeedbackService.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Commands;
using Application.Queries;
using Application.DTOs;

/// <summary>
/// REST API controller for feedback operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class FeedbackController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the FeedbackController class
    /// </summary>
    public FeedbackController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Creates a new feedback
    /// </summary>
    /// <param name="command">Create feedback command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created feedback</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FeedbackDto>> CreateFeedback(
        [FromBody] CreateFeedbackCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetFeedback), new { id = result.Id }, result);
    }

    /// <summary>
    /// Gets feedback by ID
    /// </summary>
    /// <param name="id">Feedback ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The feedback</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FeedbackDto>> GetFeedback(
        decimal id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetFeedbackByIdQuery { FeedbackId = id }, cancellationToken);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Gets all feedback with pagination
    /// </summary>
    /// <param name="pageNumber">Page number (1-based)</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="status">Filter by status</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of feedback</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<FeedbackDto>>> GetAllFeedback(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? status = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new GetAllFeedbackQuery 
            { 
                PageNumber = pageNumber,
                PageSize = pageSize,
                StatusFilter = status
            },
            cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Gets feedback by request number
    /// </summary>
    /// <param name="requestNo">Request number</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of feedback for the request</returns>
    [HttpGet("by-request/{requestNo}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<FeedbackDto>>> GetFeedbackByRequestNo(
        decimal requestNo,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetFeedbackByRequestNoQuery { RequestNo = requestNo },
            cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Adds an item to feedback
    /// </summary>
    /// <param name="command">Add feedback item command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated feedback</returns>
    [HttpPost("items")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FeedbackDto>> AddFeedbackItem(
        [FromBody] AddFeedbackItemCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Submits feedback
    /// </summary>
    /// <param name="feedbackId">Feedback ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The submitted feedback</returns>
    [HttpPost("{feedbackId}/submit")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FeedbackDto>> SubmitFeedback(
        decimal feedbackId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new SubmitFeedbackCommand { FeedbackId = feedbackId },
            cancellationToken);
        return Ok(result);
    }
}
