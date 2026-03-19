using HRDocumentService.Application.Commands;
using HRDocumentService.Application.DTOs;
using HRDocumentService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRDocumentService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class HRDocumentsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<HRDocumentDto>>> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllHRDocumentsQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<HRDocumentDto>> GetById(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetHRDocumentByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<IReadOnlyList<HRDocumentDto>>> GetByStatus(string status, CancellationToken ct)
    {
        var result = await mediator.Send(new GetHRDocumentsByStatusQuery(status), ct);
        return Ok(result);
    }

    [HttpGet("{id:long}/files")]
    public async Task<ActionResult<IReadOnlyList<HRDocumentFileDto>>> GetFiles(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetDocumentFilesByDocIdQuery(id), ct);
        return Ok(result);
    }

    [HttpGet("{id:long}/receipts")]
    public async Task<ActionResult<IReadOnlyList<HRDocumentReceiptDto>>> GetReceipts(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetDocumentReceiptsByDocIdQuery(id), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<HRDocumentDto>> Create([FromBody] CreateHRDocumentCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.DocId }, result);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateHRDocumentCommand command, CancellationToken ct)
    {
        if (id != command.DocId) return BadRequest("ID mismatch.");
        var result = await mediator.Send(command, ct);
        return result ? NoContent() : NotFound();
    }

    [HttpPost("{id:long}/approve")]
    public async Task<IActionResult> Approve(long id, [FromBody] ApproveRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new ApproveHRDocumentCommand(id, request.ApprovedBy), ct);
        return result ? NoContent() : NotFound();
    }

    [HttpPost("{id:long}/reject")]
    public async Task<IActionResult> Reject(long id, [FromBody] RejectRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new RejectHRDocumentCommand(id, request.RejectedBy, request.RejectRemarks), ct);
        return result ? NoContent() : NotFound();
    }

    [HttpPost("{id:long}/cancel")]
    public async Task<IActionResult> Cancel(long id, [FromBody] CancelRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new CancelHRDocumentCommand(id, request.CancelledBy), ct);
        return result ? NoContent() : NotFound();
    }

    [HttpPost("{id:long}/upload")]
    public async Task<ActionResult<HRDocumentFileDto>> UploadFile(long id, IFormFile file, CancellationToken ct)
    {
        if (file.Length == 0) return BadRequest("File is empty.");

        using var stream = file.OpenReadStream();
        var command = new UploadDocumentFileCommand(id, file.FileName, file.ContentType, stream);
        var result = await mediator.Send(command, ct);
        return Ok(result);
    }
}

public record ApproveRequest(decimal ApprovedBy);
public record RejectRequest(decimal RejectedBy, string RejectRemarks);
public record CancelRequest(decimal CancelledBy);
