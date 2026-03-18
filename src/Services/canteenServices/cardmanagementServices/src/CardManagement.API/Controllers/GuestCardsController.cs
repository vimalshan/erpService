using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CardManagement.Application.Cards.Commands.CreateGuestCard;
using CardManagement.Application.Cards.Commands.UpdateGuestCard;
using CardManagement.Application.Cards.Commands.CloseGuestCard;
using CardManagement.Application.Cards.Commands.SettleCard;
using CardManagement.Application.Cards.Queries.GetGuestCards;
using CardManagement.Application.Cards.Queries.GetGuestCardById;
using CardManagement.Application.Common.DTOs;
using CardManagement.Application.Common.Models;

namespace CardManagement.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class GuestCardsController : ControllerBase
{
    private readonly IMediator _mediator;

    public GuestCardsController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get all guest cards with optional paging and filtering.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<GuestCardDto>), 200)]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20, [FromQuery] long? canteenUnit = null, CancellationToken ct = default)
        => Ok(await _mediator.Send(new GetGuestCardsQuery(pageNumber, pageSize, canteenUnit), ct));

    /// <summary>Get a guest card by canteen unit (primary key).</summary>
    [HttpGet("{canteenUnit:long}")]
    [ProducesResponseType(typeof(GuestCardDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(long canteenUnit, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetGuestCardByIdQuery(canteenUnit), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Create a new guest card.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(GuestCardDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] CreateGuestCardCommand command, CancellationToken ct = default)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { canteenUnit = result.CanteenUnit }, result);
    }

    /// <summary>Update an existing guest card.</summary>
    [HttpPut("{canteenUnit:long}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(long canteenUnit, [FromBody] UpdateGuestCardCommand command, CancellationToken ct = default)
    {
        if (canteenUnit != command.CanteenUnit) return BadRequest("Route canteen unit does not match body.");
        await _mediator.Send(command, ct);
        return NoContent();
    }

    /// <summary>Close a guest card.</summary>
    [HttpPatch("{canteenUnit:long}/close")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Close(long canteenUnit, CancellationToken ct = default)
    {
        await _mediator.Send(new CloseGuestCardCommand(canteenUnit), ct);
        return NoContent();
    }
}
