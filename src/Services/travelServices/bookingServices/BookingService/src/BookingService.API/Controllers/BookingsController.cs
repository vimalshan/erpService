using BookingService.Application.Commands.CancelBooking;
using BookingService.Application.Commands.ConfirmBooking;
using BookingService.Application.Commands.CreateBooking;
using BookingService.Application.DTOs;
using BookingService.Application.Exceptions;
using BookingService.Application.Queries.GetBookingDetails;
using BookingService.Application.Queries.GetBookingList;
using BookingService.Infrastructure.DapperRepositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class BookingsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IBookingReadRepository _readRepository;

    public BookingsController(IMediator mediator, IBookingReadRepository readRepository)
    {
        _mediator = mediator;
        _readRepository = readRepository;
    }

    /// <summary>Get a booking by number.</summary>
    [HttpGet("{bookingNumber:long}")]
    [ProducesResponseType(typeof(BookingRequestDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBooking(long bookingNumber, CancellationToken ct)
    {
        var result = await _readRepository.GetBookingDetailsAsync(bookingNumber, ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Get confirmation details.</summary>
    [HttpGet("confirmations/{confirmationNumber:long}")]
    [ProducesResponseType(typeof(BookingConfirmationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetConfirmation(long confirmationNumber, CancellationToken ct)
    {
        var result = await _readRepository.GetConfirmationDetailsAsync(confirmationNumber, ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>List bookings for a user with paging.</summary>
    [HttpGet("user/{userCode}")]
    [ProducesResponseType(typeof(IEnumerable<BookingListDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserBookings(
        string userCode,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetBookingListQuery(userCode, page, pageSize), ct);
        return Ok(result);
    }

    /// <summary>Create a new booking request.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequestDto dto, CancellationToken ct)
    {
        var bookingNumber = await _mediator.Send(new CreateBookingCommand(dto), ct);
        return CreatedAtAction(nameof(GetBooking), new { bookingNumber }, new { bookingNumber });
    }

    /// <summary>Confirm a booking request.</summary>
    [HttpPost("{bookingNumber:long}/confirm")]
    [Authorize(Roles = "Admin,TravelDesk")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConfirmBooking(long bookingNumber, [FromBody] ConfirmBookingRequestDto dto, CancellationToken ct)
    {
        var confirmationNumber = await _mediator.Send(
            new ConfirmBookingCommand(bookingNumber, dto.ModeOfTravel, dto.VendorCode, dto.TicketNumber, dto.AdminRemarks), ct);
        return Ok(new { confirmationNumber });
    }

    /// <summary>Cancel a booking.</summary>
    [HttpPost("{bookingNumber:long}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelBooking(long bookingNumber, [FromBody] CancelBookingRequestDto dto, CancellationToken ct)
    {
        await _mediator.Send(new CancelBookingCommand(bookingNumber, dto.CancellationRemarks, dto.CancelledBy), ct);
        return NoContent();
    }
}
