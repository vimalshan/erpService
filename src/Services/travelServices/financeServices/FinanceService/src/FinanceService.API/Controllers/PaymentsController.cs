using FinanceService.Application.DTOs;
using FinanceService.Application.Features.Payments.Commands.ProcessPayment;
using FinanceService.Application.Features.Payments.Queries.GetPaymentDetails;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<List<PaymentDto>>> GetAll([FromQuery] string? unitCode, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetPaymentDetailsQuery { UnitCode = unitCode }, ct);
        return Ok(result);
    }

    [HttpPost("process")]
    public async Task<ActionResult<PaymentDto>> Process([FromBody] ProcessPaymentCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }
}
