using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CardManagement.Application.Cards.Commands.SettleCard;
using CardManagement.Application.Common.DTOs;

namespace CardManagement.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class SettlementsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SettlementsController(IMediator mediator) => _mediator = mediator;

    /// <summary>Create a card settlement record.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(CardSettlementDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] SettleCardCommand command, CancellationToken ct = default)
    {
        var result = await _mediator.Send(command, ct);
        return Created(string.Empty, result);
    }
}
