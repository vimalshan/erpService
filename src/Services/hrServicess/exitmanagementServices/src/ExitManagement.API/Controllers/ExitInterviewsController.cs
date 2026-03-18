using ExitManagement.Application.Features.ExitInterviews.Commands;
using ExitManagement.Application.Features.ExitInterviews.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExitManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class ExitInterviewsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExitInterviewsController(IMediator mediator) => _mediator = mediator;

    /// <summary>Gets interview feedback for an exit.</summary>
    [HttpGet("{exitNo:decimal}/feedback")]
    public async Task<IActionResult> GetFeedback(decimal exitNo, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetInterviewFeedbackQuery(exitNo), ct);
        return Ok(result);
    }

    /// <summary>Submits interview feedback for an exit.</summary>
    [HttpPost("feedback")]
    public async Task<IActionResult> SubmitFeedback([FromBody] SubmitInterviewFeedbackCommand command, CancellationToken ct)
    {
        await _mediator.Send(command, ct);
        return Ok(new { message = "Feedback submitted successfully." });
    }
}
