using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSCTransactional.Application.DTOs;
using SSCTransactional.Application.Queries.Oracle;

namespace SSCTransactional.API.Controllers;

[ApiController]
[Route("api/v1/oracle")]
[Authorize]
public class OracleInvoicesController : ControllerBase
{
    private readonly IMediator _mediator;
    public OracleInvoicesController(IMediator mediator) => _mediator = mediator;

    [HttpGet("invoices/doc/{docId:long}")]
    [ProducesResponseType(typeof(IEnumerable<OracleInvoiceDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetInvoicesByDocId(long docId, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetOracleInvoicesByDocIdQuery(docId), ct);
        return Ok(result);
    }

    [HttpGet("payments/doc/{docId:long}")]
    [ProducesResponseType(typeof(IEnumerable<OraclePaymentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaymentsByDocId(long docId, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetOraclePaymentsByDocIdQuery(docId), ct);
        return Ok(result);
    }

    [HttpGet("bank-details/doc/{docId:long}")]
    [ProducesResponseType(typeof(IEnumerable<OracleBankDetailDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBankDetailsByDocId(long docId, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetOracleBankDetailsByDocIdQuery(docId), ct);
        return Ok(result);
    }

    [HttpGet("due-details/doc/{docId:long}")]
    [ProducesResponseType(typeof(IEnumerable<OracleDueDetailDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDueDetailsByDocId(long docId, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetOracleDueDetailsByDocIdQuery(docId), ct);
        return Ok(result);
    }
}

[ApiController]
[Route("api/v1/statuses")]
[Authorize]
public class DocumentStatusesController : ControllerBase
{
    private readonly IMediator _mediator;
    public DocumentStatusesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DocumentStatusDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetDocumentStatusesQuery(), ct);
        return Ok(result);
    }

    [HttpGet("type/{docType}")]
    [ProducesResponseType(typeof(IEnumerable<DocumentStatusDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByType(string docType, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetDocumentStatusesByTypeQuery(docType), ct);
        return Ok(result);
    }
}
