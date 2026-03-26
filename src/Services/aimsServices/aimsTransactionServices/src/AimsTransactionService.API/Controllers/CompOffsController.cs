using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AimsTransactionService.Application.CompOffs.Commands.RequestCompOff;
using AimsTransactionService.Application.CompOffs.Queries.GetCompOffsByEmployee;

namespace AimsTransactionService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class CompOffsController(ISender sender) : ControllerBase
{
    [HttpGet("employee/{employeeSysId:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEmployee(long employeeSysId, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetCompOffsByEmployeeQuery(employeeSysId), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RequestCompOff([FromBody] RequestCompOffCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, result);
    }
}
