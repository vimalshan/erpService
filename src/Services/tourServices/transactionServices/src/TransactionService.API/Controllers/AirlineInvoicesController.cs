using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransactionService.Application.AirlineInvoices;

namespace TransactionService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public sealed class AirlineInvoicesController : ControllerBase
{
    private readonly IMediator _mediator;

    public AirlineInvoicesController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(string id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetAirlineInvoiceByIdQuery(id), ct));

    [HttpGet("by-booking/{bookCnfId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByBooking(string bookCnfId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetAirlineInvoicesByBookingQuery(bookCnfId), ct));

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateAirlineInvoiceCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.AirTicketId }, result);
    }
}
