using ComplaintService.Application.Commands.CloseComplaint;
using ComplaintService.Application.Commands.CreateComplaint;
using ComplaintService.Application.Commands.ReopenComplaint;
using ComplaintService.Application.DTOs;
using ComplaintService.Application.Queries.GetAllComplaints;
using ComplaintService.Application.Queries.GetComplaintById;
using ComplaintService.Application.Queries.GetComplaintsByGroup;
using ComplaintService.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComplaintService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ComplaintsController(ISender mediator, DapperComplaintRepository dapperRepo) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IEnumerable<ComplaintTicketDto>>(200)]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetAllComplaintsQuery(page, pageSize), ct);
        return Ok(result);
    }

    [HttpGet("{ticketNum:decimal}")]
    [ProducesResponseType<ComplaintTicketDto>(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(decimal ticketNum, CancellationToken ct)
    {
        var result = await mediator.Send(new GetComplaintByIdQuery(ticketNum), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("group/{groupId:decimal}")]
    [ProducesResponseType<IEnumerable<ComplaintTicketDto>>(200)]
    public async Task<IActionResult> GetByGroup(decimal groupId, CancellationToken ct) =>
        Ok(await mediator.Send(new GetComplaintsByGroupQuery(groupId), ct));

    [HttpGet("{ticketNum:decimal}/status")]
    [ProducesResponseType<string>(200)]
    public async Task<IActionResult> GetStatus(decimal ticketNum, CancellationToken ct) =>
        Ok(await dapperRepo.GetComplaintStatusAsync(ticketNum, ct));

    [HttpPost]
    [ProducesResponseType<decimal>(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] CreateComplaintRequest request, CancellationToken ct)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        decimal.TryParse(userIdClaim, out var userId);

        var ticketNum = await mediator.Send(new CreateComplaintCommand(
            request.GroupId, request.Type, request.Location, request.Department,
            request.Process, request.Subject, request.Description, request.IsNCR,
            request.TargetResolutionHours, userId), ct);

        return CreatedAtAction(nameof(GetById), new { ticketNum }, ticketNum);
    }

    [HttpPost("{ticketNum:decimal}/close")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Close(decimal ticketNum, [FromBody] CloseComplaintRequest request, CancellationToken ct)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        decimal.TryParse(userIdClaim, out var userId);
        await mediator.Send(new CloseComplaintCommand(ticketNum, request.FinalRemarks, userId), ct);
        return NoContent();
    }

    [HttpPost("{ticketNum:decimal}/reopen")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Reopen(decimal ticketNum, [FromBody] ReopenComplaintRequest request, CancellationToken ct)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        decimal.TryParse(userIdClaim, out var userId);
        await mediator.Send(new ReopenComplaintCommand(ticketNum, request.Remarks, userId), ct);
        return NoContent();
    }
}
