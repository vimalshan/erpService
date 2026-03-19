using InvoiceProcessing.Application.DTOs;
using InvoiceProcessing.Application.Features.Documents.Commands;
using InvoiceProcessing.Application.Features.Documents.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceProcessing.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DocumentsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<DocumentDetailDto>>> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllDocumentsQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<DocumentDetailDto>> GetById(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetDocumentByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("paged")]
    public async Task<ActionResult<PagedResultDto<DocumentDetailDto>>> GetPaged(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
        [FromQuery] string? orgId = null, [FromQuery] string? status = null,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetPagedDocumentsQuery(page, pageSize, orgId, status), ct);
        return Ok(result);
    }

    [HttpGet("org/{orgId}")]
    public async Task<ActionResult<IReadOnlyList<DocumentDetailDto>>> GetByOrg(string orgId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetDocumentsByOrgQuery(orgId), ct);
        return Ok(result);
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<IReadOnlyList<DocumentDetailDto>>> GetByStatus(string status, CancellationToken ct)
    {
        var result = await mediator.Send(new GetDocumentsByStatusQuery(status), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<DocumentDetailDto>> Create([FromBody] CreateDocumentCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.DocId }, result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<DocumentDetailDto>> Update(long id, [FromBody] UpdateDocumentCommand command, CancellationToken ct)
    {
        if (id != command.Id) return BadRequest("ID mismatch");
        var result = await mediator.Send(command, ct);
        return Ok(result);
    }

    [HttpPost("{id:long}/submit")]
    public async Task<ActionResult<DocumentDetailDto>> Submit(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new SubmitDocumentCommand(id), ct);
        return Ok(result);
    }

    [HttpPost("{id:long}/approve")]
    public async Task<ActionResult<DocumentDetailDto>> Approve(long id, [FromQuery] long approvedBy, CancellationToken ct)
    {
        var result = await mediator.Send(new ApproveDocumentCommand(id, approvedBy), ct);
        return Ok(result);
    }

    [HttpPost("{id:long}/cancel")]
    public async Task<ActionResult<DocumentDetailDto>> Cancel(long id, [FromQuery] long cancelledBy, [FromQuery] string? remarks, CancellationToken ct)
    {
        var result = await mediator.Send(new CancelDocumentCommand(id, cancelledBy, remarks), ct);
        return Ok(result);
    }

    [HttpPost("{id:long}/hold")]
    public async Task<ActionResult<DocumentDetailDto>> Hold(long id, [FromQuery] string? remarks, CancellationToken ct)
    {
        var result = await mediator.Send(new HoldDocumentCommand(id, remarks), ct);
        return Ok(result);
    }

    [HttpPost("{id:long}/release-hold")]
    public async Task<ActionResult<DocumentDetailDto>> ReleaseHold(long id, [FromQuery] string? remarks, CancellationToken ct)
    {
        var result = await mediator.Send(new ReleaseHoldCommand(id, remarks), ct);
        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult> Delete(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteDocumentCommand(id), ct);
        return result ? NoContent() : NotFound();
    }
}
