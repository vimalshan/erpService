using BusServices.Application.DeductionRates.Commands;
using BusServices.Application.DTOs;
using BusServices.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusServices.API.Controllers;

[ApiController]
[Route("api/buses/{busId:int}/deductions")]
[Authorize]
public sealed class DeductionRatesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IBusDeductionRateRepository _rateRepo;

    public DeductionRatesController(IMediator mediator, IBusDeductionRateRepository rateRepo)
    {
        _mediator = mediator;
        _rateRepo = rateRepo;
    }

    /// <summary>Get all deduction rates for a bus.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BusDeductionRateDto>), 200)]
    public async Task<IActionResult> GetByBus(int busId, CancellationToken ct)
    {
        var rates = await _rateRepo.GetByBusIdAsync(busId, ct);
        return Ok(rates.Select(r => new BusDeductionRateDto(r.DeductId, r.BusId, r.Amount,
            r.EffectiveDate, r.ClosingDate, r.LastModifiedBy, r.LastModifiedOn)));
    }

    /// <summary>Set a deduction rate for a bus.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(BusDeductionRateDto), 201)]
    public async Task<IActionResult> Set(int busId, [FromBody] SetDeductionRateBody body, CancellationToken ct)
    {
        var result = await _mediator.Send(
            new SetDeductionRateCommand(busId, body.Amount, body.EffectiveDate, body.CreatedBy), ct);
        return StatusCode(201, result);
    }
}

public record SetDeductionRateBody(decimal Amount, DateTime EffectiveDate, long CreatedBy);
