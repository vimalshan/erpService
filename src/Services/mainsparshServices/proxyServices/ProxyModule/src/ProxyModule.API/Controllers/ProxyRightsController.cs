using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProxyModule.Application.Commands.CreateProxyRight;
using ProxyModule.Application.Commands.DeactivateProxyRight;
using ProxyModule.Application.Commands.UpdateProxyRight;
using ProxyModule.Application.DTOs;
using ProxyModule.Application.Queries.GetActiveProxyRights;
using ProxyModule.Application.Queries.GetProxyRightById;
using ProxyModule.Application.Queries.GetProxyRightsByUser;

namespace ProxyModule.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProxyRightsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProxyRightsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ProxyRightDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetProxyRightByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("user/{proxyUserId:long}")]
    [ProducesResponseType(typeof(IEnumerable<ProxyRightDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUser(long proxyUserId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetProxyRightsByUserQuery(proxyUserId), ct);
        return Ok(result);
    }

    [HttpGet("active")]
    [ProducesResponseType(typeof(IEnumerable<ProxyRightDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActive(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetActiveProxyRightsQuery(), ct);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProxyRightDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateProxyRightDto dto, CancellationToken ct)
    {
        var command = new CreateProxyRightCommand(
            dto.ProxyUserId, dto.DelegatedUserId, dto.ProxyStartDate,
            dto.ProxyEndDate, dto.ProxyType, dto.Scope, dto.Notes, dto.CreatedBy);

        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.ProxyId }, result);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ProxyRightDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateProxyRightDto dto, CancellationToken ct)
    {
        var command = new UpdateProxyRightCommand(
            id, dto.ProxyStartDate, dto.ProxyEndDate, dto.ProxyType,
            dto.Scope, dto.Notes, dto.UpdatedBy);

        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(long id, [FromQuery] long updatedBy, CancellationToken ct)
    {
        await _mediator.Send(new DeactivateProxyRightCommand(id, updatedBy), ct);
        return NoContent();
    }
}
