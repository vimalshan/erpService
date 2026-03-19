using CategoryAndVendorService.Application.DTOs;
using CategoryAndVendorService.Application.SupportDocuments.Commands;
using CategoryAndVendorService.Application.SupportDocuments.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CategoryAndVendorService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SupportDocumentsController : ControllerBase
{
    private readonly IMediator _mediator;
    public SupportDocumentsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SupportDocumentDto>>> GetAll(CancellationToken ct)
        => Ok(await _mediator.Send(new GetAllSupportDocumentsQuery(), ct));

    [HttpGet("{id:long}")]
    public async Task<ActionResult<SupportDocumentDto>> GetById(long id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetSupportDocumentByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<SupportDocumentDto>> Create([FromBody] CreateSupportDocumentCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.DocId }, result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteSupportDocumentCommand(id), ct);
        return result ? NoContent() : NotFound();
    }
}
