using BookingService.Application.Commands.AddAttendee;
using BookingService.Application.Commands.ApproveBooking;
using BookingService.Application.Commands.CancelBooking;
using BookingService.Application.Commands.CreateBooking;
using BookingService.Application.Commands.RejectBooking;
using BookingService.Application.Commands.RemoveAttendee;
using BookingService.Application.Commands.SubmitBooking;
using BookingService.Application.Commands.UpdateBooking;
using BookingService.Application.Common;
using BookingService.Application.DTOs;
using BookingService.Application.Queries.GetAllBookings;
using BookingService.Application.Queries.GetBookingAttendees;
using BookingService.Application.Queries.GetBookingById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class BookingsController(ISender sender) : ControllerBase
{
    /// <summary>Gets a paged list of bookings.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<BookingDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? status = null,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetAllBookingsQuery(page, pageSize, status), cancellationToken);
        return Ok(result);
    }

    /// <summary>Gets booking details by ID.</summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(BookingDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetBookingByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    /// <summary>Creates a new booking.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(BookingDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateBookingCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.BookingId }, result);
    }

    /// <summary>Updates an existing draft booking.</summary>
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(BookingDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateBookingCommand command, CancellationToken cancellationToken)
    {
        if (id != command.BookingId) return BadRequest("ID mismatch.");
        return Ok(await sender.Send(command, cancellationToken));
    }

    /// <summary>Submits a booking for approval.</summary>
    [HttpPost("{id:long}/submit")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Submit(long id, [FromQuery] long updatedBy, CancellationToken cancellationToken)
    {
        await sender.Send(new SubmitBookingCommand(id, updatedBy), cancellationToken);
        return NoContent();
    }

    /// <summary>Approves a submitted booking.</summary>
    [HttpPost("{id:long}/approve")]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Approve(long id, [FromQuery] long updatedBy, CancellationToken cancellationToken)
    {
        await sender.Send(new ApproveBookingCommand(id, updatedBy), cancellationToken);
        return NoContent();
    }

    /// <summary>Rejects a submitted booking.</summary>
    [HttpPost("{id:long}/reject")]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Reject(long id, [FromQuery] long updatedBy, CancellationToken cancellationToken)
    {
        await sender.Send(new RejectBookingCommand(id, updatedBy), cancellationToken);
        return NoContent();
    }

    /// <summary>Cancels a booking.</summary>
    [HttpPost("{id:long}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Cancel(long id, [FromQuery] long updatedBy, CancellationToken cancellationToken)
    {
        await sender.Send(new CancelBookingCommand(id, updatedBy), cancellationToken);
        return NoContent();
    }

    /// <summary>Gets all attendees for a booking.</summary>
    [HttpGet("{id:long}/attendees")]
    [ProducesResponseType(typeof(IEnumerable<AttendeeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAttendees(long id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetBookingAttendeesQuery(id), cancellationToken);
        return Ok(result);
    }

    /// <summary>Adds an attendee to a booking.</summary>
    [HttpPost("{id:long}/attendees")]
    [ProducesResponseType(typeof(AttendeeDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> AddAttendee(long id, [FromBody] AddAttendeeCommand command, CancellationToken cancellationToken)
    {
        if (id != command.BookingId) return BadRequest("ID mismatch.");
        var result = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetAttendees), new { id }, result);
    }

    /// <summary>Removes (cancels) an attendee from a booking.</summary>
    [HttpDelete("{id:long}/attendees/{attendeeSysId:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemoveAttendee(long id, long attendeeSysId, [FromQuery] long updatedBy, CancellationToken cancellationToken)
    {
        await sender.Send(new RemoveAttendeeCommand(id, attendeeSysId, updatedBy), cancellationToken);
        return NoContent();
    }
}
