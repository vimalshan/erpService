using ArchiveService.Application.DTOs;
using ArchiveService.Application.Features.ServiceOrders.Commands;
using ArchiveService.Application.Features.ServiceOrders.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArchiveService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ServiceOrdersController(IMediator mediator) : ControllerBase
{
    [HttpGet("{sernoDell}")]
    public async Task<ActionResult<ServiceOrderDto>> GetById(string sernoDell)
    {
        var result = await mediator.Send(new GetServiceOrderByIdQuery(sernoDell));
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<ServiceOrderDto>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await mediator.Send(new GetServiceOrdersPagedQuery(page, pageSize));
        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IReadOnlyList<ServiceOrderDto>>> Search(
        [FromQuery] string? branch,
        [FromQuery] string? engineerId,
        [FromQuery] string? callStatus,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate)
    {
        var result = await mediator.Send(new SearchServiceOrdersQuery(branch, engineerId, callStatus, fromDate, toDate));
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<string>> Create([FromBody] CreateServiceOrderCommand command)
    {
        var sernoDell = await mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { sernoDell }, sernoDell);
    }

    [HttpPut("{sernoDell}/status")]
    public async Task<IActionResult> UpdateStatus(string sernoDell, [FromBody] UpdateServiceOrderStatusCommand command)
    {
        if (sernoDell != command.SernoDell) return BadRequest("SernoDell mismatch");
        var result = await mediator.Send(command);
        return result ? NoContent() : NotFound();
    }

    [HttpDelete("{sernoDell}")]
    public async Task<IActionResult> Delete(string sernoDell)
    {
        var result = await mediator.Send(new DeleteServiceOrderCommand(sernoDell));
        return result ? NoContent() : NotFound();
    }
}
