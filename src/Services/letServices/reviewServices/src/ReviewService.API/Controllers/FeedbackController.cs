using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReviewService.Application.Commands.SubmitFeedback;
using ReviewService.Application.DTOs;
using ReviewService.Application.Queries.GetCourseReviews;
using ReviewService.Application.Queries.GetFeedbackSummary;

namespace ReviewService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FeedbackController : ControllerBase
{
    private readonly IMediator _mediator;

    public FeedbackController(IMediator mediator) => _mediator = mediator;

    /// <summary>Gets all feedbacks for a course.</summary>
    [HttpGet("course/{courseId:long}")]
    [ProducesResponseType(typeof(IEnumerable<CourseFeedbackDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCourseReviews(long courseId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCourseReviewsQuery(courseId), cancellationToken);
        return Ok(result);
    }

    /// <summary>Gets feedback summary (count + average rating) for a course.</summary>
    [HttpGet("course/{courseId:long}/summary")]
    [ProducesResponseType(typeof(FeedbackSummaryDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFeedbackSummary(long courseId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetFeedbackSummaryQuery(courseId), cancellationToken);
        return Ok(result);
    }

    /// <summary>Submits feedback for a course.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(CourseFeedbackDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> SubmitFeedback(
        [FromBody] SubmitFeedbackCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetCourseReviews), new { courseId = result.CourseId }, result);
    }
}
