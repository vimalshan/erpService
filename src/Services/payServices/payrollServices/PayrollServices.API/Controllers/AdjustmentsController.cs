using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayrollServices.Application.Commands;

namespace PayrollServices.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AdjustmentsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AdjustmentsController> _logger;

    public AdjustmentsController(IMediator mediator, ILogger<AdjustmentsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult> CreateAdjustment([FromBody] CreatePayrollAdjustmentCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(CreateAdjustment), result);
    }
}
