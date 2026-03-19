using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PurchaseSalesService.Application.DTOs;
using PurchaseSalesService.Application.Purchases.Commands.CancelPurchase;
using PurchaseSalesService.Application.Purchases.Commands.CreatePurchase;
using PurchaseSalesService.Application.Purchases.Queries.GetAllPurchases;
using PurchaseSalesService.Application.Purchases.Queries.GetPurchaseById;

namespace PurchaseSalesService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public sealed class PurchasesController : ControllerBase
{
    private readonly IMediator _mediator;

    public PurchasesController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get all purchase records.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PurchaseDetailDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _mediator.Send(new GetAllPurchasesQuery(), ct));

    /// <summary>Get a single purchase by serial number.</summary>
    [HttpGet("{serialNumber:long}")]
    [ProducesResponseType(typeof(PurchaseDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long serialNumber, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetPurchaseByIdQuery(serialNumber), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Create a new purchase record.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(PurchaseDetailDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreatePurchaseCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { serialNumber = result.SerialNumber }, result);
    }

    /// <summary>Cancel a purchase record.</summary>
    [HttpPatch("{serialNumber:long}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(long serialNumber, [FromQuery] string cancelledBy, CancellationToken ct)
    {
        await _mediator.Send(new CancelPurchaseCommand(serialNumber, cancelledBy), ct);
        return NoContent();
    }
}
