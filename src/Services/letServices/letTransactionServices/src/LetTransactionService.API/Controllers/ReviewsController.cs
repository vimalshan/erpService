using LetTransactionService.Application.Commands.AddReviewSub;
using LetTransactionService.Application.Commands.ApproveReview;
using LetTransactionService.Application.Commands.SubmitReview;
using LetTransactionService.Application.DTOs;
using LetTransactionService.Application.Queries.GetPendingReviews;
using LetTransactionService.Application.Queries.GetReview;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LetTransactionService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class ReviewsController(IMediator mediator) : ControllerBase
{
    /// <summary>Get pending reviews with optional reviewer filtering.</summary>
    [HttpGet("pending")]
    [ProducesResponseType(typeof(IEnumerable<PendingReviewDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPending(
        CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetPendingReviewsQuery(), ct);
        return Ok(result);
    }

    /// <summary>Get a specific review by review number.</summary>
    [HttpGet("{reviewNumber:long}")]
    [ProducesResponseType(typeof(ReviewMainDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long reviewNumber, CancellationToken ct)
    {
        var result = await mediator.Send(new GetReviewQuery(reviewNumber), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Submit a new review.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ReviewMainDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Submit([FromBody] SubmitReviewCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { reviewNumber = result.ReviewSerialNumber }, result);
    }

    /// <summary>Approve a review.</summary>
    [HttpPost("{reviewNumber:long}/approve")]
    [Authorize(Policy = "ReviewerPolicy")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Approve(long reviewNumber, CancellationToken ct)
    {
        await mediator.Send(new ApproveReviewCommand(reviewNumber), ct);
        return Ok(new { message = "Review approved successfully." });
    }

    /// <summary>Add a sub-entry to an existing review.</summary>
    [HttpPost("{reviewNumber:long}/sub")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddSub(long reviewNumber, [FromBody] AddReviewSubCommand command, CancellationToken ct)
    {
        if (command.ReviewNumber != reviewNumber)
            return BadRequest("Review number mismatch.");

        await mediator.Send(command, ct);
        return Ok(new { message = "Review sub-entry added successfully." });
    }
}
