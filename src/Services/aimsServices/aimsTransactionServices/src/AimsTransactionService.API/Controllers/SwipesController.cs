using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AimsTransactionService.Application.Swipes.Commands.RecordSwipe;
using AimsTransactionService.Application.Swipes.Queries.GetSwipesByEmployee;

namespace AimsTransactionService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class SwipesController(ISender sender) : ControllerBase
{
    [HttpGet("employee/{employeeSysId:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEmployee(
        long employeeSysId, [FromQuery] DateTime fromDate, [FromQuery] DateTime toDate, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetSwipesByEmployeeQuery(employeeSysId, fromDate, toDate), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Record([FromBody] RecordSwipeCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, result);
    }
}
