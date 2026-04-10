using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSCTransactional.Application.Commands.Revoke;
using SSCTransactional.Application.DTOs;

namespace SSCTransactional.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class RevokesController : ControllerBase
{
    private readonly IMediator _mediator;
    public RevokesController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(typeof(RevokeDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateRevokeCommand command, CancellationToken ct = default)
    {
        var result = await _mediator.Send(command, ct);
        return Created("", result);
    }
}
