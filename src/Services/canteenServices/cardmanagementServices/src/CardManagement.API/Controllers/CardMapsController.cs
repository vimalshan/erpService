using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CardManagement.Application.CardMaps.Queries.GetCardMaps;
using CardManagement.Application.Common.DTOs;

namespace CardManagement.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CardMapsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CardMapsController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get card maps for a canteen unit.</summary>
    [HttpGet("{canteenUnit:long}")]
    [ProducesResponseType(typeof(IEnumerable<CanteenCardMapDto>), 200)]
    public async Task<IActionResult> Get(long canteenUnit, [FromQuery] bool activeOnly = false, CancellationToken ct = default)
        => Ok(await _mediator.Send(new GetCardMapsQuery(canteenUnit, activeOnly), ct));
}
