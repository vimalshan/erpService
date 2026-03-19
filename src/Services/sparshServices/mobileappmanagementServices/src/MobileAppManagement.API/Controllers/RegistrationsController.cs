using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileAppManagement.Application.Commands;
using MobileAppManagement.Application.Queries;

namespace MobileAppManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RegistrationsController(IMediator mediator) : ControllerBase
{
    [HttpGet("{registrationId}")]
    public async Task<IActionResult> GetById(long registrationId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetRegistrationByIdQuery(registrationId), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserId(string userId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetRegistrationsByUserIdQuery(userId), ct);
        return Ok(result);
    }

    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetByStatus(string status, CancellationToken ct)
    {
        var result = await mediator.Send(new GetRegistrationsByStatusQuery(status), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRegistrationCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { registrationId = result.RegistrationId }, result);
    }

    [HttpPut("{registrationId}/status")]
    public async Task<IActionResult> UpdateStatus(long registrationId,
        [FromBody] UpdateStatusRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new UpdateRegistrationStatusCommand(registrationId, request.Status ?? ""), ct);
        return Ok(new { message = result });
    }

    [HttpPost("{registrationId}/generate-pin")]
    public async Task<IActionResult> GeneratePin(long registrationId, CancellationToken ct)
    {
        var pin = await mediator.Send(new GenerateRegistrationPinCommand(registrationId), ct);
        return Ok(new { pin });
    }
}

public record UpdateStatusRequest(string? Status);
