using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProblemManagement.Application.Queries;

namespace ProblemManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LookupController(IMediator mediator) : ControllerBase
{
    [HttpGet("functions")]
    public async Task<IActionResult> GetFunctions(CancellationToken ct)
    {
        var result = await mediator.Send(new GetProblemFunctionsQuery(), ct);
        return Ok(result);
    }

    [HttpGet("impacts")]
    public async Task<IActionResult> GetImpacts(CancellationToken ct)
    {
        var result = await mediator.Send(new GetProblemImpactsQuery(), ct);
        return Ok(result);
    }
}
