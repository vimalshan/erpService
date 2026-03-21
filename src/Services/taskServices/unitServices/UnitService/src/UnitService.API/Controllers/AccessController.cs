using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnitService.Application.Commands.GrantAccess;

namespace UnitService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AccessController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccessController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<int>> GrantAccess([FromBody] GrantAccessCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
