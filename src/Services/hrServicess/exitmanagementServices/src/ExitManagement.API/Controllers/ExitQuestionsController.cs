using ExitManagement.Application.Features.ExitQuestions.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExitManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class ExitQuestionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExitQuestionsController(IMediator mediator) => _mediator = mediator;

    /// <summary>Gets all exit survey questions.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllExitQuestionsQuery(), ct);
        return Ok(result);
    }

    /// <summary>Gets all exit interview questions.</summary>
    [HttpGet("interview")]
    public async Task<IActionResult> GetInterviewQuestions(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllInterviewQuestionsQuery(), ct);
        return Ok(result);
    }
}
