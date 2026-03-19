using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PurchaseSalesService.Application.DTOs;
using PurchaseSalesService.Application.Sales.Commands.CancelSale;
using PurchaseSalesService.Application.Sales.Commands.CreateSale;
using PurchaseSalesService.Application.Sales.Queries.GetAllSales;
using PurchaseSalesService.Application.Sales.Queries.GetSaleById;

namespace PurchaseSalesService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public sealed class SalesController : ControllerBase
{
    private readonly IMediator _mediator;

    public SalesController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get all sale records.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SaleMainDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _mediator.Send(new GetAllSalesQuery(), ct));

    /// <summary>Get a single sale by serial number.</summary>
    [HttpGet("{serialNumber:long}")]
    [ProducesResponseType(typeof(SaleMainDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long serialNumber, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetSaleByIdQuery(serialNumber), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Create a new sale record.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(SaleMainDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateSaleCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { serialNumber = result.SerialNumber }, result);
    }

    /// <summary>Cancel a sale record.</summary>
    [HttpPatch("{serialNumber:long}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(long serialNumber, [FromQuery] string cancelledBy, CancellationToken ct)
    {
        await _mediator.Send(new CancelSaleCommand(serialNumber, cancelledBy), ct);
        return NoContent();
    }
}
