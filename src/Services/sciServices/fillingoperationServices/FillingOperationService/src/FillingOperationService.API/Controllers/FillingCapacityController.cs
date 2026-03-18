using FillingOperationService.Application.FillingCapacities.Queries.GetFillingCapacity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FillingOperationService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FillingCapacityController(IMediator mediator) : ControllerBase
{
    [HttpGet("{groupId:int}")]
    public async Task<IActionResult> GetCapacity(int groupId, [FromQuery] int? productId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetFillingCapacityQuery(groupId, productId), ct);
        return Ok(result);
    }
}
