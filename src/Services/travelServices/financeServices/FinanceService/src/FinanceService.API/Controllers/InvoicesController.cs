using FinanceService.Application.DTOs;
using FinanceService.Application.Features.Invoices.Commands.CreateInvoice;
using FinanceService.Application.Features.Invoices.Commands.UpdateInvoice;
using FinanceService.Application.Features.Invoices.Queries.GetAllInvoices;
using FinanceService.Application.Features.Invoices.Queries.GetInvoiceById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InvoicesController : ControllerBase
{
    private readonly IMediator _mediator;

    public InvoicesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<List<InvoiceDto>>> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllInvoicesQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{invoiceId:long}")]
    public async Task<ActionResult<InvoiceDto>> GetById(long invoiceId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetInvoiceByIdQuery(invoiceId), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<InvoiceDto>> Create([FromBody] CreateInvoiceCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { invoiceId = result.InvoiceId }, result);
    }

    [HttpPut("{invoiceId:long}")]
    public async Task<ActionResult<InvoiceDto>> Update(long invoiceId, [FromBody] UpdateInvoiceCommand command, CancellationToken ct)
    {
        if (invoiceId != command.InvoiceId)
            return BadRequest("Invoice ID mismatch.");
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }
}
