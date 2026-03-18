using DemandManagement.Application.Commands;
using DemandManagement.Application.DTOs;
using DemandManagement.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemandManagement.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DemandController : ControllerBase
{
    private readonly IMediator _mediator;

    public DemandController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll() =>
        Ok(await _mediator.Send(new GetAllDemandsQuery()));

    [HttpGet("{id:long}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _mediator.Send(new GetDemandByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDemandRequest request)
    {
        var id = await _mediator.Send(new CreateDemandCommand(request));
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:long}/approve")]
    public async Task<IActionResult> Approve(long id, [FromBody] ApprovalRequest request)
    {
        var success = await _mediator.Send(new ApproveDemandCommand(id, request.UserId, request.Remarks));
        return success ? Ok() : NotFound();
    }

    [HttpPut("{id:long}/reject")]
    public async Task<IActionResult> Reject(long id, [FromBody] ApprovalRequest request)
    {
        var success = await _mediator.Send(new RejectDemandCommand(id, request.UserId, request.Remarks));
        return success ? Ok() : NotFound();
    }

    [HttpPut("{id:long}/complete")]
    public async Task<IActionResult> Complete(long id, [FromBody] ApprovalRequest request)
    {
        var success = await _mediator.Send(new CompleteDemandCommand(id, request.UserId, request.Remarks));
        return success ? Ok() : NotFound();
    }
}

public record ApprovalRequest(long UserId, string Remarks);
