using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransactionService.Application.SupplierJournalVouchers.Commands.CreateSupplierJV;
using TransactionService.Application.SupplierJournalVouchers.Commands.PostSupplierJV;
using TransactionService.Application.SupplierJournalVouchers.Queries;

namespace TransactionService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public sealed class SupplierJournalVouchersController : ControllerBase
{
    private readonly IMediator _mediator;

    public SupplierJournalVouchersController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] long? vendorId = null,
        CancellationToken ct = default)
        => Ok(await _mediator.Send(new GetAllSupplierJVsQuery(page, pageSize, vendorId), ct));

    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetSupplierJVByIdQuery(id), ct));

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateSupplierJVCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.JvId }, result);
    }

    [HttpPatch("{id:long}/post")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Post(long id, [FromQuery] string? oracleRefNo, [FromQuery] long postedBy, CancellationToken ct)
    {
        await _mediator.Send(new PostSupplierJVCommand(id, oracleRefNo, postedBy), ct);
        return NoContent();
    }
}
