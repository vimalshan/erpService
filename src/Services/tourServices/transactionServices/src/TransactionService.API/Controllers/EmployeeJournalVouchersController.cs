using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransactionService.Application.EmployeeJournalVouchers.Commands.CreateEmployeeJV;
using TransactionService.Application.EmployeeJournalVouchers.Commands.PostEmployeeJV;
using TransactionService.Application.EmployeeJournalVouchers.Commands.ReverseEmployeeJV;
using TransactionService.Application.EmployeeJournalVouchers.Queries;

namespace TransactionService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public sealed class EmployeeJournalVouchersController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeeJournalVouchersController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] long? employeeId = null,
        [FromQuery] string? status = null,
        CancellationToken ct = default)
        => Ok(await _mediator.Send(new GetAllEmployeeJVsQuery(page, pageSize, employeeId, status), ct));

    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetEmployeeJVByIdQuery(id), ct));

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeJVCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.JvBatchId }, result);
    }

    [HttpPatch("{id:long}/post")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Post(long id, [FromQuery] string? oracleRefNo, [FromQuery] long postedBy, CancellationToken ct)
    {
        await _mediator.Send(new PostEmployeeJVCommand(id, oracleRefNo, postedBy), ct);
        return NoContent();
    }

    [HttpPatch("{id:long}/reverse")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Reverse(long id, [FromQuery] long reversedBy, CancellationToken ct)
    {
        await _mediator.Send(new ReverseEmployeeJVCommand(id, reversedBy), ct);
        return NoContent();
    }
}
