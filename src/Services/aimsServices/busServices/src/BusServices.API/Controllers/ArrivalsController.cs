using BusServices.Application.Arrivals.Commands;
using BusServices.Application.Arrivals.Queries;
using BusServices.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusServices.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class ArrivalsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ArrivalsController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get arrivals for a bus.</summary>
    [HttpGet("bus/{busId:int}")]
    [ProducesResponseType(typeof(IEnumerable<BusArrivalDto>), 200)]
    public async Task<IActionResult> GetByBus(int busId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetArrivalsByBusQuery(busId), ct));

    /// <summary>Get arrivals for a date.</summary>
    [HttpGet("date/{date:datetime}")]
    [ProducesResponseType(typeof(IEnumerable<BusArrivalDto>), 200)]
    public async Task<IActionResult> GetByDate(DateTime date, CancellationToken ct)
        => Ok(await _mediator.Send(new GetArrivalsByDateQuery(date), ct));

    /// <summary>Record a new bus arrival.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(BusArrivalDto), 201)]
    public async Task<IActionResult> Record([FromBody] RecordArrivalBody body, CancellationToken ct)
    {
        var time = TimeOnly.Parse(body.ArrivalTime);
        var result = await _mediator.Send(
            new RecordArrivalCommand(body.BusId, body.ArrivalDate, time, body.Status[0], body.Remarks, body.RecordedBy), ct);
        return StatusCode(201, result);
    }
}

public record RecordArrivalBody(int BusId, DateTime ArrivalDate, string ArrivalTime, string Status, string? Remarks, long RecordedBy);
