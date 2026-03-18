using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SettlementService.Application.Commands.AddDeduction;
using SettlementService.Application.Commands.AddPayment;
using SettlementService.Application.Commands.ApproveSettlement;
using SettlementService.Application.Commands.CreateSettlement;
using SettlementService.Application.Commands.RejectSettlement;
using SettlementService.Application.DTOs;
using SettlementService.Application.Queries.GetSettlement;
using SettlementService.Application.Queries.GetSettlements;
using SettlementService.Application.Queries.GetSettlementsByMember;

namespace SettlementService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SettlementsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SettlementsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SettlementDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetSettlementsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(SettlementDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetSettlementQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("member/{memberNo:long}")]
    [ProducesResponseType(typeof(IEnumerable<SettlementDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByMember(long memberNo, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetSettlementsByMemberQuery(memberNo), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(SettlementDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateSettlementCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.SettlementNumber }, result);
    }

    [HttpPost("{id:long}/approve")]
    [ProducesResponseType(typeof(SettlementDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Approve(long id, [FromBody] ApproveSettlementRequest request, CancellationToken cancellationToken)
    {
        var command = new ApproveSettlementCommand
        {
            SettlementNumber = id,
            ApprovedBy = request.ApprovedBy,
            Remarks = request.Remarks
        };
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:long}/reject")]
    [ProducesResponseType(typeof(SettlementDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Reject(long id, [FromBody] RejectSettlementRequest request, CancellationToken cancellationToken)
    {
        var command = new RejectSettlementCommand
        {
            SettlementNumber = id,
            RejectedBy = request.RejectedBy,
            Remarks = request.Remarks
        };
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:long}/deductions")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AddDeduction(long id, [FromBody] AddDeductionRequest request, CancellationToken cancellationToken)
    {
        var command = new AddDeductionCommand
        {
            SettlementNumber = id,
            DeductionType = request.DeductionType,
            Amount = request.Amount
        };
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:long}/payments")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AddPayment(long id, [FromBody] AddPaymentRequest request, CancellationToken cancellationToken)
    {
        var command = new AddPaymentCommand
        {
            SettlementNumber = id,
            PaymentMode = request.PaymentMode,
            Amount = request.Amount,
            ReferenceNo = request.ReferenceNo
        };
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}

// Request models for the controller
public record ApproveSettlementRequest(long ApprovedBy, string? Remarks);
public record RejectSettlementRequest(long RejectedBy, string? Remarks);
public record AddDeductionRequest(string DeductionType, decimal Amount);
public record AddPaymentRequest(string PaymentMode, decimal Amount, string? ReferenceNo);
