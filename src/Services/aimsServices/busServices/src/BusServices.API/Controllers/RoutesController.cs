using BusServices.Application.Routes.Commands;
using BusServices.Application.Routes.Queries;
using BusServices.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusServices.API.Controllers;

[ApiController]
[Route("api/buses/{busId:int}/routes")]
[Authorize]
public sealed class RoutesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoutesController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get all routes for a bus.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BusRouteDto>), 200)]
    public async Task<IActionResult> GetByBus(int busId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetRoutesByBusQuery(busId), ct));

    /// <summary>Create a new route for a bus.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(BusRouteDto), 201)]
    public async Task<IActionResult> Create(int busId, [FromBody] CreateRouteBody body, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateRouteCommand(busId, body.Name, body.Description, body.CreatedBy), ct);
        return StatusCode(201, result);
    }
}

public record CreateRouteBody(string Name, string? Description, long CreatedBy);
