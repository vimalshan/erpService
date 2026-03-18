using BankService.Application.Commands.ChequeRegisters;
using BankService.Application.Commands.Reconciliations;
using BankService.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChequeRegistersController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ChequeRegisterDto>> Create([FromBody] CreateChequeRegisterCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return Ok(result);
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReconciliationsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<PaymentReconciliationDto>> Create([FromBody] CreateReconciliationCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return Ok(result);
    }
}
