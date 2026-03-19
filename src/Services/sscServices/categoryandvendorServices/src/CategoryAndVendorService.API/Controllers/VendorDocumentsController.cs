using CategoryAndVendorService.Application.DTOs;
using CategoryAndVendorService.Application.VendorDocuments.Commands;
using CategoryAndVendorService.Application.VendorDocuments.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CategoryAndVendorService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VendorDocumentsController : ControllerBase
{
    private readonly IMediator _mediator;
    public VendorDocumentsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<VendorDocumentDto>>> GetAll(CancellationToken ct)
        => Ok(await _mediator.Send(new GetAllVendorDocumentsQuery(), ct));

    [HttpGet("{id:long}")]
    public async Task<ActionResult<VendorDocumentDto>> GetById(long id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetVendorDocumentByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("by-vendor/{vendorId:long}")]
    public async Task<ActionResult<IReadOnlyList<VendorDocumentDto>>> GetByVendor(long vendorId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetVendorDocumentsByVendorIdQuery(vendorId), ct));

    [HttpPost]
    public async Task<ActionResult<VendorDocumentDto>> Create([FromBody] CreateVendorDocumentCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.VndDocId }, result);
    }

    [HttpPost("{id:long}/approve")]
    public async Task<ActionResult<VendorDocumentDto>> Approve(long id, [FromBody] ApproveVendorDocumentCommand command, CancellationToken ct)
    {
        if (id != command.VndDocId) return BadRequest("ID mismatch");
        return Ok(await _mediator.Send(command, ct));
    }

    [HttpPost("{id:long}/reject")]
    public async Task<ActionResult<VendorDocumentDto>> Reject(long id, [FromBody] RejectVendorDocumentCommand command, CancellationToken ct)
    {
        if (id != command.VndDocId) return BadRequest("ID mismatch");
        return Ok(await _mediator.Send(command, ct));
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteVendorDocumentCommand(id), ct);
        return result ? NoContent() : NotFound();
    }
}
