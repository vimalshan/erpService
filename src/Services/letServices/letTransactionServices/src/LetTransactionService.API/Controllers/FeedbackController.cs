using LetTransactionService.Application.Commands.CancelFeedback;
using LetTransactionService.Application.Commands.SubmitFeedback;
using LetTransactionService.Application.DTOs;
using LetTransactionService.Application.Queries.GetFeedback;
using LetTransactionService.Application.Queries.GetFeedbacks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LetTransactionService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class FeedbackController(IMediator mediator) : ControllerBase
{
    /// <summary>Get all feedback records with optional course filtering and pagination.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<FeedbackSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetFeedbacksQuery(page, pageSize), ct);
        return Ok(result);
    }

    /// <summary>Get a specific feedback by its number.</summary>
    [HttpGet("{feedbackNumber:long}")]
    [ProducesResponseType(typeof(FeedbackMainDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long feedbackNumber, CancellationToken ct)
    {
        var result = await mediator.Send(new GetFeedbackQuery(feedbackNumber), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Submit new course feedback.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(FeedbackMainDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Submit([FromBody] SubmitFeedbackCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { feedbackNumber = result.FeedbackNumber }, result);
    }

    /// <summary>Cancel an existing feedback.</summary>
    [HttpPost("{feedbackNumber:long}/cancel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(long feedbackNumber, [FromBody] CancelFeedbackInput input, CancellationToken ct)
    {
        await mediator.Send(new CancelFeedbackCommand(feedbackNumber, input.CancelRemark), ct);
        return Ok(new { message = "Feedback cancelled successfully." });
    }
}

public record CancelFeedbackInput(string CancelRemark);
