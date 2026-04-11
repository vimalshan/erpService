using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransactionService.Application.EmployeePayments;

namespace TransactionService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public sealed class EmployeePaymentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeePaymentsController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetEmployeePaymentByIdQuery(id), ct));

    [HttpGet("by-employee/{empSysId:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEmployee(long empSysId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetEmployeePaymentsByEmployeeQuery(empSysId), ct));

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateEmployeePaymentCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.PayId }, result);
    }
}
