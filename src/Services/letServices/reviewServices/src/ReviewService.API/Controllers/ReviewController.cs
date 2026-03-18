using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReviewService.Application.Commands.AddReviewDetail;
using ReviewService.Application.Commands.SubmitCourseReview;
using ReviewService.Application.DTOs;
using ReviewService.Application.Queries.GetReviewById;

namespace ReviewService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReviewController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReviewController(IMediator mediator) => _mediator = mediator;

    /// <summary>Gets a review by serial number.</summary>
    [HttpGet("{srlNum:long}")]
    [ProducesResponseType(typeof(ReviewMainDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long srlNum, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetReviewByIdQuery(srlNum), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Submits a new course review.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ReviewMainDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> SubmitCourseReview(
        [FromBody] SubmitCourseReviewCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { srlNum = result.RevSrlNum }, result);
    }

    /// <summary>Adds a review detail sub-record.</summary>
    [HttpPost("{srlNum:long}/details")]
    [ProducesResponseType(typeof(ReviewSubDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddReviewDetail(
        long srlNum, [FromBody] AddReviewDetailRequest request, CancellationToken cancellationToken)
    {
        var command = new AddReviewDetailCommand(
            srlNum, request.ReviewNum, request.ReviewDate,
            request.ReviewedBy, request.ReviewStatus, request.Remarks);
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { srlNum }, result);
    }
}

public record AddReviewDetailRequest(
    long ReviewNum,
    DateTime ReviewDate,
    long ReviewedBy,
    string ReviewStatus,
    string? Remarks);
