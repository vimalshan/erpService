using IntegrationService.Application.DTOs;
using IntegrationService.Application.PurchaseOrders.Commands;
using IntegrationService.Application.PurchaseOrders.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PurchaseOrdersController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PurchaseOrderDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllPurchaseOrdersQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<PurchaseOrderDto>> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetPurchaseOrderByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("{id:long}/with-mrc")]
    public async Task<ActionResult<PurchaseOrderDto>> GetWithMrc(long id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetPurchaseOrderWithMrcQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<PurchaseOrderDto>> Create(
        [FromBody] CreatePurchaseOrderCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.PoSeqId }, result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<PurchaseOrderDto>> Update(long id,
        [FromBody] UpdatePurchaseOrderCommand command, CancellationToken cancellationToken)
    {
        if (id != command.PoSeqId) return BadRequest("ID mismatch");
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeletePurchaseOrderCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:long}/material-receipts")]
    public async Task<ActionResult<MaterialReceiptDto>> AddMaterialReceipt(long id,
        [FromBody] AddMaterialReceiptCommand command, CancellationToken cancellationToken)
    {
        if (id != command.PurchaseOrderId) return BadRequest("PO ID mismatch");
        var result = await mediator.Send(command, cancellationToken);
        return Created($"api/purchaseorders/{id}/material-receipts/{result.MrcSeqId}", result);
    }
}
