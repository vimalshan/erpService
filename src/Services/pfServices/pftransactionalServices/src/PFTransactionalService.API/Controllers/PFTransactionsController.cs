using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PFTransactionalService.Application.Commands.ApplyInterest;
using PFTransactionalService.Application.Commands.GenerateCertificate;
using PFTransactionalService.Application.Commands.ProcessContribution;
using PFTransactionalService.Application.Commands.ProcessWithdrawal;
using PFTransactionalService.Application.DTOs;
using PFTransactionalService.Application.Queries.GetAccumulation;
using PFTransactionalService.Application.Queries.GetAccumulations;
using PFTransactionalService.Application.Queries.GetSettlements;

namespace PFTransactionalService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PFTransactionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PFTransactionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("accumulations")]
    [ProducesResponseType(typeof(IEnumerable<PFAccumulationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAccumulations(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAccumulationsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("accumulations/{empSysId:long}")]
    [ProducesResponseType(typeof(PFAccumulationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAccumulation(long empSysId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAccumulationQuery(empSysId), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("contributions")]
    [ProducesResponseType(typeof(PFAccumulationDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ProcessContribution([FromBody] ProcessContributionCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetAccumulation), new { empSysId = result.EmpSysId }, result);
    }

    [HttpPost("withdrawals")]
    [ProducesResponseType(typeof(PFAccumulationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ProcessWithdrawal([FromBody] ProcessWithdrawalCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("interest")]
    [ProducesResponseType(typeof(PFAccumulationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ApplyInterest([FromBody] ApplyInterestCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("certificates")]
    [ProducesResponseType(typeof(WithdrawalCertificateDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GenerateCertificate([FromBody] GenerateCertificateCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Created($"/api/pftransactions/certificates/{result.CertificateId}", result);
    }

    [HttpGet("settlements")]
    [ProducesResponseType(typeof(IEnumerable<PFSettlementDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSettlements(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPFSettlementsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("settlements/employee/{empSysId:long}")]
    [ProducesResponseType(typeof(IEnumerable<PFSettlementDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSettlementsByEmployee(long empSysId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPFSettlementsByEmpQuery(empSysId), cancellationToken);
        return Ok(result);
    }
}
