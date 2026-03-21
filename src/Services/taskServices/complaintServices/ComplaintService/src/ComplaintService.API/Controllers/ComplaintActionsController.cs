using ComplaintService.Application.Commands.UpdateAction;
using ComplaintService.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComplaintService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ComplaintActionsController(ISender mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> UpdateAction([FromBody] UpdateActionRequest request, CancellationToken ct)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        decimal.TryParse(userIdClaim, out var userId);
        await mediator.Send(new UpdateActionCommand(request.ActionNum, request.ActionLevel, request.Solution, userId), ct);
        return NoContent();
    }
}
