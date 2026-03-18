using MediatR;
using MedicineManagement.Application.DTOs;
using MedicineManagement.Application.Features.Purchases.Commands;
using MedicineManagement.Application.Features.Purchases.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicineManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PurchasesController(IMediator mediator) : ControllerBase
{
    [HttpGet("{companyCode}/{transactionNumber}")]
    public async Task<ActionResult<PurchaseMainDto>> GetById(string companyCode, long transactionNumber, CancellationToken ct)
    {
        var result = await mediator.Send(new GetPurchaseByIdQuery(companyCode, transactionNumber), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("by-date")]
    public async Task<ActionResult<IReadOnlyList<PurchaseMainDto>>> GetByDateRange(
        [FromQuery] DateTime from, [FromQuery] DateTime to, CancellationToken ct)
        => Ok(await mediator.Send(new GetPurchasesByDateRangeQuery(from, to), ct));

    [HttpGet("by-vendor")]
    public async Task<ActionResult<IReadOnlyList<PurchaseMainDto>>> GetByVendor(
        [FromQuery] string vendorName, CancellationToken ct)
        => Ok(await mediator.Send(new GetPurchasesByVendorQuery(vendorName), ct));

    [HttpPost]
    public async Task<ActionResult<PurchaseMainDto>> Create([FromBody] CreatePurchaseDto dto, CancellationToken ct)
    {
        var user = User.Identity?.Name ?? "API";
        var result = await mediator.Send(new CreatePurchaseCommand(
            dto.CompanyCode, dto.TransactionNumber, dto.VendorName,
            dto.InvoiceNumber, dto.InvoiceDate, dto.InvoiceAmount,
            user, 0, dto.LineItems), ct);
        return CreatedAtAction(nameof(GetById),
            new { companyCode = result.CompanyCode, transactionNumber = result.TransactionNumber }, result);
    }

    [HttpPost("{companyCode}/{transactionNumber}/cancel")]
    public async Task<IActionResult> Cancel(string companyCode, long transactionNumber, CancellationToken ct)
    {
        var user = User.Identity?.Name ?? "API";
        await mediator.Send(new CancelPurchaseCommand(companyCode, transactionNumber, user, 0), ct);
        return NoContent();
    }
}
