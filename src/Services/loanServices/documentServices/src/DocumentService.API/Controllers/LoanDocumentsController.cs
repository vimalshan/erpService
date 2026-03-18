using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DocumentService.Application.Commands.CreateLoanDocument;
using DocumentService.Application.Commands.DeleteLoanDocument;
using DocumentService.Application.Commands.UpdateLoanDocument;
using DocumentService.Application.DTOs;
using DocumentService.Application.Queries.GetAllLoanDocuments;
using DocumentService.Application.Queries.GetLoanDocumentById;
using DocumentService.Application.Queries.GetLoanDocumentsByLoanId;

namespace DocumentService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class LoanDocumentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public LoanDocumentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Gets all loan documents.</summary>
    [HttpGet]
    [ProducesResponseType<IEnumerable<LoanDocumentDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllLoanDocumentsQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>Gets a loan document by ID.</summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType<LoanDocumentDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetLoanDocumentByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Gets all documents for a loan.</summary>
    [HttpGet("loan/{loanId:long}")]
    [ProducesResponseType<IEnumerable<LoanDocumentDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByLoanId(long loanId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetLoanDocumentsByLoanIdQuery(loanId), cancellationToken);
        return Ok(result);
    }

    /// <summary>Creates a new loan document.</summary>
    [HttpPost]
    [ProducesResponseType<LoanDocumentDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateLoanDocumentCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Updates an existing loan document.</summary>
    [HttpPut("{id:long}")]
    [ProducesResponseType<LoanDocumentDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateLoanDocumentRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateLoanDocumentCommand(id, request.TypeId, request.ModifiedBy), cancellationToken);
        return Ok(result);
    }

    /// <summary>Deletes a loan document.</summary>
    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteLoanDocumentCommand(id), cancellationToken);
        return NoContent();
    }
}

public record UpdateLoanDocumentRequest(long TypeId, long ModifiedBy);
