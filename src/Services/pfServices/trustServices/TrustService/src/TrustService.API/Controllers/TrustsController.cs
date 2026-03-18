using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrustService.Application.DTOs;
using TrustService.Application.Features.Trusts.Commands;
using TrustService.Application.Features.Trusts.Queries;

namespace TrustService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TrustsController : ControllerBase
{
    private readonly ISender _mediator;

    public TrustsController(ISender mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Get all trusts</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<TrustMasterDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllTrustsQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>Get active trusts</summary>
    [HttpGet("active")]
    [ProducesResponseType(typeof(IReadOnlyList<TrustMasterDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActive(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetActiveTrustsQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>Get trust by code</summary>
    [HttpGet("{trustCode}")]
    [ProducesResponseType(typeof(TrustMasterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByCode(string trustCode, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTrustByCodeQuery(trustCode), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Get trusts via Dapper (read-optimized)</summary>
    [HttpGet("dapper")]
    [ProducesResponseType(typeof(IReadOnlyList<TrustMasterDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByDapper([FromQuery] string? statusFilter, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTrustsByDapperQuery(statusFilter), cancellationToken);
        return Ok(result);
    }

    /// <summary>Create a new trust</summary>
    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateTrustCommand command, CancellationToken cancellationToken)
    {
        var trustCode = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetByCode), new { trustCode }, trustCode);
    }

    /// <summary>Update a trust</summary>
    [HttpPut("{trustCode}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(string trustCode, [FromBody] UpdateTrustCommand command,
        CancellationToken cancellationToken)
    {
        if (trustCode != command.TrustCode)
            return BadRequest("Trust code mismatch.");

        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>Close a trust</summary>
    [HttpPost("{trustCode}/close")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Close(string trustCode, [FromBody] DateTime closureDate,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new CloseTrustCommand(trustCode, closureDate), cancellationToken);
        return NoContent();
    }

    /// <summary>Activate a trust</summary>
    [HttpPost("{trustCode}/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Activate(string trustCode, CancellationToken cancellationToken)
    {
        await _mediator.Send(new ActivateTrustCommand(trustCode), cancellationToken);
        return NoContent();
    }

    /// <summary>Add fund type to trust</summary>
    [HttpPost("{trustCode}/fund-types")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AddFundType(string trustCode, [FromBody] AddTrustFundTypeCommand command,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(command with { TrustCode = trustCode }, cancellationToken);
        return NoContent();
    }

    /// <summary>Add role to trust</summary>
    [HttpPost("{trustCode}/roles")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AddRole(string trustCode, [FromBody] AddTrustRoleCommand command,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(command with { TrustCode = trustCode }, cancellationToken);
        return NoContent();
    }

    /// <summary>Add unit to trust</summary>
    [HttpPost("{trustCode}/units")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AddUnit(string trustCode, [FromBody] AddTrustUnitCommand command,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(command with { TrustCode = trustCode }, cancellationToken);
        return NoContent();
    }

    /// <summary>Add approver to trust</summary>
    [HttpPost("{trustCode}/approvers")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AddApprover(string trustCode, [FromBody] AddTrustApproverCommand command,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(command with { TrustCode = trustCode }, cancellationToken);
        return NoContent();
    }

    /// <summary>Add configuration to trust</summary>
    [HttpPost("{trustCode}/configurations")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AddConfiguration(string trustCode, [FromBody] AddTrustConfigurationCommand command,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(command with { TrustCode = trustCode }, cancellationToken);
        return NoContent();
    }
}
