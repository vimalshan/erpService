using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookingService.Application.Commands;
using BookingService.Application.DTOs;
using BookingService.Application.Queries;

namespace BookingService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookingsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<BookRequestMainDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllBookingsQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BookRequestMainDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(string id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetBookingByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("employee/{employeeSysId}")]
    [ProducesResponseType(typeof(IReadOnlyList<BookRequestMainDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEmployee(string employeeSysId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetBookingsByEmployeeQuery(employeeSysId), ct);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(BookRequestMainDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateBookingCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.BookMainId }, result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(BookRequestMainDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateBookingCommand command, CancellationToken ct)
    {
        if (id != command.BookMainId)
            return BadRequest("ID mismatch");

        var result = await mediator.Send(command, ct);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string id, CancellationToken ct)
    {
        await mediator.Send(new DeleteBookingCommand(id), ct);
        return NoContent();
    }

    [HttpPost("{id}/approve")]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Approve(string id, [FromQuery] string approvedBy, CancellationToken ct)
    {
        await mediator.Send(new ApproveBookingCommand(id, approvedBy), ct);
        return Ok(new { message = "Booking approved" });
    }

    [HttpPost("{id}/confirm")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(BookConfirmationDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Confirm(string id, [FromBody] ConfirmBookingCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPost("{id}/cancel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Cancel(string id, [FromQuery] string reason, CancellationToken ct)
    {
        await mediator.Send(new CancelBookingCommand(id, reason), ct);
        return Ok(new { message = "Booking cancelled" });
    }

    [HttpGet("{id}/confirmations")]
    [ProducesResponseType(typeof(IReadOnlyList<BookConfirmationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetConfirmations(string id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetBookingConfirmationsQuery(id), ct);
        return Ok(result);
    }
}
