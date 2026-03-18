using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ItemMasterService.Application.CQRS.Commands;
using ItemMasterService.Application.CQRS.Queries;
using ItemMasterService.Application.DTOs;

namespace ItemMasterService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class CanteenItemMasterController : ControllerBase
{
    private readonly IMediator _mediator;

    public CanteenItemMasterController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get all canteen items for a unit.</summary>
    [HttpGet("{canteenUnitCode}")]
    [ProducesResponseType(typeof(IEnumerable<CanteenItemMasterDto>), 200)]
    public async Task<IActionResult> GetAll(long canteenUnitCode, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllCanteenItemsQuery(canteenUnitCode), ct);
        return Ok(result);
    }

    /// <summary>Get a specific canteen item.</summary>
    [HttpGet("{canteenUnitCode}/{itemCode}")]
    [ProducesResponseType(typeof(CanteenItemMasterDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(long canteenUnitCode, long itemCode, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetCanteenItemByIdQuery(canteenUnitCode, itemCode), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Create a new canteen item.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(CanteenItemMasterDto), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(409)]
    public async Task<IActionResult> Create([FromBody] CreateCanteenItemCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { canteenUnitCode = result.CanteenUnitCode, itemCode = result.ItemCode }, result);
    }

    /// <summary>Update an existing canteen item.</summary>
    [HttpPut("{canteenUnitCode}/{itemCode}")]
    [ProducesResponseType(typeof(CanteenItemMasterDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(long canteenUnitCode, long itemCode, [FromBody] UpdateCanteenItemCommand command, CancellationToken ct)
    {
        if (command.CanteenUnitCode != canteenUnitCode || command.ItemCode != itemCode)
            return BadRequest("Route parameters do not match body.");

        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }

    /// <summary>Delete a canteen item.</summary>
    [HttpDelete("{canteenUnitCode}/{itemCode}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(long canteenUnitCode, long itemCode, CancellationToken ct)
    {
        await _mediator.Send(new DeleteCanteenItemCommand(canteenUnitCode, itemCode), ct);
        return NoContent();
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class CanteenItemPriceController : ControllerBase
{
    private readonly IMediator _mediator;

    public CanteenItemPriceController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get active price for item.</summary>
    [HttpGet("{canteenUnitCode}/{itemCode}/active")]
    [ProducesResponseType(typeof(CanteenItemPriceMasterDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetActivePrice(long canteenUnitCode, long itemCode, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetItemPriceQuery(canteenUnitCode, itemCode), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Get price history for item.</summary>
    [HttpGet("{canteenUnitCode}/{itemCode}/history")]
    [ProducesResponseType(typeof(IEnumerable<CanteenItemPriceMasterDto>), 200)]
    public async Task<IActionResult> GetPriceHistory(long canteenUnitCode, long itemCode, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetItemPriceHistoryQuery(canteenUnitCode, itemCode), ct);
        return Ok(result);
    }

    /// <summary>Create a new item price record.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(CanteenItemPriceMasterDto), 201)]
    public async Task<IActionResult> CreatePrice([FromBody] CreateItemPriceCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetActivePrice), new { canteenUnitCode = result.CanteenUnitCode, itemCode = result.ItemCode }, result);
    }

    /// <summary>Close an active price record.</summary>
    [HttpPatch("{canteenUnitCode}/{itemCode}/close")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> ClosePrice(long canteenUnitCode, long itemCode, [FromBody] DateTime closureDate, CancellationToken ct)
    {
        await _mediator.Send(new CloseItemPriceCommand(canteenUnitCode, itemCode, closureDate), ct);
        return NoContent();
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class CanteenGradeItemPriceController : ControllerBase
{
    private readonly IMediator _mediator;

    public CanteenGradeItemPriceController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await _mediator.Send(new GetAllGradeItemPricesQuery(), ct));

    [HttpGet("{canteenUnitCode}")]
    public async Task<IActionResult> GetByUnit(long canteenUnitCode, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetGradeItemPriceQuery(canteenUnitCode), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGradeItemPriceCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetByUnit), new { canteenUnitCode = result.CanteenUnitCode }, result);
    }

    [HttpPut("{canteenUnitCode}")]
    public async Task<IActionResult> Update(long canteenUnitCode, [FromBody] UpdateGradeItemPriceCommand command, CancellationToken ct)
    {
        if (command.CanteenUnitCode != canteenUnitCode) return BadRequest();
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }
}
