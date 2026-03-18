using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.Features.UserProfileHist.Queries;

namespace UserManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class ProfileHistoryController(IMediator mediator) : ControllerBase
{
    /// <summary>Get profile change history for a specific user.</summary>
    [HttpGet("user/{userSysId:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUser(long userSysId, CancellationToken ct)
        => Ok(await mediator.Send(new GetProfileHistoryByUserQuery(userSysId), ct));

    /// <summary>Get profile change history for a specific policy.</summary>
    [HttpGet("policy/{policyId:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByPolicy(long policyId, CancellationToken ct)
        => Ok(await mediator.Send(new GetProfileHistoryByPolicyQuery(policyId), ct));
}
